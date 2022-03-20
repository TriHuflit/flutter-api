using MongoDB.Bson;

namespace Flutter.Backend.DAL.Domains
{
    public class ClassifyProduct : AuditLogSystem
    {

        /// <summary>
        /// ProductId
        /// </summary>
        public ObjectId ProductId { get; set; }

        /// <summary>
        /// Image
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// OriginalPrice
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// PromotionPrice
        /// </summary>
        public decimal PromotionPrice { get; set; }

        /// <summary>
        /// Stock
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// IsShow
        /// </summary>
        public int? IsShow { get; set; }

    }
}
