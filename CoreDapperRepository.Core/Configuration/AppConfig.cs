
namespace CoreDapperRepository.Core.Configuration
{
    public class AppConfig
    {
        /// <summary>
        /// Gets or sets Redis connection string. Used when Redis caching is enabled
        /// </summary>
        public string RedisCachingConnectionString { get; set; }
    }
}
