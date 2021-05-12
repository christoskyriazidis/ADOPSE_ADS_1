using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class ReviewViewModel : FormViewModel<Review> {

        private ICustomerService customerService;
        public ISet<int> Ratings { get; set; } = new HashSet<int>();
        public override Review Form { get ; protected set ; }

        public Ad Ad { get; set; }

        public Customer AdOwner { get; set; }

        private ReviewViewModel(ICustomerService customerService, ICustomerNotifier notifier,Ad ad,Customer adOwner) 
            : base(notifier,"The review has been posted successfully.") {
            this.customerService = customerService;
            Ad = ad;
            AdOwner = adOwner;
            for(int i = 1; i <=10; i++) {
                Ratings.Add(i);
            }
            Form = new Review() {
                SoldAd = ad.Id
            };
        }

        public static ReviewViewModel GetInstance(FactoryServices factory,Ad ad,Customer adOwner) {
            ICustomerService customerService = factory.CustomerServiceInstance();
            ICustomerNotifier notifier = factory.CustomerNotifier();

            return new ReviewViewModel(customerService, notifier, ad,adOwner);
        }


        protected override Func<Review, Task> SubmitAction() {
            return async (Review review) => {
                await customerService.AddReview(review);
                await Mediator.Notify(MediatorToken.ReviewSentToken, Ad);
                await Mediator.Notify(MediatorToken.NotificationsViewToken);
            };
        }

        protected override void ClearFormStrep() {
            Form = new Review();
        }
    }
}
