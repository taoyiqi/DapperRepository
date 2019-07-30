
namespace CoreDapperRepository.Core.Configuration
{
    public interface IDbConnConfig
    {
        string GetConnectionString(string connKey);
    }
}
