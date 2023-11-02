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
    }
}
