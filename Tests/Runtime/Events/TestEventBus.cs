using System;
using NUnit.Framework;

namespace EasyToolkit.Core.Events.Tests
{
    /// <summary>
    /// Unit tests for event bus dispatch behavior.
    /// </summary>
    [TestFixture]
    public class TestEventBus
    {
        /// <summary>
        /// Verifies that dispatch aggregates handler failures with actionable context.
        /// </summary>
        [Test]
        public void Dispatch_HandlerFailures_ThrowsAggregateExceptionWithContext()
        {
            // Arrange
            var eventBus = new EventBus();
            var completedHandlerCount = 0;

            eventBus.Subscribe<DispatchTestEventArgs>(
                _ => throw new InvalidOperationException("action handler failed"),
                EventPriority.High);
            eventBus.Subscribe(new ThrowingHandler(), EventPriority.Low);
            eventBus.Subscribe<DispatchTestEventArgs>(_ => completedHandlerCount++, EventPriority.Normal);

            // Act
            var exception = Assert.Throws<AggregateException>(() => eventBus.Dispatch(new DispatchTestEventArgs()));

            // Assert
            Assert.That(completedHandlerCount, Is.EqualTo(1));
            Assert.That(exception.InnerExceptions, Has.Count.EqualTo(2));
            Assert.That(exception.Message, Does.Contain(typeof(DispatchTestEventArgs).FullName));
            Assert.That(exception.Message, Does.Contain("2 of 3 handler(s) failed"));
            Assert.That(exception.Message, Does.Contain("1 handler(s) completed successfully"));
            Assert.That(exception.Message, Does.Contain("High"));
            Assert.That(exception.Message, Does.Contain("Low"));
            Assert.That(exception.Message, Does.Contain("action handler failed"));
            Assert.That(exception.Message, Does.Contain("interface handler failed"));
            Assert.That(exception.Message, Does.Contain(typeof(ThrowingHandler).FullName));
        }

        private sealed class DispatchTestEventArgs
        {
        }

        private sealed class ThrowingHandler : IEventHandler<DispatchTestEventArgs>
        {
            public void OnEvent(DispatchTestEventArgs eventArgs)
            {
                throw new InvalidOperationException("interface handler failed");
            }
        }
    }
}
