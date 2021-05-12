using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToastNotifications.Core;
using WpfClientt.model;
using WpfClientt.views;

namespace WpfClientt.viewModels {
    public class AdReviewToastViewModel : NotificationBase {

        private NotificationDisplayPart displayPart;
        public override NotificationDisplayPart DisplayPart => displayPart;
        public ICommand ReviewAdCommand { get; set; }
        public ReviewAdNotification ReviewNotification { get; set; }

        public AdReviewToastViewModel(MessageOptions options,ReviewAdNotification reviewNotification): base(string.Empty, options) {
            displayPart = new AdReviewToastView(this);
            ReviewNotification = reviewNotification;
            ReviewAdCommand = new AsyncCommand(ReviewAd);
            Mediator.Subscribe(MediatorToken.ReviewSentToken, ReviewSentListener);
        }

        private async Task ReviewAd() {
            displayPart.OnClose();
            await Mediator.Notify(MediatorToken.ReviewViewToken, ReviewNotification);
        }

        private Task ReviewSentListener(object param) {
            Ad ad = (Ad)param;

            if (ad.Id.Equals(ReviewNotification.Ad.Id)) {
                displayPart.OnClose();
            }

            return Task.CompletedTask;
        }
    }
}
