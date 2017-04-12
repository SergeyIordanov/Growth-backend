using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Growth.DAL.Entities
{
    [BsonIgnoreExtraElements]
    public class Kid : BaseType
    {
        public Kid()
        {
            Paths = new List<Path>();
        }

        public string Name { get; set; }

        public string Gender { get; set; }

        public Guid UserId { get; set; }

        public string Photo { get; set; }

        public IList<Path> Paths { get; set; }

        public override string CollectionName => "kids";
    }
}