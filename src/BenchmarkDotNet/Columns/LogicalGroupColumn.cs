using BenchmarkDotNet.Portability;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using JetBrains.Annotations;

namespace BenchmarkDotNet.Columns
{
    public class LogicalGroupColumn : IColumn
    {
        [PublicAPI] public static readonly IColumn Default = new LogicalGroupColumn();

        public string Id => nameof(LogicalGroupColumn);
        public string ColumnName => "LogicalGroup";

        public string GetValue(Summary summary, BenchmarkCase benchmarkCase, IRuntimeInfoWrapper runtimeInfoWrapper) => summary.GetLogicalGroupKey(benchmarkCase);
        public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style, IRuntimeInfoWrapper runtimeInfoWrapper) => GetValue(summary, benchmarkCase, runtimeInfoWrapper);
        public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
        public bool IsAvailable(Summary summary) => true;

        public bool AlwaysShow => true;
        public ColumnCategory Category => ColumnCategory.Meta;
        public int PriorityInCategory => 1;
        public bool IsNumeric => false;
        public UnitType UnitType => UnitType.Dimensionless;
        public string Legend => "";
    }
}