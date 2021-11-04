using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCoreDecorators
{
    public class TaskTimer
    {
        private readonly TimeSpan _delay;

        private readonly Dictionary<string, Func<ValueTask>> _items = new Dictionary<string, Func<ValueTask>>();

        public TaskTimer(TimeSpan delay)
        {
            _delay = delay;
        }

        public TaskTimer Register(string name, Func<ValueTask> callback)
        {
            _items.Add(name, callback);
            return this;
        }

        private Func<string, Exception, ValueTask> _exceptionCallback;

        public TaskTimer RegisterExceptionHandler(Func<string, Exception, ValueTask> exceptionCallback)
        {
            _exceptionCallback = exceptionCallback;
            return this;
        }

        private bool _working;
        private Task _task;

        private void HandleException(string serviceName, Exception e)
        {
            Console.WriteLine("Exception executing timer: " + serviceName);
            Console.WriteLine(e);

            _exceptionCallback?.Invoke(serviceName, e);
        }
        
        

        private async Task ExecuteAsync(string serviceName,  Func<ValueTask> callback)
        {
            try
            {
                await callback();
            }
            catch (Exception e)
            {
                HandleException(serviceName, e);
            }

        }

        private async Task LoopAsync()
        {
            var tasks = new List<Task>();

            while (_working)
            {
                try
                {
                    foreach (var item in _items)
                        tasks.Add(ExecuteAsync(item.Key, item.Value));

                    foreach (var task in tasks)
                        await task;
                }
                finally
                {
                    tasks.Clear();
                    await Task.Delay(_delay);
                }
            }
        }

        public void Start()
        {
            _working = true;
            _task = Task.Run(LoopAsync);
        }
        
        public void Stop()
        {
            _working = false;
            _task.Wait();
        }

    }
}