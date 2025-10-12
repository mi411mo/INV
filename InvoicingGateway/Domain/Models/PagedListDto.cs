using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PagedListDto<T> : List<T>
    {
        public int TotalPages { get; private set; }
        public long TotalRecords { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public PagedListDto(List<T> items, long totalRecords, int pageNumber, int pageSize)
        {
            TotalRecords = totalRecords;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            AddRange(items);
        }
        public static PagedListDto<T> ToPagedList(IEnumerable<T> items, int totalRecords, int pageNumber, int pageSize)
        {
            return new PagedListDto<T>(items.ToList(), (long)totalRecords, pageNumber, pageSize);
        }
        public static PagedListDto<T> ToPagedList(IEnumerable<T> items, long totalRecords, int pageNumber, int pageSize)
        {
            return new PagedListDto<T>(items.ToList(), totalRecords, pageNumber, pageSize);
        }
    }
}
