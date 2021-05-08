using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels {
    static class Mediator {

        private static Dictionary<MediatorToken, List<Func<object,Task>>> subscribers = new Dictionary<MediatorToken, List<Func<object, Task>>>();

        public static void Subscribe(MediatorToken token, Func<object, Task> action) {
            if (!subscribers.ContainsKey(token)) {
                List<Func<object, Task>> list = new List<Func<object, Task>>();
                list.Add(action);
                subscribers.Add(token, list);
            } else {
                bool found = false;
                foreach(Func<object, Task> callback in subscribers[token]) {
                    if (callback.Method.ToString() == action.Method.ToString()) {
                        found = true;
                    }
                }
                if (!found) {
                    subscribers[token].Add(action);
                }
            }

        }

        public static async Task Notify(MediatorToken token, object args = null) {
            subscribers.TryGetValue(token, out List<Func<object, Task>> listeners);
            if (listeners != null) {
                foreach (Func<object, Task> listener in listeners) {
                    await listener.Invoke(args);
                }
            }
        }

        public static void Unsubscribe(MediatorToken token, Func<object, Task> action) {
            if (subscribers.ContainsKey(token)) {
                subscribers[token].Remove(action);
            }
        }
    }
}
