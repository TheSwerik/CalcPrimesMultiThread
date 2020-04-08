using System;
using System.IO;

namespace CalcPrimesMultiThread.Prime.util
{
    public static class CustomConsole
    {
        private static readonly TextWriter StandardOutput;
        private static readonly StringWriter ConsoleOutput;

        static CustomConsole()
        {
            StandardOutput = Console.Out;
            Console.SetOut(ConsoleOutput = new StringWriter());
        }

        public static string Output()
        {
            return ConsoleOutput.GetStringBuilder().ToString();
        }

        public static void Clear()
        {
            ConsoleOutput.GetStringBuilder().Clear();
        }

        public static void WriteLine(string input)
        {
            Console.WriteLine(input);
        }

        public static void ReplaceLine(string input)
        {
            var sb = ConsoleOutput.GetStringBuilder();
            var index = sb.ToString().LastIndexOf(Environment.NewLine, StringComparison.Ordinal) >= 0
                            ? sb.ToString().LastIndexOf(Environment.NewLine, StringComparison.Ordinal)
                            : sb.Length;
            sb.Remove(index, Math.Min(sb.Length - index, Environment.NewLine.Length));

            index = sb.ToString().LastIndexOf(Environment.NewLine, StringComparison.Ordinal) >= 0
                        ? sb.ToString().LastIndexOf(Environment.NewLine, StringComparison.Ordinal) +
                          Environment.NewLine.Length
                        : 0;
            sb.Remove(index, sb.Length - index);
            WriteLine(input);
        }

        public static void NewLine()
        {
            Console.WriteLine();
        }
    }
}