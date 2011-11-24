using System;
using ContentHunter.Web.Models.Util;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace ContentHunter.Web.Models.Engines
{
    [FriendlyNameAttribute("Skoob")]
    public class Skoob : Crawler
    {
        protected override bool IsCustomSaved
        {
            get { return true; }
        }

        private string skoobUrl = "http://www.skoob.com.br";

        public override ContextResult ParseHtml(Instruction instruction)
        {
            ContextResult context = new ContextResult();
            CrawlerResult output = null;

            string url = instruction.Url;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(GetContent(url));

            HtmlNodeCollection books = doc.DocumentNode.SelectNodes("//div[@id='resultadoBusca']//div[@class='box_capa_lista_busca']//a[position()=1]");

            HtmlNodeCollection candidateLinks = doc.DocumentNode.SelectNodes("//div[@id='resultadoBusca']//a[@class='l13' and position()=1]");

           /* foreach (var link in candidateLinks)
            {
                AddCandidateLink(context, string.Format("{0}{1}", "http://www.skoob.com.br", link.Attributes["href"].Value));
            }*/

            foreach (HtmlNode book in books)
            {
                HtmlDocument internalDoc = new HtmlDocument();
                internalDoc.LoadHtml(GetContent(string.Format("{0}{1}", skoobUrl, book.Attributes["href"].Value)));

                /*HtmlNode img = internalDoc.DocumentNode.SelectSingleNode("//div[@id='livro_capa']//img");
                string bookCover = string.Empty;
                if (img != null && img.Attributes["src"].Value != "/img/geral/semcapa_m.gif")
                    bookCover = img.Attributes["src"].Value;*/

                HtmlNode title = internalDoc.DocumentNode.SelectSingleNode("//div[@id='barra_titulo']//h1");
                string bookTitle = string.Empty;
                if (title != null)
                    bookTitle = title.InnerText;

                HtmlNode author = internalDoc.DocumentNode.SelectSingleNode("//div[@id='barra_autor']//a");
                string bookAuthor = string.Empty;
                if (author != null)
                    bookAuthor = author.InnerText;

                HtmlNode editions = internalDoc.DocumentNode.SelectSingleNode("//div[@id='menubusca']//a[@title='Edições']");
                string bookEditionsUrl = string.Empty;
                if (editions != null)
                {
                    bookEditionsUrl = string.Format("{0}{1}", skoobUrl, editions.Attributes["href"].Value);
                    List<Book> bookEditions = GetBooksFromEdition(bookTitle, bookAuthor, bookEditionsUrl);
                    foreach (Book bookEdition in bookEditions)
                    {
                        context.Results.Add(new CrawlerResult() { CustomBag = bookEdition });
                    }
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

        public override void CustomSave(ContextResult context)
        {
            Book book = null;
            foreach (CrawlerResult item in context.Results)
            {
                book = (Book)item.CustomBag;
            }
        }

        private List<Book> GetBooksFromEdition(string title, string author, string editionsUrl)
        {
            List<Book> books = new List<Book>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(GetContent(editionsUrl));

            HtmlNodeCollection editions = doc.DocumentNode.SelectNodes("//div[preceding-sibling::div[@id='menubusca']]//div[position()=3]");                

            return books;
        }
    }

    class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Cover { get; set; }
        public int Edition { get; set; }
        public string Publisher { get; set; }
        public string ISBN { get; set; }
        public int Year { get; set; }
        public int Pages { get; set; }
        public string Translators { get; set; }

    }
}