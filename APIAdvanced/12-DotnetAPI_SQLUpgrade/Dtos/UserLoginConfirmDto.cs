namespace DotnetAPI.Dtos
{
    public class UserLoginConfirmDto
    {
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
    }
}