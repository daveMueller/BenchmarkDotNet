﻿using System.Linq;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Portability;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Tests.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace BenchmarkDotNet.Tests.Reports
{
    public class ColumnTests
    {
        private readonly ITestOutputHelper output;

        public ColumnTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void UniqueIdTest()
        {
            var config = ManualConfig.Create(DefaultConfig.Instance)
                .AddColumn(StatisticColumn.Mean)
                .AddColumn(StatisticColumn.Mean)
                .AddColumn(StatisticColumn.StdDev)
                .AddColumn(StatisticColumn.Mean)
                .AddColumn(StatisticColumn.Mean)
                .AddColumn(StatisticColumn.P67);

            var runtimeInfoWrapperMock = MockFactory.CreateRuntimeInfoWrapper();
            var summary = CreateSummary(config, runtimeInfoWrapperMock);
            var columns = summary.GetColumns(runtimeInfoWrapperMock);
            Assert.Equal(1, columns.Count(c => c.Id == StatisticColumn.Mean.Id));
            Assert.Equal(1, columns.Count(c => c.Id == StatisticColumn.StdDev.Id));
            Assert.Equal(1, columns.Count(c => c.Id == StatisticColumn.P67.Id));
        }

        private Summary CreateSummary(IConfig config, IRuntimeInfoWrapper runtimeInfoWrapper)
        {
            var logger = new AccumulationLogger();
            var summary = MockFactory.CreateSummary(config, runtimeInfoWrapper);
            MarkdownExporter.Default.ExportToLog(summary, logger);
            output.WriteLine(logger.GetLog());
            return summary;
        }
    }
}