namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoCategory : DtoAuditLogSystem
    {
        public string Id { get; set; }

        public string Name { get; set; }    

        public int IsShow { get; set; }

        public string ImageCategory { get; set; }
    }
}
