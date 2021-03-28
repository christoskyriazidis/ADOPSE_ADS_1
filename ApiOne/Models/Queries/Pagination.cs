using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Queries
{
    public class Pagination
    {
        
        private int _pageNumber = 1;
        public int PageNumber { 
            get { return _pageNumber; }
            set { _pageNumber = (value < 1) ? 1 : value; } 
        }
        
        const int maxPageSize = 50;
        const int minPageSize = 5;
        private int _pageSize = 10;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : (value < 6 ? minPageSize:value); }
        }

        
    }
}
