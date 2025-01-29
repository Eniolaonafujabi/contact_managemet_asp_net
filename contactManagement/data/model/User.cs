using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace contactManagement.data.model;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] // Maps the ID to MongoDB's ObjectId type
    public string Id { get; set; } = null!;

    [BsonElement("Email")]
    public string Email { get; set; } = null!;

    [BsonElement("FirstName")]
    public string FirstName { get; set; } = null!;

    [BsonElement("LastName")]
    public string LastName { get; set; } = null!;
}