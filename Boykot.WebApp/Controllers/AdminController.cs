﻿using Boykot.WebApp.Models;
using Boykot.WebApp.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using Boykot.WebApp.Models.Response;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Data.OleDb;


namespace Boykot.WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BoykotDbContext _boykotDbContext;
        private IHostingEnvironment Environment;
        private IConfiguration Configuration;
        public AdminController(
            ILogger<HomeController> logger, 
            BoykotDbContext boykotDbContext = null,
            IHostingEnvironment _environment=null, IConfiguration _configuration=null)
        {
            _logger = logger;
            _boykotDbContext = boykotDbContext;
            Environment = _environment;
            Configuration = _configuration;
        }
        /// <summary>
        /// User Login - Get
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("user")))
                return RedirectToAction("List");

            return View();
        }
        /// <summary>
        /// User Login - Post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(UserLoginRequestModel model)
        {
            if (model is null)
            {
                ViewBag.Message = "Bilgiler Bos Gecilemez!";
                return View();
            }

            var user = _boykotDbContext.Users
                .Where(x => x.IsActive && x.UserName == model.UserName.Trim() && x.Password == model.Password)
                .FirstOrDefault();
            if (user is null)
            {
                ViewBag.Message = "Kullanici Adi veya Sifre Hatali!";
                return View();
            }

            HttpContext.Session.SetString("user",model.UserName);
            return RedirectToAction("List");
        }

        /// <summary>
        /// Product List
        /// </summary>
        /// <returns></returns>
        public IActionResult List()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("user")))
                return RedirectToAction("Index");

            return View();
        }

        [HttpGet]
        public IActionResult CreateOrUpdate(int? id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("user")))
                return RedirectToAction("Index");

            if (id.HasValue)
            {
                var product = _boykotDbContext.Uruns
                    .Where(x => x.Id == id && x.Aktifmi)
                    .FirstOrDefault();

                if (product is null)
                    return View();

                var response = new UrunResponseModel
                {
                    Adi = product.Adi,
                    Barkod = product.Barkod,
                    Firma = product.Firma,
                    Id = product.Id,
                    Kodu = product.Kodu,
                    Ulke = product.Ulke,
                    Not1 = product.Not1,
                    Not2 = product.Not2,
                    Aciklama = product.Aciklama,
                    Resim = product.Resim,
                    Marka = product.Marka
                };

                return View(response);
            }
            return View();
        }
        /// <summary>
        /// Product Create or Update with Post Op.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(IFormFile file, CreateUrunRequestModel model)
        {
            #region validation
            if (model is null)
                return null;

            //if (model.Barkod is null)
            //{
            //    ViewBag.Message = $"Barkod Bos Olamaz!!";
            //    return View();
            //}
            #endregion

            try
            {
                var product = new Urun()
                {
                    Aciklama = model.Aciklama,
                    Adi = model.Adi,
                    Barkod = model.Barkod,
                    Firma = model.Firma,
                    Kodu = model.Kodu,
                    Marka = model.Marka,
                    Not1 = model.Not1,
                    Not2 = model.Not2,
                    Ulke = model.Ulke,
                    Resim = $"/images/{ImageUpload(file).Result ?? "/images/no_product.png"}"
                };

                if (string.IsNullOrEmpty(model.Id.ToString()) || model.Id != 0)
                {
                    var hasproduct = _boykotDbContext.Uruns
                        .Where(x => x.Id == model.Id)
                        .FirstOrDefault();

                    hasproduct.Kodu = model.Kodu;
                    hasproduct.Ulke = model.Ulke;
                    hasproduct.Not1 = model.Not1;
                    hasproduct.Barkod = model.Barkod;
                    hasproduct.Firma = model.Firma;
                    hasproduct.Adi = model.Adi;
                    hasproduct.Marka = model.Marka;
                    hasproduct.Aciklama = model.Aciklama;
                    hasproduct.Not2 = model.Not2;
                    hasproduct.Resim = file?.FileName == null ? (model.Resim == null ? "/images/no_product.png"
                                                                                    : model.Resim)
                                                            : $"/images/{ImageUpload(file).Result ?? "/images/no_product.png"}";
                }
                else
                {
                    bool hasProduct = _boykotDbContext.Uruns.Any(x => x.Barkod == model.Barkod.Trim());
                    if (hasProduct)
                    {
                        ViewBag.Message = $"Bu Barkod Sistemde Kayitlidir! : {model.Barkod}";
                        return View();
                    }

                    await _boykotDbContext.Uruns.AddAsync(product);
                }

                await _boykotDbContext.SaveChangesAsync();

                ViewBag.Message = "islem basarili!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message.ToString();
            }

            return View();
        }
        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Delete(int? Id)
        {
            if (!Id.HasValue)
                return Json(null);

            var urun = await _boykotDbContext.Uruns
                .Where(x => x.Id == Id && x.Aktifmi == true)
                .FirstAsync();

            if (urun == null)
                return Json(null);

            urun.Aktifmi = false;
            var result = await _boykotDbContext.SaveChangesAsync();
            return Json(result);
        }

        #region private
        /// <summary>
        /// Image Upload
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> ImageUpload(IFormFile file)
        {
            if (file != null)
            {
                string imageExtension = Path.GetExtension(file.FileName);
                string imageName = Guid.NewGuid() + imageExtension;
                string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/{imageName}");
                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);
                return imageName;
            }

            return string.Empty;
        }
        [HttpGet]
        public IActionResult DocumentUpload()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("user")))
                return RedirectToAction("Index");
            return View();
        }
        [HttpPost]
        public IActionResult DocumentUpload(IFormFile postedFile)
        {
            string message = string.Empty;
            if (postedFile != null)
            {
                //Create a Folder.
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                string conString = this.Configuration.GetConnectionString("ExcelConString");
                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);

                #region Excel Read
                try
                {
                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();

                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                connExcel.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message.ToString();
                }

                #endregion
                #region Excel Data Insert
                try
                {
                    conString = this.Configuration.GetConnectionString("SqlConnection");
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            sqlBulkCopy.DestinationTableName = "dbo.Uruns";

                            sqlBulkCopy.ColumnMappings.Add("ProductName", "Adi");
                            sqlBulkCopy.ColumnMappings.Add("Kodu", "Kodu");
                            sqlBulkCopy.ColumnMappings.Add("Barcode", "Barkod");
                            sqlBulkCopy.ColumnMappings.Add("Firma", "Firma");
                            sqlBulkCopy.ColumnMappings.Add("Ulke", "Ulke");
                            sqlBulkCopy.ColumnMappings.Add("Marka", "Marka");
                            sqlBulkCopy.ColumnMappings.Add("Aciklama", "Aciklama");
                            sqlBulkCopy.ColumnMappings.Add("Aciklama1", "Aciklama1");
                            sqlBulkCopy.ColumnMappings.Add("Aciklama2", "Aciklama2");
                            sqlBulkCopy.ColumnMappings.Add("Aciklama3", "Aciklama3");
                            sqlBulkCopy.ColumnMappings.Add("Not1", "Not1");
                            sqlBulkCopy.ColumnMappings.Add("Not2", "Not2");
                            sqlBulkCopy.ColumnMappings.Add("Resim", "Resim");
                            sqlBulkCopy.ColumnMappings.Add("Aktifmi", "Aktifmi");

                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message.ToString();
                }
                #endregion
            }
            ViewBag.Message = "Islem Basarili";
            return View();
        }
        #endregion
    }
}
