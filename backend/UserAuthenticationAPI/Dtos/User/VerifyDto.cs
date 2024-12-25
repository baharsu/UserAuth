namespace UserAuthenticationAPI.Dtos.User
{
    public class VerifyDto
    {
        public string verificationCode { get; set; }
        public string username { get; set; }
        public string loginStartTime { get; set; }
    }
}
