namespace API.Cache.Redis.Domain.Implementation.Interfaces
{
    public interface IRedisService
    {
        string Get(string key);

        bool Set(string key, string value);

        void Delete();
    }
}