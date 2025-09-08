using MongoDB.Driver;

namespace HelloBlazor.Data
{
    public class TodoService
    {
        private readonly IMongoCollection<TodoItem> _collection;

        // Constructor accepts connection string and database name
        public TodoService(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<TodoItem>("TodoItems");
        }

        public List<TodoItem> GetAll() => _collection.Find(_ => true).ToList();
        public void Add(TodoItem item) => _collection.InsertOne(item);
    }
}