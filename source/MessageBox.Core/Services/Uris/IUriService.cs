using MessageBox.Data.Models.Pagers;
using System;

namespace MessageBox.Core.Services.Uris
{
    public interface IUriService
    {
        /// <summary>
        /// Get page uri
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
