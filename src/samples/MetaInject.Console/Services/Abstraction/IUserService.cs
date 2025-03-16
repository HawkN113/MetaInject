namespace MetaInject.Console.Services.Abstraction;

public interface IUserService
{
    Task<string> GetCurrentUserAsync();
}