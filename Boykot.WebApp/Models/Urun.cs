namespace Boykot.WebApp.Models
{
    public class Urun
    {
        public int Id { get; set; }
        public string Adi { get; set; }
        public string Kodu { get; set; }
        public string Barkod { get; set; }
        public string Firma { get; set; }
        public string Ulke { get; set; }
        public string Marka { get; set; }
        public string Aciklama { get; set; }
        public string Not1 { get; set; }
        public string Not2 { get; set; }
        public string Resim { get; set; }
        public bool Aktifmi { get; set; } = true;
        public string Aciklama1 { get; set; }
        public string Aciklama2 { get; set; }
        public string Aciklama3 { get; set; }
    }
}
