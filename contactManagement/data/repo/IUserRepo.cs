using contactManagement.data.model;
using MongoDB.Driver;

namespace contactManagement.data.repo;

public interface IUserRepo
{
    IMongoCollection<User> GetUserCollection(string databaseName);
    Task<User?> FindByIdAsync(string databaseName, string id);
}