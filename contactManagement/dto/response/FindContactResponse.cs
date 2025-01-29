namespace contactManagement.dto;

public class FindContactResponse
{
    public FindContactResponse(string id, string userId, string email, string firstName, string lastName, string phoneNumber)
    {
        Id = id;
        UserId = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }

    public FindContactResponse()
    {
    }

    public string Id { get; set; } = null!;
    
    public string UserId { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
    
    public string PhoneNumber { get; set; } = null!;
}