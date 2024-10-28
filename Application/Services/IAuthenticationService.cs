namespace JsonWebTokenAPI.Application.Services
{
    public interface IAuthenticationService
    {
        string ValidateCredentials(string userName, string password);
    }
}
