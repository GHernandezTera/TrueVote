using Hangfire;
using Hangfire.Common;

namespace TrueVote.Jobs
{
    public class Scheduler
    {
        public static void Init() {
            var manager = new RecurringJobManager();

            BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

            manager.AddOrUpdate("counter", Job.FromExpression(() => Console.WriteLine("It's been another minute!")), Cron.Minutely());
        }
    }
}
