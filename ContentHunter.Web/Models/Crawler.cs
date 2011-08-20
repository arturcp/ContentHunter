using System.Net;
using HtmlAgilityPack;
using System.IO;

namespace ContentHunter.Web.Models
{
    public class Crawler
    {
        public int HttpCode { get; set; }

        public Crawler()
        {
            HttpCode = 200;
        }
        
        public string GetContent(string url)
        {
            string content = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                HttpCode = 200;
                using (WebClient crawler = new WebClient())
                {
                    try
                    {
                        content = new StreamReader(crawler.OpenRead(url)).ReadToEnd();
                    }
                    catch (WebException error)
                    {
                        HttpWebResponse response = (HttpWebResponse)error.Response;
                        HttpCode = (int)response.StatusCode;
                    }
                }
            }
            else HttpCode = 404;
            return content;
        }
    }
}