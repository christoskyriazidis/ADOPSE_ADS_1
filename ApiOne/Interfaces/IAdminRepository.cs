using ApiOne.Models.Admin;
using ApiOne.Models.Admin.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Interfaces
{
    public interface IAdminRepository
    {
        bool InsertCategory(InsertCategory insertCategory);
        bool InsertSubCategory(InsertSubCategory insertSubCategory);

        IEnumerable<GetReportsByAd> GetReportsByAd(int AdId);
        IEnumerable<GetReports> GetReports();

    }
}
