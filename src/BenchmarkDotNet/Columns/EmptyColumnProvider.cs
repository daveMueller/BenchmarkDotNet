using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Portability;
using BenchmarkDotNet.Reports;

namespace BenchmarkDotNet.Columns
{
    public class EmptyColumnProvider : IColumnProvider
    {
        public static readonly IColumnProvider Instance = new EmptyColumnProvider();

        private EmptyColumnProvider()
        {
        }

        public IEnumerable<IColumn> GetColumns(Summary summary, IRuntimeInfoWrapper runtimeInfoWrapper) => Enumerable.Empty<IColumn>();
    }
}