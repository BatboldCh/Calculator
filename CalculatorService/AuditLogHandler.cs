namespace CalculatorService
{
    public interface IAuditLogService
    {
        Task LogAuditEventAsync(string message);
    }

    public class AuditLogService : IAuditLogService
    {
        public Task LogAuditEventAsync(string message)
        {
            // Implement, writing to a file, database, or external logging service.
            Console.WriteLine($"[Audit Log] {DateTime.UtcNow}: {message}");

            return Task.CompletedTask;
        }
    }

}
