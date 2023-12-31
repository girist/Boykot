﻿using System.Collections.Generic;

namespace Boykot.WebApp.Models.Response
{
    public record UrunResponseModel
    {
        public int Id { get; set; }
        public string Adi { get; init; }
        public string Kodu { get; init; }
        public string Barkod { get; init; }
        public string Firma { get; init; }
        public string Ulke { get; init; }
        public string KategoriAdi { get; init; }
        public string Aciklama { get; set; }
        public string Not1 { get; set; }
        public string Not2 { get; set; }
        public string Resim { get; set; }
        public string Marka { get; init; }
    }
}
