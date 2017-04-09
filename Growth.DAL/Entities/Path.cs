using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Growth.DAL.Entities
{
    [BsonIgnoreExtraElements]
    public class Path : BaseType
    {
        public Path()
        {
            Goals = new List<Goal>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public IList<Goal> Goals { get; set; }

        public override string CollectionName => "Paths";
    }
}