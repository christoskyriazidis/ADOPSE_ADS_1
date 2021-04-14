using WpfClientt.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.services.filtering;

namespace WpfClientt.services {
    public interface IAdService : IService<Ad> {

        IScroller<Ad> Fiter(AdsFilterBuilder adsFilterBuilder);

        IScroller<Ad> ProfileAds();
    }
}
