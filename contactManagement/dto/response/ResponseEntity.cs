using System.Net;

namespace contactManagement.dto.response;

public class ResponseEntity<T>
{
    public HttpStatusCode Status { get; set; }
    public T? Body { get; set; }
    public Exception? ErrorMessage { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();

    public ResponseEntity(T body, HttpStatusCode status)
    {
        Body = body;
        Status = status;
    }

    public ResponseEntity(Exception errorMessage, HttpStatusCode status)
    {
        ErrorMessage = errorMessage;
        Status = status;
    }
}
 