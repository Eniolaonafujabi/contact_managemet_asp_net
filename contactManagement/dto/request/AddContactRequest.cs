﻿namespace contactManagement.dto.request;

public class AddContactRequest
{
    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
    
    public string PhoneNumber { get; set; } = null!;
}