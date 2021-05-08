using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;

namespace WpfClientt.viewModels {
    public class AdViewModel {
        public ICommand ReadMoreCommand { get; private set; }

        public Ad Ad { get; set; }

        public AdViewModel(Ad ad) {
            ReadMoreCommand = new AsyncCommand<Ad>(OnReadMore);
            this.Ad = ad;
        }
        private async Task OnReadMore(Ad ad) {
            await Mediator.Notify(MediatorToken.AdDetailsViewToken, ad);
        }
    }
}
