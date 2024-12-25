using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver.Core.Misc;

namespace UserAuthenticationAPI.Models
{
    public class User
    {
        [BsonId]    // The [BsonId] attribute tells MongoDB that this is the _id field.
        [BsonRepresentation(BsonType.ObjectId)]  // The[BsonRepresentation(BsonType.ObjectId)] attribute ensures that Id is handled as a string in your code but stored as an ObjectId in the database.
        public string Id { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } 
        public string VerificationCode { get; set; }
        public bool IsRegisterVerified { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime VerificationCodeSentAt { get; set; }
        public DateTime VerificationCodeUsedAt { get; set; }
        public DateTime VerificationCodeExpiresAt { get; set; }
        public DateTime? LastLoginTime { get; set; } 
        public DateTime? LastLogoutTime { get; set; }
        public DateTime? LoginStartTime { get; set; }
        public DateTime? LoginEndTime { get; set; }
        public bool IsOnline { get; set; } 
        public string Role { get; set; } = "User";

    }
}
