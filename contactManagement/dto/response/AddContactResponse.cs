namespace contactManagement.dto;

public class AddContactResponse
{
    private string message;

    private string id;
    
    private string userId;

    private string email;

    public string Id
    {
        get => id;
        set => id = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string UserId
    {
        get => userId;
        set => userId = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Email
    {
        get => email;
        set => email = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string FirstName
    {
        get => firstName;
        set => firstName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string LastName
    {
        get => lastName;
        set => lastName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string PhoneNumber
    {
        get => phoneNumber;
        set => phoneNumber = value ?? throw new ArgumentNullException(nameof(value));
    }

    private string firstName;

    private string lastName;
    
    private string phoneNumber;

    public string Message
    {
        get => message;
        set => message = value ?? throw new ArgumentNullException(nameof(value));
    }
    
}