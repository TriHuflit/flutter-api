﻿using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;

namespace Flutter.Backend.DAL.Implementations
{
    public class BrandRespository : GenericMongoDB<Brand> , IBrandRepository
    {
        public BrandRespository(IMongoClient dbClient) : base(dbClient)
        {

        }
    }
}
