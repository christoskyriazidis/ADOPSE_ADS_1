using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiOne.Helpers {
    public class ConcurrentHashSet<T> {

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private readonly HashSet<T> _hashSet = new HashSet<T>();

        public bool Add(T item) {
            _lock.EnterWriteLock();
            try {
                return _hashSet.Add(item);
            } finally {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }

        public void Clear() {
            _lock.EnterWriteLock();
            try {
                _hashSet.Clear();
            } finally {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }

        public bool Contains(T item) {
            _lock.EnterReadLock();
            try {
                return _hashSet.Contains(item);
            } finally {
                if (_lock.IsReadLockHeld) _lock.ExitReadLock();
            }
        }

        public bool Remove(T item) {
            _lock.EnterWriteLock();
            try {
                return _hashSet.Remove(item);
            } finally {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }

        public async Task ForEachDoInParallel(Action<T> action) {
            _lock.EnterReadLock();
            try {
                foreach(T t in _hashSet) {
                    action.Invoke(t);
                }
            } finally {
                if (_lock.IsReadLockHeld) _lock.ExitReadLock();
            }
        }

        public int Count {
            get {
                _lock.EnterReadLock();
                try {
                    return _hashSet.Count;
                } finally {
                    if (_lock.IsReadLockHeld) _lock.ExitReadLock();
                }
            }
        }


    }

}

