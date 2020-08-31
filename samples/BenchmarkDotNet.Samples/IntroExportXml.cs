using System.Threading;
using BenchmarkDotNet.Attributes;

namespace BenchmarkDotNet.Samples
{
    [CsvExporter]
    public class IntroExportXml
    {
        [Benchmark] public void Sleep10() => Thread.Sleep(10);
        [Benchmark] public void Sleep20() => Thread.Sleep(20);
    }
}
