using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HelloBlazor.Data
{
    public class TodoItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; set; } = false;
    }
}