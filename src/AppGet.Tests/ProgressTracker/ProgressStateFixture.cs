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
                Value = 10,
                MaxValue = 20
            };

            state.GetPercentCompleted().Should().Be(50);
        }
    }
}