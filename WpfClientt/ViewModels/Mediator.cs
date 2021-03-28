using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels {
    static class Mediator {

        private static Dictionary<string, List<Action<object>>> subscribers = new Dictionary<string, List<Action<object>>>();

        public static void Subscribe(string token, Action<object> action) {
            if (!subscribers.ContainsKey(token)) {
                List<Action<object>> list = new List<Action<object>>();
                list.Add(action);
                subscribers.Add(token, list);
            } else {
                bool found = false;
                foreach(Action<object> callback in subscribers[token]) {
                    if (callback.Method.ToString() == action.Method.ToString()) {
                        found = true;
                    }
                    if (!found) {
                        subscribers[token].Add(action);
                    }
                }
            }

        }

        public static void Notify(string token, object args = null) {
            subscribers[token]?.ForEach(action => action.Invoke(args));
        }

        public static void Unsubscribe(string token,Action<object> action) {
            if (subscribers.ContainsKey(token)) {
                subscribers[token].Remove(action);
            }
        }
    }
}
