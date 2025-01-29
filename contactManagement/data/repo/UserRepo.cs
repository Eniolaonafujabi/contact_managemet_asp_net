using contactManagement.data.model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace contactManagement.data.repo;

public class UserRepo : IUserRepo
{
    private readonly IMongoClient _mongoClient;

    public UserRepo(IMongoClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public IMongoCollection<User> GetUserCollection(string databaseName)
    {
        var database = _mongoClient.GetDatabase("contact_management");
        IMongoCollection<User> collection = database.GetCollection<User>("users");
        if (collection == null) throw new NullReferenceException("Internal Error");
        return collection;
    }
    
    public async Task<User?> FindByIdAsync(string databaseName, string id)
    {
        var collection = GetUserCollection(databaseName);

        // Filter by string ID
        var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        var user = await collection.Find(filter).FirstOrDefaultAsync();
        if (user == null) throw new NullReferenceException("User Not Found");
        return user;
    }
}