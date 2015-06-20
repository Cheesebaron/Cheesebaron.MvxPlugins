using System.Threading.Tasks;

namespace Cheesebaron.MvxPlugins.AppId
{
    public interface IAppIdGeneratorEx : IAppIdGenerator
    {
        Task<string> GetOsVersionAsync();
        Task<string> GetPhoneModelAsync();
    }
}