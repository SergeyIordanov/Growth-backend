using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Growth.DAL.Entities
{
    [BsonIgnoreExtraElements]
    public class Goal : BaseType
    {
        public Goal()
        {
            Steps = new List<Step>();
        }

        public string Title { get; set; }

        public bool Completed { get; set; }

        public int GoalYear { get; set; }

        public string GoalMonth { get; set; }

        public IEnumerable<Step> Steps { get; set; }

        public override string CollectionName => "goals";
    }
}