using WpfClientt.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.services;
using System.Windows.Input;
using System.Diagnostics;

namespace WpfClientt.ViewModels {
    public class AdScrollerViewModel {

        public ObservableCollection<Ad> ads { get; } = new ObservableCollection<Ad>();
        private IScroller<Ad> scroller;
        public ICommand NextPageCommand { get; private set; }
        public ICommand PreviousPageCommand { get; private set; }

        public AdScrollerViewModel() {
            HttpClient client = new HttpClient();
            scroller = new AdServiceImpl(client).Scroller();
            scroller.Init((scroll) => {
                foreach (Ad ad in scroll.CurrentPage().Objects()) {
                    ads.Add(ad);
                }
            });
            NextPageCommand = new DelegateCommand(OnMoveNext);

            PreviousPageCommand = new DelegateCommand(OnMoveBack);
        }

        private async void OnMoveNext(object param) {
            if (await scroller.MoveNext()) {
                ads.Clear();
                foreach (Ad ad in scroller.CurrentPage().Objects()) {
                    ads.Add(ad);
                }
            }
        }

        private async void OnMoveBack(object param) {
            if (await scroller.MoveBack()) {
                ads.Clear();
                foreach (Ad ad in scroller.CurrentPage().Objects()) {
                    ads.Add(ad);
                }
            }
        }
     

    }
}
