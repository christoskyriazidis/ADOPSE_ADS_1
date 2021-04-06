using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels.filters {
    public abstract class MinMaxFilterMember<T> : BaseViewModel, FilterMember where T : IComparable<T>  {
        private T min;
        private T max;
        private Action<T, T> onFinish;
        public T Min { 
            get {
                return min;
            }
            set {
                if ( value != null && value.CompareTo(Max) <= 0) {
                    min = value;
                    OnPropertyChanged("Min");
                }    
            } 
        }
        public T Max {
            get {
                return max;
            }
            set {
                if (value != null &&   value.CompareTo(Min) >= 0) {
                    max = value;
                    OnPropertyChanged("Max");
                }
            }
        }
        public string Title { get; private set; }


        public MinMaxFilterMember(string title,Action<T,T> onFinish) {
            this.Title = title;
            this.onFinish = onFinish;
        }

        public void Finish() {
            onFinish.Invoke(Min, Max);
        }

        public void Reset() {
            Min = default;
            Max = default;
        }
    }
}
