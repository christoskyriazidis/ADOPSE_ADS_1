using DypaApi.Models.PostRequest;
using DypaApi.Models.Worker;
using DypaApi.Models.Xorafi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Interfaces
{
    public interface IXorafi
    {
        Xorafi GetXorafi(int Xid);
        bool AddXorafi(Xorafi xorafi);
        bool AddSensorToXorafi(SensorToXorafi sensorToXorafi);
        bool EditXorafi(Xorafi xorafi);
        bool RemoveXorafi(int xid);


        bool AddSubCategory(SubCategory subCategory);
        bool AddCategory(Category category);
        bool SetWatering(int XorafiId,bool state);
        bool WaterXorafi(int XorafiId);

        List<Xorafi> GetXorafia();

        IEnumerable<Xorafi> GetXorafiaByOwnerId(int OwnderId);
        IEnumerable<SubCategory> GetSubCategories();
        IEnumerable<Category> GetCategories();

    }
}
