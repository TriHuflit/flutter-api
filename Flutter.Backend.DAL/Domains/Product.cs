using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Flutter.Backend.DAL.Domains
{
    public class Product : AuditLogSystem
    {
        /// <summary>
        /// IDCategory of product.
        /// </summary>
        public ObjectId CategoryId { get; set; }

        /// <summary>
        /// IDBrand of product.
        /// </summary>
        public ObjectId BrandId { get; set; }

        /// <summary>
        /// Name of product.| Rules : MaxLenght 50
        /// </summary>     
        public string Name { get; set; }

        /// <summary>
        /// Feature of product
        /// </summary>     
        public List<string> Feature { get; set; }

        /// <summary>
        /// Crytal of product.
        /// </summary>     
        public string Crytal { get; set; }

        /// <summary>
        /// Machine of product.
        /// </summary>     
        public string Machine { get; set; }

        /// <summary>
        /// Guarantee of product
        /// </summary>     
        public int Guarantee { get; set; }

        /// <summary>
        /// Ablert  of product.
        /// </summary>     
        public string Ablert { get; set; }

        /// <summary>
        /// WaterProof.
        /// </summary>     
        public ObjectId WaterProofId { get; set; }

        /// <summary>
        /// MadeIn of product.| Rules : MaxLenght 50
        /// </summary>     
        public string MadeIn { get; set; }

        /// <summary>
        /// Price at least of product.
        /// </summary>
        public decimal FromPrice { get; set; }

        /// <summary>
        /// Price at most of product.
        /// </summary>    
        public decimal ToPrice { get; set; }

        /// <summary>
        /// Short  Description of product. | Rules : MaxLenght 50
        /// </summary>
        
        public string Description { get; set; }

        /// <summary>
        /// Thumbnail image of product.
        /// </summary>

        public string Thumbnail { get; set; }

        /// <summary>
        /// Active product.
        /// </summary>
        public int? IsShow { get; set; }
    }
}
