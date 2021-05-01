using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOne.Helpers {
    public class ChatConnectionHelper2 {
        private ConcurrentDictionary<string, ConcurrentHashSet<string>> connections = new ConcurrentDictionary<string, ConcurrentHashSet<string>>();


        public void Add(string username,string connectionId) {
            connections.TryAdd(username, new ConcurrentHashSet<string>());
            if(connections.TryGetValue(username,out ConcurrentHashSet<string> userConnections)) {
                userConnections.Add(connectionId);
            }
        }

        public void Remove(string username,string connectionId) {
            if(connections.TryGetValue(username,out ConcurrentHashSet<string> userConnections)) {
                userConnections.Remove(connectionId);
            }
        }

        public ConcurrentHashSet<string> ConnectionsOf(string username) {
            if(connections.TryGetValue(username,out ConcurrentHashSet<string> userConnections)) {
                return userConnections;
            }
            return new ConcurrentHashSet<string>();
        }

    }
}
