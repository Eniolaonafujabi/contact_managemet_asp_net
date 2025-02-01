using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using contactManagement.data.model;
using contactManagement.data.repo;
using contactManagement.dto;
using contactManagement.dto.request;
using contactManagement.services;
using Moq;
using Xunit;

namespace contactManagement.Tests.services;

public class ContactImplementationTest
{
    private readonly ContactImplementation _contactImplementation;
    private readonly Mock<IContactRepo> _mockDatabase;

    public ContactImplementationTest()
    {
        _mockDatabase = new Mock<IContactRepo>();
        _contactImplementation = new ContactImplementation(_mockDatabase.Object);
    }

    [Fact]
    public async Task TestToCheckIfICanCreateContact()
    {
        // Arrange
        var request = new AddContactRequest
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "johnsmith@gmail.com",
            PhoneNumber = "0123456789",
            userId = "1"
        };

        var contact = new Contact
        {
            Id = "123",
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            UserId = request.userId
        };

        _mockDatabase.Setup(r => r.FindIfContactExitForThisUserAsync(request.PhoneNumber, request.userId))
            .ReturnsAsync(false);
        _mockDatabase.Setup(r => r.AddContactAsync(It.IsAny<Contact>()))
            .ReturnsAsync(contact);

        var response = await _contactImplementation.CreateContactAsync(request);
        
        Assert.Equal("Success", response.Message);
        Assert.Equal(contact.Id, response.Id);
    }

    [Fact]
    public async Task TestThatICan_tSaveTwoSameNumberInTheDatabase()
    {
        var request = new AddContactRequest
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "johnsmith@gmail.com",
            PhoneNumber = "0123456789",
            userId = "1"
        };

        var contact = new Contact
        {
            Id = "123",
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            UserId = request.userId
        };

        _mockDatabase.SetupSequence(r => r.FindIfContactExitForThisUserAsync(request.PhoneNumber, request.userId))
            .ReturnsAsync(false)
            .ReturnsAsync(true);

        _mockDatabase.Setup(r => r.AddContactAsync(It.IsAny<Contact>()))
            .ReturnsAsync(contact);
        
        var firstResponse = await _contactImplementation.CreateContactAsync(request);
        Assert.Equal("Success", firstResponse.Message);
        
        await Assert.ThrowsAsync<ArgumentException>(() => _contactImplementation.CreateContactAsync(request));
    }

    [Fact]
    public async Task TestThatICanGetUserContact()
    {
        var request = new AddContactRequest
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "johnsmith@gmail.com",
            PhoneNumber = "0123456789",
            userId = "1"
        };

        var contact = new Contact
        {
            Id = "123",
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            UserId = request.userId
        };

        _mockDatabase.Setup(r => r.FindIfContactExitForThisUserAsync(request.PhoneNumber, request.userId))
            .ReturnsAsync(false);
        _mockDatabase.Setup(r => r.AddContactAsync(It.IsAny<Contact>()))
            .ReturnsAsync(contact);

        var addResponse = await _contactImplementation.CreateContactAsync(request);
        Assert.Equal("Success", addResponse.Message);

        _mockDatabase.Setup(r => r.FindContactByIdAndUserIdAsync(contact.Id, contact.UserId))
            .ReturnsAsync(contact);

        var findRequest = new FindContactRequest
        {
            UserId = contact.UserId,
            Id = contact.Id
        };
        
        var findResponse = await _contactImplementation.GetContactByIdAndUserId(findRequest);
        
        Assert.Equal(contact.Id, findResponse.Id);
        Assert.Equal(contact.FirstName, findResponse.FirstName);
        Assert.Equal(contact.LastName, findResponse.LastName);
        Assert.Equal(contact.Email, findResponse.Email);
        Assert.Equal(contact.PhoneNumber, findResponse.PhoneNumber);
    }

    [Fact]
    public async Task TestICanGetAllContacts()
    {
        var request = new AddContactRequest
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "johnsmith@gmail.com",
            PhoneNumber = "0123456789",
            userId = "1"
        };
        
        var request2 = new AddContactRequest
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "janedoe@gmail.com",
            PhoneNumber = "9876543210", // Different phone number
            userId = "1"
        };

        var contact = new Contact
        {
            Id = "123",
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            UserId = request.userId
        };

        var contact2 = new Contact
        {
            Id = "456",
            FirstName = request2.FirstName,
            LastName = request2.LastName,
            Email = request2.Email,
            PhoneNumber = request2.PhoneNumber,
            UserId = request2.userId
        };


        _mockDatabase.SetupSequence(r => r.FindIfContactExitForThisUserAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(false)
            .ReturnsAsync(false);

        var setupSequence = _mockDatabase.SetupSequence(r => r.AddContactAsync(It.IsAny<Contact>()));
        setupSequence.ReturnsAsync(contact);
        setupSequence.ReturnsAsync(contact2);

        var addResponse = await _contactImplementation.CreateContactAsync(request);
        var addResponse2 = await _contactImplementation.CreateContactAsync(request2);
        Assert.Equal("Success", addResponse.Message);
        Assert.Equal("Success", addResponse2.Message);

        var contactsList = new List<Contact> { contact, contact2 };
        _mockDatabase.Setup(r => r.FindContactByUserIdAsync(contact.UserId))
            .ReturnsAsync(contactsList);

        var findRequest = new FindAllContactRequest
        {
            UserId = contact.UserId,
        };
        
        var findResponse = await _contactImplementation.GetContactsByUserId(findRequest);
        
        Assert.IsType<List<FindContactResponse>>(findResponse);
        Assert.Equal(2, findResponse.Count); // Directly check count
    }

    [Fact]
    public async Task TestThatICanDeleteContactByIdAndUserId() // Changed to async Task
    {
        // Arrange
        var request = new AddContactRequest
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "johnsmith@gmail.com",
            PhoneNumber = "0123456789",
            userId = "1"
        };

        var contact = new Contact
        {
            Id = "123",
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            UserId = request.userId
        };
        
        _mockDatabase.Setup(r => r.FindContactByIdAndUserIdAsync(contact.Id, contact.UserId))
            .ReturnsAsync(contact);

        
        _mockDatabase.Setup(r => r.DeleteByIdAndUserIdAsync(contact.Id, contact.UserId))
            .ReturnsAsync(true);

        var deleteRequest = new FindContactRequest
        {
            UserId = contact.UserId,
            Id = contact.Id
        };
        
        var response = _contactImplementation.DeleteContact(deleteRequest);
        
        Assert.Equal("Success", response.message);
    }
}