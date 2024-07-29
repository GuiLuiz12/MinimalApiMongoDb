using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace minimalAPIMongo.Domains
{
    public class Order
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("date")]
        public DateOnly? Date { get; set; }

        [BsonElement("status")]
        public string? Status { get; set; }

        [BsonElement("ProductId"), BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }

        [BsonElement("ClientId"), BsonRepresentation(BsonType.ObjectId)] 
        public string ClientId { get; set;}
    }
}
