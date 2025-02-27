﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace minimalAPIMongo.Domains
{
    public class Client
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId"), BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("cpf")]
        public string Cpf { get; set;}

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("adress")]
        public string Adress { get; set;}

        //[BsonElement("additionalAttributes")]
        public Dictionary<string, string> AdditionalAttributes { get; set; }

        public Client()
        {
            AdditionalAttributes = new Dictionary<string, string>();
        }
    }
}
