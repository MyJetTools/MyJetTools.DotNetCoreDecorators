using System;
using System.Threading.Tasks;

namespace DotNetCoreDecorators
{
    public interface IPublisher<in TValue>
    {
        ValueTask PublishAsync(TValue valueToPublish);
    }
    public interface ISubscriber<out TValue>
    {
        void Subscribe(Func<TValue, ValueTask> callback);
    }
}