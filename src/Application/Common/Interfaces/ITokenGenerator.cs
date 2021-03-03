namespace Application.Common.Interfaces
{
    public interface ITokenGenerator
    {
        string CreateToken(string userId);
    }
}