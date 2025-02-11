﻿using contactManagement.dto;
using contactManagement.dto.request;

namespace contactManagement.services;

public interface IContactInterface
{
    Task<AddContactResponse> CreateContactAsync(AddContactRequest request);
    
    Task<List<FindContactResponse>> GetContactsByUserId(FindAllContactRequest request);
    
    Task<FindContactResponse> GetContactByIdAndUserId(FindContactRequest request);
    
    DeleteContactResponse DeleteContact(FindContactRequest request);
    
}