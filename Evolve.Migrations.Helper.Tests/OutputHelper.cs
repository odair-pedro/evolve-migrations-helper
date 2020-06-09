using System;
using System.IO;
using System.Text;

namespace Evolve.Migrations.Helper.Tests
{
    public static class OutputHelper
    {
        public static string ExecuteAndReadOutputText(Action action)
        {
            using var stream = new MemoryStream();
            
            var writer = new StreamWriter(stream);
            Console.SetOut(writer);
            action();
            Console.Out.Flush();

            var buffer = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer);
        }
    }
}