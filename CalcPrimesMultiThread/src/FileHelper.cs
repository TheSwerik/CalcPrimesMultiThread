using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace CalcPrimesMultiThread
{
    public static class FileHelper
    {
        private const string FileName = "Primes";
        private const string FileExtension = ".txt";
        private static int _fileNumber = 0;

        public static BigInteger FindLastPrime()
        {
            while (File.Exists(FullFileName)) _fileNumber++;
            _fileNumber--;
            return BigInteger.Parse(File.ReadLines(FullFileName).Last());
        }

        public static void WriteFile<T>(IEnumerable<T> a)
        {
            var stream = new FileStream(FullFileName, FileMode.Append);
            var writer = new StreamWriter(stream);
            foreach (var i in a)
            {
                writer.WriteLine(i);
                if (stream.Length / 1_000_000 < 500) continue; // if less than 500MB
                writer.DisposeAsync();
                stream.DisposeAsync();
                _fileNumber++;
                stream = new FileStream(FullFileName, FileMode.Append);
                writer = new StreamWriter(stream);
            }
            Console.WriteLine(stream.Length);

            writer.Dispose();
            stream.Dispose();
        }

        private static string FullFileName => FileName + _fileNumber + FileExtension;

        public static void Restart()
        {
            _fileNumber = 0;
            while (File.Exists(FullFileName))
            {
                File.Delete(FullFileName);
                _fileNumber++;
            }

            _fileNumber = 0;
            File.Create(FullFileName).Dispose();
        }
    }
}