namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoClassifyProduct : DtoAuditLogSystem
    {


        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        public string ClassifyProductId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        public string ProductId { get; set; }

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
