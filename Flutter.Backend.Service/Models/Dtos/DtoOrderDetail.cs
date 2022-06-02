namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoOrderDetail
    {
        public string ClassifyProductId { get; set; }

        public string ProductName { get; set; }

        public string ClassifyProductName { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }
    }
}
