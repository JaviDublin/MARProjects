using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Pooling.Models.Abstract;
using System.Text;

namespace Mars.Pooling.Models
{
    public class RedirectModel : IRedirectModel
    {
        private string queryString;
        private readonly string redirectPage;
        private readonly char[] splitChars = { ',', '|' };
        public RedirectModel(string queryString, string redirectPage)
        {
            if (redirectPage == null) throw new ArgumentNullException("redirectPage in the constructor cannot have a null value passed to it");
            this.queryString = queryString;
            this.redirectPage = redirectPage;
        }
        public string RedirectString()
        {
            if (string.IsNullOrEmpty(queryString)) return redirectPage;
            string[] retVal = queryString.Split(splitChars);
            StringBuilder sb = new StringBuilder(redirectPage + "?");
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append("q" + i + "=" + retVal[i] + "&");
            }
            return sb.ToString().Remove(sb.Length - 1);
        }
    }
}