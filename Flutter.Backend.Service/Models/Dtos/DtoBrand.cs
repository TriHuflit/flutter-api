namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoBrand : DtoAuditLogSystem
    {
        public string Id { get; set; }

        public string CategoryId { get; set; }

        public string Name { get; set; }    

        public int IsShow { get; set; }

        public string ImageBrand { get; set; }
    }
}
