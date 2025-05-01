namespace TestTaskV
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: FolderSync <sourceDir> <replicaDir> <intervalInSeconds> <logFilePath>");
                return;
            }

            string sourceDir = args[0];
            string replicaDir = args[1];
            int interval = int.TryParse(args[2], out var seconds) ? seconds : 60;
            string logPath = args[3];

            var logger = new Logger(logPath);
            var comparer = new FileComparer();
            var syncService = new SyncService(sourceDir, replicaDir, interval, logger, comparer);

            syncService.Start();

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}