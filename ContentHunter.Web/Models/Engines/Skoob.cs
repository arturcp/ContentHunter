using System;
using ContentHunter.Web.Models.Util;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace ContentHunter.Web.Models.Engines
{
    [FriendlyNameAttribute("Skoob")]
    public class Skoob : Crawler
    {
        public override ContextResult ParseHtml(Instruction instruction)
        {
            ContextResult context = new ContextResult();
            CrawlerResult output = null;

            string url = instruction.Url;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(GetContent(url));

            HtmlNodeCollection books = doc.DocumentNode.SelectNodes("//div[@id='resultadoBusca']//div[@class='box_capa_lista_busca']//a");

            HtmlNodeCollection candidateLinks = doc.DocumentNode.SelectNodes("//div[@id='resultadoBusca']//a[@class='l13']");

            foreach (var link in candidateLinks)
            {
                AddCandidateLink(context, string.Format("{0}{1}", "http://www.skoob.com.br", link.Attributes["href"].Value));
            }

            foreach (HtmlNode post in books)
            {
                output = new CrawlerResult();
                HtmlNode title = post.SelectSingleNode(".//h3[@class='storytitle']");
                HtmlNode content = post.SelectSingleNode(".//div[@class='storycontent']");
                HtmlNodeCollection tags = post.SelectNodes(".//div[@class='meta']//a");

                if (title != null && content != null)
                {
                    if (title.ChildNodes.Count > 0 && title.ChildNodes[0].Attributes["href"] != null)
                        output.Url = title.ChildNodes[0].Attributes["href"].Value;

                    output.Title = System.Web.HttpUtility.HtmlDecode(title.InnerText);
                    output.Content = content.InnerHtml;
                    //var aux = Sanitize.Strip(output.Content);

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
            throw new NotImplementedException();
        }

        public override ContextResult ParseXml(Instruction instruction)
        {
            throw new NotImplementedException();
        }
    }
}