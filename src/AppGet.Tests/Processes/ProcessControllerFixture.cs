using AppGet.Windows;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Processes
{
    [TestFixture]
    public class ProcessControllerFixture : TestBase<ProcessController>
    {
        [Test]
        public void get_process_by_id()
        {
            var process = Subject.TryGetRunningProcess(999);
            process.Should().BeNull();
        }
    }
}