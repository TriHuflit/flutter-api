﻿namespace Flutter.Backend.Service.Models.Requests
{
    public class SearchRequestProduct : PaginationRequest
    {


        /// <summary>
        /// Key search product
        /// </summary>
        public string KeySearch { get; set; }

        /// <summary>
        /// Category search product
        /// </summary>
        public string CategorySearch { get; set; }

        /// <summary>s
        /// Brand search product
        /// </summary>
        public string BrandSearch { get; set; }

        /// <summary>
        /// Filter price
        /// </summary>
        public int SortPrice { get; set; }


    }
}
