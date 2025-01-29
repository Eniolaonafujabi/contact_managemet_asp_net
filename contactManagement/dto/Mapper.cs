using contactManagement.data.model;
using contactManagement.dto.request;

namespace contactManagement.dto;

public static class Mapper
{
    public static void map(Contact contact, AddContactRequest request)
    {
        contact.FirstName = request.FirstName;
        contact.LastName = request.LastName;
        contact.Email = request.Email;
        contact.PhoneNumber = request.PhoneNumber;
    }

    public static void map(Contact contact, FindContactResponse response)
    {
        response.FirstName = contact.FirstName;
        response.LastName = contact.LastName;
        response.PhoneNumber = contact.PhoneNumber;
        response.Email = contact.Email;
    }
}