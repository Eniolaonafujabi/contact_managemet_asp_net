using contactManagement.data.model;
using MongoDB.Driver;

namespace contactManagement.data.repo;

public class ContactRepo : IContactRepo
{
    private readonly IMongoCollection<Contact> _contacts;

    public ContactRepo(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("contactManagement"); // Get database
        _contacts = database.GetCollection<Contact>("contacts"); // Get collection
    }
    
    public async Task<Contact?> AddContactAsync(Contact contact)
    {
        await _contacts.InsertOneAsync(contact); // Correct usage
        return contact;
    }

    public async Task<List<Contact>> FindContactByUserIdAsync(string requestUserId)
    {
        return await _contacts.Find<Contact>(contact => contact.UserId == requestUserId).ToListAsync();
    }

    public async Task<Contact> FindContactByIdAndUserIdAsync(string requestId, string requestUserId)
    {
        return await _contacts.Find(contact => contact.Id == requestId && contact.UserId == requestUserId).SingleOrDefaultAsync();
    }

    public async  Task<bool> DeleteByIdAndUserIdAsync(string requestId, string requestUserId)
    {
        var result = await _contacts.DeleteOneAsync(contact => contact.Id == requestId);
        return result.DeletedCount>0;
    }

    public async Task<bool> FindIfContactExitForThisUserAsync(string requestPhoneNumber, string requestUserId)
    {
        return await _contacts.Find(contact => contact.PhoneNumber == requestPhoneNumber && contact.UserId == requestUserId).AnyAsync();
    }
}