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
        private const int MaxSize = 500_000_000; // 500MB
        private static int _fileNumber;
        private static FileStream _stream;
        private static StreamWriter _writer;

        private static string FullFileName => FileName + _fileNumber + FileExtension;

        public static BigInteger FindLastPrime()
        {
            while (File.Exists(FullFileName)) _fileNumber++;
            _fileNumber--;
            var result = BigInteger.Parse(File.ReadLines(FullFileName).Last());
            _stream ??= new FileStream(FullFileName, FileMode.Append);
            _writer ??= new StreamWriter(_stream);
            return result;
        }

        public static void WriteFile<T>(IEnumerable<T> a)
        {
            _stream ??= new FileStream(FullFileName, FileMode.Append);
            _writer ??= new StreamWriter(_stream);
            foreach (var i in a)
            {
                _writer.WriteLine(i);
                if (_stream.Length < MaxSize) continue; // if less than 500MB
                _writer.Dispose();
                _stream.Dispose();
                _fileNumber++;
                _stream = new FileStream(FullFileName, FileMode.Append);
                _writer = new StreamWriter(_stream);
            }
        }

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

        public static void Dispose()
        {
            _writer?.Dispose();
            _stream?.Dispose();
        }
    }
}