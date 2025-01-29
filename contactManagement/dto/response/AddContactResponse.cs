namespace contactManagement.dto;

public class AddContactResponse
{
    private string message;

    public string Message
    {
        get => message;
        set => message = value ?? throw new ArgumentNullException(nameof(value));
    }
}