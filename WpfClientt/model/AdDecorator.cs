using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.model {
    public class AdDecorator : Ad {

        private Action validation;
        private Action<Category> categoryConsumer;

        public override AdType AdType { 
            get => base.AdType; 
            set {
                base.AdType = value;
                validation.Invoke();
            }
        }
        public override Manufacturer AdManufacturer { 
            get => base.AdManufacturer;
            set { 
                base.AdManufacturer = value;
                validation.Invoke();
            } 
        }
        public override Condition AdCondition { 
            get => base.AdCondition;
            set { 
                base.AdCondition = value;
                validation.Invoke();
            }
        }
        public override Category AdCategory { 
            get => base.AdCategory;
            set { 
                base.AdCategory = value;
                validation.Invoke();
                categoryConsumer.Invoke(value);
            }
        }
        public override Subcategory AdSubcategory { 
            get => base.AdSubcategory;
            set { 
                base.AdSubcategory = value;
                validation.Invoke();
            }
        }
        public override string Title { 
            get => base.Title;
            set { 
                base.Title = value;
                validation.Invoke();
            }
        }
        public override string Description {
            get => base.Description;
            set { 
                base.Description = value;
                validation.Invoke();
            }
        }
        public override int Price { 
            get => base.Price;
            set { 
                base.Price = value;
                validation.Invoke();
            }
        }

        public AdDecorator(Action validation,Action<Category> categoryConsumer) {
            this.validation = validation;
            this.categoryConsumer = categoryConsumer;
        }
    }
}
