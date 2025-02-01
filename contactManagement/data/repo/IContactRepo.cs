using contactManagement.data.model;
using MongoDB.Driver;

namespace contactManagement.data.repo;

public interface IContactRepo
{
    Task<Contact> AddContactAsync(Contact contact);
    Task<List<Contact>> FindContactByUserIdAsync(string requestUserId);
    Task<Contact> FindContactByIdAndUserIdAsync(string requestId, string requestUserId);
    Task<bool> DeleteByIdAndUserIdAsync(string requestId, string requestUserId);
   Task<bool> FindIfContactExitForThisUserAsync(string requestPhoneNumber, string requestUserId);
}
