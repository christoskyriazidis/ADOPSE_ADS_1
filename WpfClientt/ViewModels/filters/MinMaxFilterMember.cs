using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels {
    /// <summary>
    /// Represents a generic filter where user can choose between Min and Max value.
    /// </summary>
    /// <typeparam name="T">The type of the range values.</typeparam>
    public abstract class MinMaxFilterMember<T> : BaseViewModel, FilterMember where T : IComparable<T>  {
        private T min;
        private T max;
        private Action<T, T> onFinish;
        /// <summary>
        /// The minimum value the user can choose inclusively.
        /// </summary>
        public T Min { 
            get {
                return min;
            }
            set {
                if ( value != null && value.CompareTo(Max) <= 0) {
                    min = value;
                    OnPropertyChanged(nameof(Min));
                }    
            } 
        }
        /// <summary>
        /// The maximum vlaue the user can choose inclusively.
        /// </summary>
        public T Max {
            get {
                return max;
            }
            set {
                if (value != null && value.CompareTo(Min) >= 0) {
                    max = value;
                    OnPropertyChanged(nameof(Max));
                }
            }
        }
        /// <summary>
        /// The title of this filter.
        /// </summary>
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
