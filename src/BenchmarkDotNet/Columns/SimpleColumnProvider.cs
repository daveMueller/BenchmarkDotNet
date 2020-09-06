using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Portability;
using BenchmarkDotNet.Reports;

namespace BenchmarkDotNet.Columns
{
    public class SimpleColumnProvider : IColumnProvider
    {
        private readonly IColumn[] columns;

        public SimpleColumnProvider(params IColumn[] columns)
        {
            this.columns = columns;
        }

        public IEnumerable<IColumn> GetColumns(Summary summary, IRuntimeInfoWrapper runtimeInfoWrapper) => columns.Where(c => c.IsAvailable(summary));
    }
}