﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace ContentHunter.Web.Models
{
    public class LeitoraCompulsiva: Crawler, IHunter
    {
        //Check XmlDocument for documentation on HtmlAgilityPack
        #region IHunter Members

        public string Parse(string url)
        {
            string result = string.Empty;

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



            /*HtmlNodeCollection titles = doc.DocumentNode.SelectNodes("//h3[@class='storytitle']");

            foreach(HtmlNode node in titles)
            {
                //result += HttpUtility.HtmlDecode(node.InnerText) + "<br />";
                result += node.InnerText + "<br />";
            }*/

            return result;
        }

        #endregion
    }
}