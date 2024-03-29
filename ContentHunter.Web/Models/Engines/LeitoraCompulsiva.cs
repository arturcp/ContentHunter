﻿using HtmlAgilityPack;
using ContentHunter.Web.Models.Util;
using System.Collections.Generic;

namespace ContentHunter.Web.Models.Engines
{
    [FriendlyNameAttribute("Leitora Compulsiva")]
    public class LeitoraCompulsiva: Crawler
    {
        //Check XmlDocument for documentation on HtmlAgilityPack
        //XPath cheat sheet http://xpath.alephzarro.com/content/cheatsheet.html

        public override ContextResult ParseHtml(Instruction instruction)
        {
            ContextResult context = new ContextResult();
            CrawlerResult output = null;

            string url = instruction.Url;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(GetContent(url));

            HtmlNodeCollection posts = doc.DocumentNode.SelectNodes("//div[@id='content']//div[contains(@class,'post') and contains(@class,'status-publish')]");

            HtmlNodeCollection candidateLinks = doc.DocumentNode.SelectNodes("//div[@id='postnav']//a[contains(.,'Próxima')]");

            if (candidateLinks != null)
            {
                foreach (var link in candidateLinks)
                {
                    AddCandidateLink(context, link.Attributes["href"].Value);
                }
            }
            
            foreach (HtmlNode post in posts)
            {
                output = new CrawlerResult();
                HtmlNode title = post.SelectSingleNode(".//h3[@class='storytitle']");
                HtmlNode content = post.SelectSingleNode(".//div[@class='storycontent']");
                HtmlNodeCollection tags = post.SelectNodes(".//div[@class='meta']//a");

                if (title != null && content != null)
                {
                    HtmlNode postUrl = title.SelectSingleNode(".//a");
                    //if (title.ChildNodes.Count > 0 && title.ChildNodes[0].Attributes["href"] != null)
                        //output.Url = title.ChildNodes[0].Attributes["href"].Value;
                    if (postUrl != null)
                        output.Url = postUrl.Attributes["href"].Value;
                        

                    output.Title = System.Web.HttpUtility.HtmlDecode(title.InnerText);
                    output.Content = content.InnerHtml;
                    //var aux = Sanitize.Strip(output.Content);

                    HtmlNode date = post.SelectSingleNode(".//h3[@class='storytitle']//span[@class='date']");

                    if (date != null)
                    {
                        output.Data = (new LeitoraCompulsivaData() { Date = date.InnerText }).ToJson();
                    }

                    if (tags != null)
                    {
                        List<string> postTags = new List<string>();
                        foreach (var tag in tags)
                        {
                            postTags.Add(tag.InnerText);
                        }
                        output.Tags = string.Join(", ", postTags.ToArray());
                    }

                    context.Results.Add(output);
                }
            }

            return context;
        }

        public override ContextResult ParseRss(Instruction instruction)
        {
            return new ContextResult();
        }

        public override ContextResult ParseXml(Instruction instruction)
        {
            System.Threading.Thread.Sleep(35000);
            return new ContextResult();
        }
    }

    class LeitoraCompulsivaData
    {
        public string Date { get; set; }

        public string ToJson()
        {
            return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(this);
        }
    }
}