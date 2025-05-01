namespace TestTaskV
{
    public class Logger
    {
        private readonly string _logFile;

        public Logger(string logFile)
        {
            _logFile = logFile;
        }

        public void Log(string message)
        {
            string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            Console.WriteLine(line);

            try
            {
                File.AppendAllLines(_logFile, new[] { line });
            }
            catch
            {
                Console.WriteLine("Logging failed.");
            }
        }
    }
}