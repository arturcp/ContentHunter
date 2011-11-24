using System;
using ContentHunter.Web.Models.Util;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebToolkit.Converter;
using System.Web.Script.Serialization;
using System.Linq;
using System.IO;
using WebToolkit.Index;

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

            foreach (var link in candidateLinks)
            {
                AddCandidateLink(context, string.Format("{0}{1}", "http://www.skoob.com.br", link.Attributes["href"].Value));
            }

            if (books != null)
            {
                foreach (HtmlNode book in books)
                {
                    HtmlDocument internalDoc = new HtmlDocument();
                    internalDoc.LoadHtml(GetContent(string.Format("{0}{1}", skoobUrl, book.Attributes["href"].Value)));

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
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();

            var list = (from b in context.Results
                       select oSerializer.Serialize((Book)b.CustomBag)).ToArray<string>();

            Persist(Input.Url.Replace(skoobUrl,string.Empty).Replace(":","-").Replace("/"," ").Trim().Replace(" ","-"), string.Join(",", list));
        }

        private void Persist(string fileName, string json)
        {
            json = "{\"Books\":[" + json + "]}";
            string path = Path.Combine(IndexSettings.IndexPath.Replace("\\Index\\","\\Skoob\\"), fileName + ".json");
            if (File.Exists(path))
            {
                DateTime now = DateTime.Now;
                string bkpPattern = string.Format("{0}-{1}-{2}_{3}-{4}-{5}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                File.Copy(path, path.Replace(".json", "." + bkpPattern + ".json"));
            }

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(json);
            }
        }

        private List<Book> GetBooksFromEdition(string title, string author, string editionsUrl)
        {
            List<Book> books = new List<Book>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(GetContent(editionsUrl));

            HtmlNodeCollection editions = doc.DocumentNode.SelectNodes("//div[preceding-sibling::div[@id='menubusca']]//div[position()=3]//div[@style='float:left; font-size:11px; font-family:arial; margin:10px 8px 0px 0px; width:250px; border:red 0px solid; line-height:18px;']");
            
            Book book = null;
            if (editions != null)
            {
                foreach (HtmlNode edition in editions)
                {
                    HtmlNode img = edition.SelectSingleNode(".//img");
                    string bookCover = string.Empty;
                    if (img != null && img.Attributes["src"].Value != "/img/geral/semcapa_m.gif")
                        bookCover = img.Attributes["src"].Value;

                    string rx = @"(\w+):<\/span>\s([\w\s,]+)<br>";
                    MatchCollection matches = Regex.Matches(edition.InnerHtml, rx, RegexOptions.IgnoreCase);

                    book = new Book();
                    book.Title = title;
                    book.Author = author;
                    book.Cover = bookCover;

                    foreach (Match m in matches)
                    {
                        if (m.Success && m.Groups.Count == 3)
                            SetBookData(book, m.Groups[1].Value, m.Groups[2].Value);
                    }

                    books.Add(book);
                }
            }

            return books;
        }

        private Book SetBookData(Book book, string field, string value)
        {
            switch (field)
            {
                case "Edição": book.Edition = SafeConvert.ToInt(value); break;
                case "Editora": book.Publisher = value; break;
                case "ISBN": book.ISBN = value; break;
                case "Ano": book.Year = SafeConvert.ToInt(value); break;
                case "Páginas": book.Pages = SafeConvert.ToInt(value); break;
                case "Tradutor": book.Translators = value; break;
            }
            return book;
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