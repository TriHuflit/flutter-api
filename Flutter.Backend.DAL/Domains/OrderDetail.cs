using MongoDB.Bson;

namespace Flutter.Backend.DAL.Domains
{
    public class OrderDetail
    {
        public ObjectId ClassifyProductId { get; set; }

        public string ProductName { get; set; }

        public string ClassifyProductName { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }
    }
}
