using System.Security.Cryptography;

namespace TestTaskV
{
    public class FileComparer
    {
        public bool AreEqual(string file1, string file2)
        {
            using var hash = MD5.Create();
            using var f1 = File.OpenRead(file1);
            using var f2 = File.OpenRead(file2);

            var h1 = hash.ComputeHash(f1);
            var h2 = hash.ComputeHash(f2);

            return h1.SequenceEqual(h2);
        }
    }
}