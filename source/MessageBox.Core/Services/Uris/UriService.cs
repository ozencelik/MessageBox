using MessageBox.Data.Models.Pagers;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace MessageBox.Core.Services.Uris
{
    public class UriService : IUriService
    {
        #region Fields
        private readonly string _baseUri;
        #endregion

        #region Ctor
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        #endregion

        #region Methods
        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var _enpointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
        #endregion
    }
}
