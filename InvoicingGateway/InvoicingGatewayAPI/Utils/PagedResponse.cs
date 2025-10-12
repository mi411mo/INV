using Domain.Models;
using Domain.Utils;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingGatewayAPI.Utils
{
    public class PagedResponse<T> : ServiceResult<T>
    {
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        private int PageNumber { get; set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        // Just to test

        public PagedResponse(T data) : base(data)
        {
            Data = data;
        }
        public PagedResponse(T data, int pageNumber, int pageSize) : base(data)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;


        }
        public PagedResponse(T data, PaginationFilter validFilter, string msg = Constants.SUCCESS_Message) : base(data)
        {

            this.PageNumber = validFilter.PageNumber;
            this.PageSize = validFilter.PageSize;
            ResponseCode = Constants.THARWAT_SUCCESS_CODE;
            Message = msg;
        }
        // public static PagedResponse<T> Success(T items, int pageNumber, int pageSize, int totalRecords, int totalPages)
        public static PagedResponse<T> Success(T items, int pageNumber, int pageSize, int totalRecords, int totalPages, string msg = Constants.SUCCESS_Message)
        {
            return new PagedResponse<T>(items)
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                ResponseCode = Constants.THARWAT_SUCCESS_CODE,
                Message = msg
            };
        }

    }
    public static class PaginationHelper
    {


        public static ServiceResult<List<T>> CreatePagedReponse<T>(HttpResponse response, PagedListDto<T> items)
        {
            var metadata = new
            {
                items.TotalRecords,
                items.PageSize,
                items.CurrentPage,
                items.TotalPages,
                items.HasNext,
                items.HasPrevious
            };

            response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return ServiceResult.Success((List<T>)items);
        }
    }
}
