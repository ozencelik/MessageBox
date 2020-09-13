using MessageBox.Core.Services.Uris;
using MessageBox.Data.Models.Pagers;
using System;
using System.Collections.Generic;

namespace MessageBox.Api.Configuration
{
    public class PaginationHelper
    {
        public static PagedResponse<IList<T>> CreatePagedReponse<T>(IList<T> pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
        {
            var response = new PagedResponse<IList<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            response.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
                : null;
            response.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
                : null;
            response.FirstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route);
            response.LastPage = uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.PageSize), route);
            response.TotalPages = roundedTotalPages;
            response.TotalRecords = totalRecords;
            return response;
        }
    }
}
