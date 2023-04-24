using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPFBankDepartmentMVVM.Services.Implementations
{
    class MessageBusService : IMessageBus
    {
        private class Subscription<T> : IDisposable
        {
            private readonly WeakReference<MessageBusService> _Bus;
            public Action<T> Handler { get; }

            public Subscription(MessageBusService Bus, Action<T> handler)
            {
                _Bus = new(Bus);
                Handler = handler;
            }

            public void Dispose()
            {
                if (!_Bus.TryGetTarget(out var bus)) return;
                var @lock = bus._Lock;
                var message_type = typeof(T);
                @lock.EnterWriteLock();
                try
                {
                    if (!bus._Subscriptions.TryGetValue(message_type, out var refs)) return;

                    var updated_refs = refs.Where(r => r.IsAlive).ToList();
                    WeakReference? current_ref = null;
                    foreach (var @ref in updated_refs) 
                    {
                        if (ReferenceEquals(@ref.Target, this))
                        {
                            current_ref = @ref;
                            break;
                        }
                    }

                    if (current_ref is null) return;

                    updated_refs.Remove(current_ref);
                    bus._Subscriptions[message_type] = updated_refs;
                }
                finally
                {
                    @lock.ExitWriteLock();
                }
            }
        }

        private readonly Dictionary<Type, IEnumerable<WeakReference>> _Subscriptions = new();
        private readonly ReaderWriterLockSlim _Lock = new();

        public IDisposable RegesterHandler<T>(Action<T> handler)
        {
            var subscription = new Subscription<T>(this, handler);
            _Lock.EnterWriteLock();
            try
            {                
                var subscriptions_ref = new WeakReference(subscription);
                var message_type = typeof(T);
                _Subscriptions[message_type] = _Subscriptions.TryGetValue(message_type, out var subscriptions)
                    ? subscriptions.Append(subscriptions_ref)
                    : new[] { subscriptions_ref };
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
            
            return subscription;
        }


        private IEnumerable<Action<T>>? GetHandlers<T>()
        {
            var handlers = new List<Action<T>>();
            var message_type = typeof(T);
            var exist_die_refs = false;
            _Lock.EnterReadLock();
            try
            {
                if (!_Subscriptions.TryGetValue(message_type, out var refs)) return null;

                foreach(var @ref  in refs)
                {
                    if (@ref.Target is Subscription<T> { Handler: var handler }) handlers.Add(handler);
                    else exist_die_refs = true;
                }
            }
            finally
            {
                _Lock.ExitReadLock();
            }

            if (!exist_die_refs) return handlers;
            _Lock.EnterWriteLock();

            try
            {
                if (_Subscriptions.TryGetValue(message_type, out var refs))
                    if(refs.Where(r => r.IsAlive).ToArray() is { Length: > 0 } new_refs)
                        _Subscriptions[message_type] = new_refs; 
                    else _Subscriptions.Remove(message_type);
            }
            finally
            {
                _Lock.ExitWriteLock();
            }

            return handlers;
        }
        public void Send<T>(T message)
        {
            if (GetHandlers<T>() is not { } handlers) return;

            foreach(var handler in handlers)
            {
                handler(message);
            }
        }
    }
}
