using System.Threading.Tasks;

namespace DypaApi.CronJobs
{
    public interface ICronJob
    {
        Task FetchSensorAsync();
        Task WeeklyForecastAsync();
    }
}