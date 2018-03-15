using AppGet.ProgressTracker;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.ProgressTracker
{
    [TestFixture]
    public class ProgressStateFixture
    {
        [Test]
        public void should_return_correct_percent()
        {
            var state = new ProgressState
            {
                Completed = 10,
                Total = 20
            };

            state.ToString().Should().Contain("50");
        }
    }
}