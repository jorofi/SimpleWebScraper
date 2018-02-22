using System.Threading.Tasks;
using DownloadAirInfo.Models.Download;

namespace DownloadAirInfo.Services
{
    public interface IDownloadService
    {
        Task DownloadAsync(Configuration configuration);
    }
}
