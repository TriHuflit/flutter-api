﻿using System;
using System.Collections.Generic;

namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoProductDetail : DtoAuditLogSystem
    {
        /// <summary>
        /// Id of product.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Ablert  of product.
        /// </summary>     
        public string Ablert { get; set; }

        /// <summary>
        /// Brand of product.
        /// </summary>
        public string BrandId { get; set; }
        public string BrandName { get; set; }

        /// <summary>
        /// Category of product
        /// </summary>
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

        /// <summary>
        /// Crytal of product.
        /// </summary>     
        public string Crytal { get; set; }

        /// <summary>
        /// Gets or sets the classify products.
        /// </summary>
        public IEnumerable<DtoClassifyProduct> ClassifyProducts { get; set; }

        /// <summary>
        /// Short  Description of product. | Rules : MaxLenght 50
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Price at least of product.
        /// </summary>
        public decimal FromPrice { get; set; }

        /// <summary>
        /// Feature of product
        /// </summary>     
        public List<string> Feature { get; set; }

        /// <summary>
        /// Guarantee of product
        /// </summary>     
        public int Guarantee { get; set; }

        /// <summary>
        /// Active product.
        /// </summary>
        public int? IsShow { get; set; }

        /// <summary>
        /// Machine of product.
        /// </summary>     
        public string Machine { get; set; }

        /// <summary>
        /// MadeIn of product.| Rules : MaxLenght 50
        /// </summary>     
        public string MadeIn { get; set; }

        /// <summary>
        /// Name of product | Rules : MaxLenght 50
        /// </summary>      
        public string Name { get; set; }
     
        /// <summary>
        /// Price at most of product.
        /// </summary>
        public decimal ToPrice { get; set; }

        /// <summary>
        /// Thumbnail image of product.
        /// </summary>
        public string Thumbnail { get; set; }
      
        /// <summary>
        /// WaterProof.
        /// </summary>     
        public string WaterProof{ get; set; }
    }
}
