namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoBanner : DtoAuditLogSystem
    {
        public string Id { get; set; }

        public string ImageBanner { get; set; }

        public string Content { get; set; }

        public int IsShowOnMobile { get; set; }

        public string IdProduct { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }
    }
}
