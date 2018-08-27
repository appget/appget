using System;
using AppGet.Infrastructure.Eventing;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Infrastructure.Hub
{
    public class EventA : IEvent
    {

    }

    public class EventB : IEvent
    {

    }

    public class HubFixture : TestBase<AppGet.Infrastructure.Eventing.Hub>
    {
        [Test]
        public void should_registerCallback()
        {
            var calledA = 0;
            var calledB = 0;
            Action<EventA> callbackA = e => { calledA++; };
            Action<EventB> callbackB = e => { calledB++; };

            Subject.Subscribe(this, callbackA);
            Subject.Subscribe(this, callbackB);

            Subject.Publish(new EventA());
            Subject.Publish(new EventA());

            calledA.Should().Be(2);
            calledB.Should().Be(0);
        }

        [Test]
        public void should_avoid_duplicate_registration()
        {
            var calledA = 0;
            Action<EventA> callbackA = e => { calledA++; };

            Subject.Subscribe(this, callbackA);
            Subject.Subscribe(this, callbackA);

            Subject.Publish(new EventA());

            calledA.Should().Be(1);
        }

        [Test]
        public void should_unregister_Callback()
        {
            var called = 0;
            Action<EventA> callback = e => { called++; };

            Subject.Subscribe(this, callback);

            Subject.UnSubscribe(this);

            Subject.Publish(new EventA());
            Subject.Publish(new EventA());

            called.Should().Be(0);
        }

    }
}
