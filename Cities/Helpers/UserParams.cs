using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        private int pageSize = 10; //initial value

        public int PageNumber { get; set; } = 1;//initial value and it's 1-based index
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        //public int UserId { get; set; }
        public string OrderBy { get; set; }
    }
}
