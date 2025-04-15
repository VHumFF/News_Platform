namespace News_Platform.Services.Interfaces
{
    public interface IForgotPasswordService
    {
        Task<bool> SendResetPasswordEmailAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
    }
}
