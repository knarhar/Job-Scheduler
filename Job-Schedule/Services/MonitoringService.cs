namespace Job_Scheduler.Services
{
    internal class MonitoringService
    {
        public void Handle(object? sender, JobEventArgs e)
        {
            Console.WriteLine($"{e.EventName} : {e.Job.Name} [{e.Job.Status}]");
        }
    }
}