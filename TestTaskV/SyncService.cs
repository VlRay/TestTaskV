namespace TestTaskV
{
    public class SyncService
    {
        private readonly string _source;
        private readonly string _replica;
        private readonly int _interval;
        private readonly Logger _logger;
        private readonly FileComparer _comparer;
        private Timer _timer = null!;

        public SyncService(string source, string replica, int interval, Logger logger, FileComparer comparer)
        {
            _source = source;
            _replica = replica;
            _interval = interval;
            _logger = logger;
            _comparer = comparer;
        }

        public void Start()
        {
            if (!Directory.Exists(_source))
            {
                _logger.Log($"Source directory does not exist: {_source}");
                return;
            }

            Directory.CreateDirectory(_replica);
            _timer = new Timer(_ => RunSync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(_interval));
            _logger.Log("Synchronization started.");
        }

        private void RunSync()
        {
            try
            {
                _logger.Log("Syncing...");

                var sourceFiles = Directory.GetFiles(_source, "*", SearchOption.AllDirectories);
                var replicaFiles = Directory.GetFiles(_replica, "*", SearchOption.AllDirectories);

                var sourceRel = new HashSet<string>(sourceFiles.Select(f => Path.GetRelativePath(_source, f)));
                var replicaRel = new HashSet<string>(replicaFiles.Select(f => Path.GetRelativePath(_replica, f)));

                foreach (var rel in sourceRel)
                {
                    var src = Path.Combine(_source, rel);
                    var dst = Path.Combine(_replica, rel);

                    if (!File.Exists(dst) || !_comparer.AreEqual(src, dst))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(dst)!);
                        File.Copy(src, dst, true);
                        _logger.Log($"Copied/Updated: {rel}");
                    }
                }

                foreach (var rel in replicaRel.Except(sourceRel))
                {
                    var dst = Path.Combine(_replica, rel);
                    File.Delete(dst);
                    _logger.Log($"Deleted: {rel}");
                }

                CleanEmptyDirectories();
                _logger.Log("Sync completed.");
            }
            catch (Exception ex)
            {
                _logger.Log($"Error: {ex.Message}");
            }
        }

        private void CleanEmptyDirectories()
        {
            foreach (var dir in Directory.GetDirectories(_replica, "*", SearchOption.AllDirectories).OrderByDescending(d => d.Length))
            {
                if (!Directory.EnumerateFileSystemEntries(dir).Any())
                {
                    Directory.Delete(dir);
                    _logger.Log($"Removed empty dir: {Path.GetRelativePath(_replica, dir)}");
                }
            }
        }
    }
}