using Funda.Enums;
using Microsoft.AspNetCore.WebUtilities;
using System.Web;

namespace Funda.Helpers
{
    public class FundaApiUrlBuilder
    {
        private UriBuilder _urlBuilder;

        public FundaApiUrlBuilder(Uri baseAddress)
        {
            _urlBuilder = new UriBuilder(baseAddress);
        }

        public string AbsoluteUrl
        {
            get
            {
                return _urlBuilder.Uri.AbsoluteUri;
            }
        }

        public void SetType(FundaObjectType type)
        {
            switch(type)
            {
                case FundaObjectType.Buy:
                    SetParameter("type", "koop");
                    return;
                case FundaObjectType.Rent:
                    SetParameter("type", "huur");
                    return;
            }
        }

        public void SetSearch(List<string> values)
        {
            string search = "";

            foreach(string value in values)
            {
                search += $"/{value}";
            }

            SetParameter("zo", search);
        }

        public void SetPageNr(int pageNr)
        {
            SetParameter("page", pageNr.ToString());
        }

        public void SetPageSize(int pageSize)
        {
            SetParameter("pagesize", pageSize.ToString());
        }

        private void SetParameter(string name, string value)
        {
            var queryString = HttpUtility.ParseQueryString(_urlBuilder.Query);

            queryString.Set(name, value);

            _urlBuilder.Query = queryString.ToString();
        }
    }
}
