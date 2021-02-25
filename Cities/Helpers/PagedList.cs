using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; } // 1-based index
        public int Totalpages { get; set; }  // how many pages altogether
        public int PageSize { get; set; }    // lines per page
        public int TotalCount { get; set; }  // how many lines altogether

        // All items, TotalCount, CurrentPage, PageSize
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            Totalpages = (int)(count * 1.0 / pageSize + 0.5);
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            // ToListAsync() is deferred to execute
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
