using HtmlAgilityPack;
using ContentHunter.Web.Models.Util;

namespace ContentHunter.Web.Models.Engines
{
    [FriendlyNameAttribute("Leitora Compulsiva")]
    public class LeitoraCompulsiva: Crawler
    {
        //Check XmlDocument for documentation on HtmlAgilityPack

        public override CrawlerResult ParseHtml()
        {
            string result = string.Empty;
            string url = this.Input.Url;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(GetContent(url));


            HtmlNodeCollection posts = doc.DocumentNode.SelectNodes("//div[@id='content']//div[@class]");

            foreach (HtmlNode post in posts)
            {
                HtmlNode title = post.SelectSingleNode(".//h3[@class='storytitle']");
                //result += HttpUtility.HtmlDecode(node.InnerText) + "<br />";
                if (title != null)
                    result += title.InnerText + "<br />";

                HtmlNode content = post.SelectSingleNode(".//div[@class='storycontent']");

                if (content != null)
                    result += content.InnerHtml + "<br /><hr>";
                    //result += content.InnerText + "<br /><hr>";
            }

            return new CrawlerResult()
            {
                Content = result,
                Title = string.Empty
            };
        }

        public override CrawlerResult ParseRss()
        {
            return new CrawlerResult();
        }

        public override CrawlerResult ParseXml()
        {
            return new CrawlerResult();
        }

    }
}