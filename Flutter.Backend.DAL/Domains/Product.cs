using MongoDB.Bson;
using System.Collections.Generic;

namespace Flutter.Backend.DAL.Domains
{
    public class Product
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
