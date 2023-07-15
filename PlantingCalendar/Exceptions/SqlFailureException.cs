namespace PlantingCalendar.Models
{
    public class SqlFailureException : Exception
    {
        public SqlFailureException(string? message) : base(message)
        {
        }

        public SqlFailureException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}