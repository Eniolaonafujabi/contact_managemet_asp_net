using System;
using System.Threading.Tasks;
using contactManagement.data.model;
using contactManagement.data.repo;
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

        // Act
        var response = await _contactImplementation.CreateContactAsync(request);

        // Assert
        Assert.Equal("Success", response.Message);
        Assert.Equal(contact.Id, response.Id);
    }

    [Fact]
    public async Task TestThatICan_tSaveTwoSameNumberInTheDatabase()
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

        _mockDatabase.SetupSequence(r => r.FindIfContactExitForThisUserAsync(request.PhoneNumber, request.userId))
            .ReturnsAsync(false)
            .ReturnsAsync(true);

        _mockDatabase.Setup(r => r.AddContactAsync(It.IsAny<Contact>()))
            .ReturnsAsync(contact);

        // Act & Assert - First call should succeed
        var firstResponse = await _contactImplementation.CreateContactAsync(request);
        Assert.Equal("Success", firstResponse.Message);

        // Second call should throw
        await Assert.ThrowsAsync<ArgumentException>(() => _contactImplementation.CreateContactAsync(request));
    }

    [Fact]
    public async Task TestThatICanGetUserContact()
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

        // Create the contact
        var addResponse = await _contactImplementation.CreateContactAsync(request);
        Assert.Equal("Success", addResponse.Message);

        // Setup find method
        _mockDatabase.Setup(r => r.FindContactByIdAndUserIdAsync(contact.Id, contact.UserId))
            .ReturnsAsync(contact);

        var findRequest = new FindContactRequest
        {
            UserId = contact.UserId,
            Id = contact.Id
        };

        // Act
        var findResponse = await _contactImplementation.GetContactByIdAndUserId(findRequest);

        // Assert
        // Assert.NotNull(findResponse);
        Assert.Equal(contact.Id, findResponse.Id);
        // Assert.Equal(contact.FirstName, findResponse.FirstName);
        // Assert.Equal(contact.LastName, findResponse.LastName);
        // Assert.Equal(contact.Email, findResponse.Email);
        // Assert.Equal(contact.PhoneNumber, findResponse.PhoneNumber);
    }
}