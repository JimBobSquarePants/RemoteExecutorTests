using Microsoft.DotNet.RemoteExecutor;
using Xunit;

namespace ExecutorTest
{
    public class UnitTest1
    {
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
