using Microsoft.DotNet.RemoteExecutor;
using System;
using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace ExecutorTest
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ShouldThrow()
        {
            var tracePath = Path.Combine(Path.GetTempPath(), $"remoteexecutor{DateTime.Now:yyyyMMdd}.txt");
            Assert.True(RemoteExecutor.IsSupported);

            var psi = new ProcessStartInfo();
            psi.Environment[$"COREHOST_TRACE"] = "1";
            psi.Environment[$"COREHOST_TRACEFILE"] = tracePath;

            Assert.Throws<RemoteExecutionException>(() =>
                RemoteExecutor.Invoke(() => Assert.True(false),
                 new RemoteInvokeOptions
                 {
                     StartInfo = psi
                 }).Dispose()
            );

            output.WriteLine(File.ReadAllText(tracePath));
        }

        [Fact]
        public void ShouldNotThrow()
        {
            Assert.True(RemoteExecutor.IsSupported);

            RemoteExecutor.Invoke(() => RemoteExecutor.SuccessExitCode).Dispose();
        }
    }
}
