using contactManagement.dto;
using contactManagement.dto.request;
using contactManagement.services;
using JetBrains.Annotations;
using Xunit;

namespace contactManagement.Tests.services;

[TestSubject(typeof(ContactImplementation))]
public class ContactImplementationTest
{
    
    private ContactImplementation _contactImplementation;

    public ContactImplementationTest(ContactImplementation contactImplementation)
    {
        _contactImplementation = contactImplementation;
    }

    [Fact]
    public void TestToCheckIfICanCreateContact()
    {
        AddContactRequest request = new AddContactRequest();
        request.FirstName = "John";
        request.LastName = "Smith";
        request.Email = "johnsmith@gmail.com";
        request.PhoneNumber = "0123456789";
        AddContactResponse response = _contactImplementation.CreateContact(request);
        Assert.NotNull(response);
        Assert.Equal(response.Message,"success");
    }
}