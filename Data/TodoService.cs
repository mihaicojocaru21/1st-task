using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace HelloBlazor.Data
{
    public class TodoService
    {
        private readonly IMongoCollection<TodoItem> _collection;

        public TodoService(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<TodoItem>("TodoItems");
        }

        public async Task AddAsync(TodoItem item) => await _collection.InsertOneAsync(item);
        public async Task<List<TodoItem>> GetAllAsync() => await _collection.Find(_ => true).ToListAsync();
        public async Task UpdateStatusAsync(string id, bool isDone)
        {
            var filter = Builders<TodoItem>.Filter.Eq(t => t.Id, id);
            var update = Builders<TodoItem>.Update.Set(t => t.IsDone, isDone);
            await _collection.UpdateOneAsync(filter, update);
        }
        public async Task DeleteAsync(string id)
        {
            var filter = Builders<TodoItem>.Filter.Eq(t => t.Id, id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}