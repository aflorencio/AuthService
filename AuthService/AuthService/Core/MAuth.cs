using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService
{
    [BsonIgnoreExtraElements]
    class MAuth
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonIgnoreIfNull]
        public string tokenActivo { get; set; }
        [BsonIgnoreIfNull]
        public string typeService { get; set; }
        [BsonIgnoreIfNull]
        public ObjectId serviceID { get; set; }
        [BsonIgnoreIfNull]
        public DateTime cadTokenTime { get; set; }
        [BsonIgnoreIfNull]
        public bool awollAccess { get; set; }
        [BsonIgnoreIfNull]
        public double rolAccess { get; set; }
        [BsonIgnoreIfNull]
        public int openAccess { get; set; }
        [BsonIgnoreIfNull]
        public int closeAccess { get; set; }
        [BsonIgnoreIfNull]
        public string ip { get; set; }
        [BsonIgnoreIfNull]
        public DateTime loginDate { get; set; }
        [BsonIgnoreIfNull]
        public string user { get; set; }
        [BsonIgnoreIfNull]
        public string pass { get; set; }
    }
}
