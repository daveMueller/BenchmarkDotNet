using System.Linq;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Tests.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace BenchmarkDotNet.Tests.Reports
{
    public class SummaryTableTests
    {
        private readonly ITestOutputHelper output;

        public SummaryTableTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private SummaryTable CreateTable()
        {
            var logger = new AccumulationLogger();
            var config = DefaultConfig.Instance;
            var runtimeInfoWrapperMock = MockFactory.CreateRuntimeInfoWrapper();
            var summary = MockFactory.CreateSummary(config, runtimeInfoWrapperMock);
            var table = new SummaryTable(summary, runtimeInfoWrapperMock);
            MarkdownExporter.Default.ExportToLog(summary, logger);
            output.WriteLine(logger.GetLog());
            return table;
        }

        private SummaryTable.SummaryTableColumn CreateColumn(string header)
        {
            var column = CreateTable().Columns.FirstOrDefault(c => c.Header == header);
            Assert.NotNull(column);
            return column;
        }

        [Fact]
        public void PlatformTest()
        {
            var gcModeColumn = CreateColumn("Platform");
            Assert.True(gcModeColumn.IsDefault);
        }

        [Fact]
        public void NumericColumnIsRightJustified()
        {
            var config = ManualConfig.Create(DefaultConfig.Instance).AddColumn(StatisticColumn.Mean);
            var runtimeInfoWrapperMock = MockFactory.CreateRuntimeInfoWrapper();
            var summary = MockFactory.CreateSummary(config, runtimeInfoWrapperMock);
            var table = new SummaryTable(summary, runtimeInfoWrapperMock);

            Assert.Equal(SummaryTable.SummaryTableColumn.TextJustification.Right, table.Columns.First(c => c.Header == "Mean").Justify);
        }

        [Fact]
        public void TextColumnIsLeftJustified()
        {
            var config = ManualConfig.Create(DefaultConfig.Instance).AddColumn(new ParamColumn("Param"));
            var runtimeInfoWrapperMock = MockFactory.CreateRuntimeInfoWrapper();
            var summary = MockFactory.CreateSummary(config, runtimeInfoWrapperMock);
            var table = new SummaryTable(summary, runtimeInfoWrapperMock);

            Assert.Equal(SummaryTable.SummaryTableColumn.TextJustification.Left, table.Columns.First(c => c.Header == "Param").Justify);
        }

        [Fact] // Issue #1070
        public void CustomOrdererIsSupported()
        {
            var config = ManualConfig.Create(DefaultConfig.Instance)
                .WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical));
            var summary = MockFactory.CreateSummary(config, MockFactory.CreateRuntimeInfoWrapper());
            Assert.True(summary.Orderer is DefaultOrderer defaultOrderer &&
                        defaultOrderer.SummaryOrderPolicy == SummaryOrderPolicy.FastestToSlowest &&
                        defaultOrderer.MethodOrderPolicy == MethodOrderPolicy.Alphabetical);
        }

        [Fact] // Issue #1168
        public void ZeroValueInMetricColumnIsDashedByDefault()
        {
            // arrange
            var config = ManualConfig.Create(DefaultConfig.Instance);
            var metrics = new[] { new Metric(new FakeMetricDescriptor("metric1", "some legend", "0.0"), 0.0) };

            // act
            var summary = MockFactory.CreateSummary(config, hugeSd: false, metrics);
            var table = new SummaryTable(summary, MockFactory.CreateRuntimeInfoWrapper());
            var actual = table.Columns.First(c => c.Header == "metric1").Content;

            // assert
            Assert.True(actual.All(value => "-" == value));
        }

        [Fact] // Issue #1168
        public void ZeroValueInMetricColumnIsNotDashedWithCustomStyle()
        {
            // arrange
            var config = ManualConfig.Create(DefaultConfig.Instance);
            var metrics = new[] { new Metric(new FakeMetricDescriptor("metric1", "some legend", "0.0"), 0.0) };
            var style = config.SummaryStyle.WithZeroMetricValuesInContent();

            // act
            var summary = MockFactory.CreateSummary(config, hugeSd: false, metrics);
            var table = new SummaryTable(summary, MockFactory.CreateRuntimeInfoWrapper(), style);
            var actual = table.Columns.First(c => c.Header == "metric1").Content;

            // assert
            Assert.True(actual.All(value => "0.0" == value));
        }
    }
}