using DypaApi.Models.Users;
using DypaApi.Models.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Interfaces
{
    public interface IWorker
    {
        IEnumerable<Worker> GetWorkers();
        int GetCustomerIdFromSub(string SubId);

        Owner GetMyProfile(int Uid);
    }
}
