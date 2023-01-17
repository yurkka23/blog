using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace Blog.Domain.Models
{
    [CollectionName("BlogEntity")]
    public class MongoEntity
    {
        public Guid UserId { get; set; }
        [BsonId]
        public Guid EntityId { get; set; }
    }
}
