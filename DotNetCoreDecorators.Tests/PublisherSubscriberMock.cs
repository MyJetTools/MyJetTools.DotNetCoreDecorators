using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCoreDecorators.Tests
{
    public class PublisherSubscriberMock<T> : IPublisher<T>, ISubscriber<T>
    {
        public async ValueTask PublishAsync(T valueToPublish)
        {
            foreach (var callback in _callbacks)
                await callback(valueToPublish);
        }

        
        private readonly List<Func<T, ValueTask>> _callbacks = new List<Func<T, ValueTask>>();
        public void Subscribe(Func<T, ValueTask> callback)
        {
            _callbacks.Add(callback);
        }
    }
}