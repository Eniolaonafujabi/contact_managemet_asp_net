using System;
using System.Threading.Tasks;
using contactManagement.data.model;
using contactManagement.data.repo;
using contactManagement.dto;
using contactManagement.dto.request;
using contactManagement.services;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace contactManagement.Tests.services;

[TestSubject(typeof(ContactImplementation))]
public class ContactImplementationTest
{
    
    private ContactImplementation _contactImplementation;
    
    private readonly Mock<IContactRepo> _mockDatabase;

    public ContactImplementationTest()
    {
        _mockDatabase = new Mock<IContactRepo>();
        _contactImplementation = new ContactImplementation(_mockDatabase.Object);
    }

    [Fact]
    public void TestToCheckIfICanCreateContact()
    {
        Task<AddContactResponse> response = AddContactResponse();
        string expected = "Success";
        Assert.Equal(response.Result.Message,expected);
    }
    
    [Fact]
    public async Task TestThatICan_tSaveTwoSameNumberInTheDatabase()
    {
        _mockDatabase.SetupSequence(r => r.FindIfContactExitForThisUserAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()
            ))
            .ReturnsAsync(false)
            .ReturnsAsync(true);
        var addContactResponse = AddContactResponse();
        string expected = "Success";
        Assert.Equal(addContactResponse.Result.Message,expected);
        await Assert.ThrowsAsync<ArgumentException>(() => AddContactResponse());
    }

    [Fact]
    public async Task TestThatICanGetUserContact()
    {  
        var addContactResponse = AddContactResponse();
        string expected = "Success";
        Assert.Equal(addContactResponse.Result.Message,expected);
        FindContactRequest request = new FindContactRequest();
        request.UserId = "1";
        request.id = addContactResponse.Result.Id;
        _mockDatabase.Setup(r => r.FindContactByIdAndUserIdAsync(
            addContactResponse.Result.Id, 
            addContactResponse.Result.UserId
        )).ReturnsAsync(addContactResponse);
        FindContactResponse findContactResponse = await _contactImplementation.GetContactByIdAndUserId(request);
        Assert.NotNull(findContactResponse);
    }

    private async Task<AddContactResponse> AddContactResponse()
    {
        AddContactRequest request = new AddContactRequest();
        request.FirstName = "John";
        request.LastName = "Smith";
        request.Email = "johnsmith@gmail.com";
        request.PhoneNumber = "0123456789";
        request.userId = "1";
        AddContactResponse response = await _contactImplementation.CreateContactAsync(request);
        return response;
    }
    
}