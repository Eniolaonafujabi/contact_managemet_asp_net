using contactManagement.data.model;
using contactManagement.data.repo;
using contactManagement.dto;
using contactManagement.dto.request;

namespace contactManagement.services;

public class ContactImplementation : IContactInterface
{
    
    private IContactRepo _contactRepo;

    public ContactImplementation(IContactRepo contactRepo)
    {
        _contactRepo = contactRepo;
    }

    public AddContactResponse CreateContact(AddContactRequest request)
    {
        Contact contact = new Contact();
        Mapper.map(contact, request);
        _contactRepo.AddContactAsync(contact);
        AddContactResponse response = new AddContactResponse();
        response.Message = "Success";
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
        Contact contact = await _contactRepo.FindContactByIdAndUserIdAsync(request.id,request.UserId);
        if (contact == null) throw new Exception("Contact does not exist");
        FindContactResponse response = new FindContactResponse();
        Mapper.map(contact, response);
        return response;
    }

    public DeleteContactResponse DeleteContact(FindContactRequest request)
    {
        _contactRepo.DeleteByIdAndUserIdAsync(request.id,request.UserId);
        DeleteContactResponse response = new DeleteContactResponse();
        response.message="Success";
        return response;
    }
}