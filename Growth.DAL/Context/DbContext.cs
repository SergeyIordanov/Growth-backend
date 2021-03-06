﻿using Growth.DAL.Entities;
using Growth.DAL.Interfaces;
using MongoDB.Driver;

namespace Growth.DAL.Context
{
    public class DbContext : IDbContext
    {
        private readonly IMongoDatabase database;

        public DbContext()
        {
        }

        public DbContext(string connectionString)
        {
            var builder = new MongoUrlBuilder(connectionString);

            IMongoClient client = new MongoClient(connectionString);
            database = client.GetDatabase(builder.DatabaseName);
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>()
            where TEntity : BaseType, new()
        {
            return database.GetCollection<TEntity>(new TEntity().CollectionName);
        }
    }
}