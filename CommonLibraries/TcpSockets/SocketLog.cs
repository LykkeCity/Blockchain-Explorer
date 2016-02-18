using System;
using System.Collections.Generic;
using System.Threading;

namespace TcpSockets
{
    public class SocketLogItem
    {
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
    }

    public interface ISocketLog
    {
        void Add(string message);
    }

    public class SocketLogInMemory : ISocketLog
    {
        private readonly int _maxItems;
        private readonly ReaderWriterLockSlim _readerWriterLockSlim = new ReaderWriterLockSlim();

        public SocketLogInMemory(int maxItems = 100)
        {
            _maxItems = maxItems;
        }

        private readonly List<SocketLogItem> _items = new List<SocketLogItem>();

        public void Add(string message)
        {


            _readerWriterLockSlim.EnterWriteLock();
            try
            {
                _items.Add(new SocketLogItem {DateTime = DateTime.UtcNow, Message = message});

                while (_items.Count > _maxItems)
                    _items.RemoveAt(0);

                Count++;

            }
            finally
            {
                _readerWriterLockSlim.ExitWriteLock();
            }

        }

        public IEnumerable<SocketLogItem> GetItems()
        {
            _readerWriterLockSlim.EnterReadLock();
            try
            {
                return _items.ToArray();
            }
            finally
            {
                _readerWriterLockSlim.ExitReadLock();
            }
                
        }

        public int Count { get; private set; }

    }

    public class SocketLogDynamic : ISocketLog
    {
        private readonly Action<int> _changeCount;
        private readonly Action<string> _newMessage;

        public SocketLogDynamic(Action<int> changeCount, Action<string> newMessage)
        {
            _changeCount = changeCount;
            _newMessage = newMessage;
        }

        public void Add(string message)
        {
            _newMessage(message);
        }

    }
}
