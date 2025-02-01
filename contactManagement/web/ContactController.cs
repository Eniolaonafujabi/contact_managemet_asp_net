using System.Net;
using contactManagement.dto;
using contactManagement.dto.request;
using contactManagement.dto.response;
using contactManagement.services;
using Microsoft.AspNetCore.Mvc;

namespace contactManagement.web;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactInterface _contactInterface;

    public ContactController(IContactInterface contactInterface)
    {
        _contactInterface = contactInterface;
    }

    [HttpPost("addContact")]
    public async Task<ResponseEntity<AddContactResponse>> AddContact([FromBody] AddContactRequest request)
    {
        try
        {
            AddContactResponse addContactResponse = await _contactInterface.CreateContactAsync(request);
            return new ResponseEntity<AddContactResponse>(addContactResponse, HttpStatusCode.Created);
        }
        catch (Exception e)
        { 
            return new ResponseEntity<AddContactResponse>(e, HttpStatusCode.BadRequest);
        }
    }

    [HttpGet("getAllContact")]
    public async Task<ResponseEntity<List<FindContactResponse>>> getAllContactAsync([FromBody] FindAllContactRequest request)
    {
        try
        {
            List<FindContactResponse> findContactResponses = await _contactInterface.GetContactsByUserId(request);
            return new ResponseEntity<List<FindContactResponse>>(findContactResponses, HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return new ResponseEntity<List<FindContactResponse>>(e, HttpStatusCode.BadRequest);
        }
    }

    [HttpGet("getContactByIdAndUserId")]
    public async Task<ResponseEntity<FindContactResponse>> getContactAsync([FromBody] FindContactRequest request)
    {
        try
        {
            FindContactResponse findContactResponse = await _contactInterface.GetContactByIdAndUserId(request);
            return new ResponseEntity<FindContactResponse>(findContactResponse, HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return new ResponseEntity<FindContactResponse>(e, HttpStatusCode.BadRequest);
        }
    }
    
    [HttpPost("DeleteContact")]
    public  ResponseEntity<DeleteContactResponse> deleteContact([FromBody] FindContactRequest request)
    {
        try
        {
            DeleteContactResponse deleteContactResponse = _contactInterface.DeleteContact(request);
            return new ResponseEntity<DeleteContactResponse>(deleteContactResponse, HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return new ResponseEntity<DeleteContactResponse>(e, HttpStatusCode.BadRequest);
        }
    }

}