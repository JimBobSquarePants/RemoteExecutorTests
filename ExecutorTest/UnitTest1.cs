using Microsoft.DotNet.RemoteExecutor;
using System;
using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace ExecutorTest
{
    public class UnitTest1 : XunitContextBase
    {
        public UnitTest1(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public void CaptureInfo()
        {
            var tracePath = Path.Combine(Path.GetTempPath(), $"remoteexecutor{DateTime.Now:yyyyMMdd}.txt");
            Assert.True(RemoteExecutor.IsSupported);

            var psi = new ProcessStartInfo();
            psi.Environment["COREHOST_TRACE"] = "1";
            psi.Environment["COREHOST_TRACEFILE"] = tracePath;

            RemoteExecutor.Invoke(() => Assert.True(true),
             new RemoteInvokeOptions
             {
                 StartInfo = psi
             }).Dispose();

            this.WriteLine(File.ReadAllText(tracePath));
        }

        [Fact]
        public void ShouldThrow()
        {
            Assert.True(RemoteExecutor.IsSupported);

            Assert.Throws<RemoteExecutionException>(() =>
                RemoteExecutor.Invoke(() => Assert.True(false)).Dispose()
            );
        }

        [Fact]
        public void ShouldNotThrow()
        {
            Assert.True(RemoteExecutor.IsSupported);

            RemoteExecutor.Invoke(() => RemoteExecutor.SuccessExitCode).Dispose();
        }
    }
}
