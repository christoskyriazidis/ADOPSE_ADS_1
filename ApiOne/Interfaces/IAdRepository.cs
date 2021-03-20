using ApiOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Interfaces
{
    public interface IAdRepository
    {
        IEnumerable<Ad> GetAds();
        Ad GetAd(int id);
        Ad UpdateAd(Ad ad);
        bool InsertAd(Ad ad);
        bool DeleteAd(Ad ad);


    }
}
