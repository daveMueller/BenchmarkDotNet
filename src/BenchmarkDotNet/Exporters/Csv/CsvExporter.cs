using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Portability;
using BenchmarkDotNet.Reports;
using JetBrains.Annotations;

namespace BenchmarkDotNet.Exporters.Csv
{
    public class CsvExporter : ExporterBase
    {
        private readonly SummaryStyle style;
        private readonly IRuntimeInfoWrapper runtimeInfoWrapper;
        private readonly CsvSeparator separator;
        protected override string FileExtension => "csv";

        public static readonly IExporter Default = new CsvExporter(CsvSeparator.CurrentCulture, SummaryStyle.Default.WithZeroMetricValuesInContent(), new RuntimeInfoWrapper());

        public CsvExporter(CsvSeparator separator) : this (separator, SummaryStyle.Default.WithZeroMetricValuesInContent(), new RuntimeInfoWrapper())
        {
        }

        [PublicAPI] public CsvExporter(CsvSeparator separator, SummaryStyle style, IRuntimeInfoWrapper runtimeInfoWrapper)
        {
            this.style = style;
            this.runtimeInfoWrapper = runtimeInfoWrapper;
            this.separator = separator;
        }

        public override void ExportToLog(Summary summary, ILogger logger)
        {
            string realSeparator = separator.ToRealSeparator();
            foreach (var line in summary.GetTable(style.WithCultureInfo(summary.GetCultureInfo()).WithUnitsInContent(false).WithUnitsInHeader(true), runtimeInfoWrapper).FullContentWithHeader)
            {
                for (int i = 0; i < line.Length;)
                {
                    logger.Write(CsvHelper.Escape(line[i], realSeparator));

                    if (++i < line.Length)
                    {
                        logger.Write(realSeparator);
                    }
                }

                logger.WriteLine();
            }
        }
    }
}