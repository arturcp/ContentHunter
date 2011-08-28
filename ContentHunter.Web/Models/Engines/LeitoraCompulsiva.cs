using HtmlAgilityPack;
using ContentHunter.Web.Models.Util;
using System.Collections.Generic;

namespace ContentHunter.Web.Models.Engines
{
    [FriendlyNameAttribute("Leitora Compulsiva")]
    public class LeitoraCompulsiva: Crawler
    {
        //Check XmlDocument for documentation on HtmlAgilityPack
        //XPath cheat sheet http://xpath.alephzarro.com/content/cheatsheet.html

        public override List<CrawlerResult> ParseHtml()
        {
            List<CrawlerResult> list = new List<CrawlerResult>();
            CrawlerResult output = null;

            string url = this.Input.Url;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(GetContent(url));

            //HtmlNodeCollection posts = doc.DocumentNode.SelectNodes("//div[@id='content']//div[@class='post']");
            HtmlNodeCollection posts = doc.DocumentNode.SelectNodes("//div[@id='content']//div[contains(@class,'post') and contains(@class,'status-publish')]");

            foreach (HtmlNode post in posts)
            {
                output = new CrawlerResult();
                HtmlNode title = post.SelectSingleNode(".//h3[@class='storytitle']");
                HtmlNode content = post.SelectSingleNode(".//div[@class='storycontent']");
                HtmlNodeCollection tags = post.SelectNodes(".//div[@id='meta']//a");

                if (title != null && content != null)
                {
                    if (title.ChildNodes.Count > 0 && title.ChildNodes[0].Attributes["href"] != null)
                        output.Url = title.ChildNodes[0].Attributes["href"].Value;

                    output.Title = System.Web.HttpUtility.HtmlDecode(title.InnerText);
                    output.Content = content.InnerHtml;

                    if (tags != null)
                    {
                        List<string> postTags = new List<string>();
                        foreach (var tag in tags)
                        {
                            postTags.Add(tag.InnerText);
                        }
                        output.Tags = string.Join(", ", postTags.ToArray());
                    }

                    list.Add(output);

                }
            }

            return list;
        }

        public override List<CrawlerResult> ParseRss()
        {
            return new List<CrawlerResult>();
        }

        public override List<CrawlerResult> ParseXml()
        {
            return new List<CrawlerResult>();
        }

    }
}