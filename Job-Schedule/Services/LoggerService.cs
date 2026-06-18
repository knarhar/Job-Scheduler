namespace Job_Scheduler.Services
{
    internal class LoggerService
    {
        public void Handle(object? sender, JobEventArgs e)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {e.EventName} (job #{e.Job.Id} '{e.Job.Name}')");
            if (e.Error != null)
            {
                Console.WriteLine($"    error: {e.Error.Message}");
            }
        }
    }
}