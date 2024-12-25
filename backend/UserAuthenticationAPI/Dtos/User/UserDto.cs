namespace UserAuthenticationAPI.Dtos.User
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string PasswordHash { get; set; }
        
    }
}
