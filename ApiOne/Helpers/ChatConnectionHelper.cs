using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Helpers
{
    public class ChatConnectionHelper<T>
    {
        private readonly Dictionary<string, HashSet<string>> _connections =
           new Dictionary<string, HashSet<string>>();


        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public Dictionary<string, HashSet<string>> GetKeyValuePairs()
        {
            return _connections;
        }

        public  string ToString(string username)
        {
            string s = "takhs:";
            foreach(var i in _connections[username])
            {
                s += $" {i} ";
            }
            return s;
        }

        public void Add(string key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(string key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public void Remove(string key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}
