using Hangfire;
using Hangfire.Common;
using TrueVote.Entities;
using TrueVote.Utilities;

namespace TrueVote.Jobs
{
    public class Scheduler
    {
        public void Init()
        {
            var manager = new RecurringJobManager();

            BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

            manager.AddOrUpdate("counter", Job.FromExpression(() => ConsoleUtilities.Sucess("It's been another minute!")), Cron.Minutely());
        }
    }
}
