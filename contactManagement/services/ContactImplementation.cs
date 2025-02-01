using contactManagement.data.model;
using contactManagement.data.repo;
using contactManagement.dto;
using contactManagement.dto.request;

namespace contactManagement.services;

public class ContactImplementation : IContactInterface
{
    
    private readonly IContactRepo _contactRepo;

    public ContactImplementation(IContactRepo contactRepo)
    {
        this._contactRepo = contactRepo;
    }

    public async Task<AddContactResponse> CreateContactAsync(AddContactRequest request)
    {
        Contact contact = new Contact();
        bool check = await _contactRepo.FindIfContactExitForThisUserAsync(request.PhoneNumber, request.userId);
        if (check) throw new ArgumentException("Contact already exists");
        Mapper.map(contact, request);
        contact = await _contactRepo.AddContactAsync(contact);
        AddContactResponse response = new AddContactResponse();
        response.Message = "Success";
        response.PhoneNumber = contact.PhoneNumber;
        response.Id = contact.Id;
        response.UserId = contact.UserId;
        response.Email = contact.Email;
        response.FirstName = contact.FirstName;
        response.LastName = contact.LastName;
        return response;
    }

    public async Task<List<FindContactResponse>> GetContactsByUserId(FindAllContactRequest request)
    {
        List<Contact> contacts = await _contactRepo.FindContactByUserIdAsync(request.UserId);
        if (contacts.Count == 0) throw new Exception("No contacts found");
        List<FindContactResponse> responses = new List<FindContactResponse>();
        foreach (var contact in contacts)
        {
            responses.Add(new FindContactResponse(contact.Id,contact.UserId,contact.Email,contact.FirstName,contact.LastName,contact.PhoneNumber));
        }
        return responses;
    }

    public async Task<FindContactResponse> GetContactByIdAndUserId(FindContactRequest request)
    {
        Contact contact = await _contactRepo.FindContactByIdAndUserIdAsync(request.Id,request.UserId);
        if (contact == null) throw new Exception("Contact does not exist");
        FindContactResponse response = new FindContactResponse();
        Mapper.map(contact, response);
        return response;
    }

    public DeleteContactResponse DeleteContact(FindContactRequest request)
    {
        var result = _contactRepo.DeleteByIdAndUserIdAsync(request.Id,request.UserId);
        DeleteContactResponse response = new DeleteContactResponse();
        if (result.Result)
        {
            response.message="Success";
        }
        return response;
    }
}