using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;

namespace WpfClientt.viewModels {
    public class AdReviewNotificationViewModel : NotificationViewModel {

        public ReviewAdNotification ReviewNotification { get; set; }
        public ICommand ReviewCommand { get; set; }

        public AdReviewNotificationViewModel(ReviewAdNotification reviewNotification) {
            ReviewNotification = reviewNotification;
            ReviewCommand = new AsyncCommand(ShowReviewView);
        }

        private async Task ShowReviewView() {
            await Mediator.Notify(MediatorToken.ReviewViewToken, ReviewNotification);
        }

    }
}
