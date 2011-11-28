using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebToolkit.Converter;
using ContentHunter.Web.Models;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.IO;
using WebToolkit.Index;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;
using System.Threading;

namespace ContentHunter.Web.Controllers
{
    public class SkoobController : Controller
    {
        private string baseUrl = "http://www.skoob.com.br/{0}/{1}";

        public ActionResult Index()
        {
            Start(1, 40000);
            Start(40001, 80000);
            Start(80001, 120000);
            Start(120001, 160000);
            Start(160001, 196597);
            return View();
        }

        public ActionResult Show()
        {
            //ParseAuthors(new Parameters() { StartId = 746, EndId = 746 });
            ParseAuthors(new Parameters() { StartId = 11, EndId = 11 });
            return View("Index");
        }

        private void Start(int startId, int endId)
        {
            Thread t = new Thread(new ParameterizedThreadStart(ParseBooks));
            t.Start(new Parameters() { StartId = startId, EndId = endId });
        }

        private void StartAuthors(int startId, int endId)
        {
            Thread t = new Thread(new ParameterizedThreadStart(ParseAuthors));
            t.Start(new Parameters() { StartId = startId, EndId = endId });
        }

        #region Books
        public void ParseBooks(object info)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            Parameters parameters = (Parameters)info;
            List<Book> books = null;
            string url = string.Empty;
            int httpCode = 0;
            for (int id = parameters.StartId; id <= parameters.EndId; id++)
            {
                books = new List<Book>();
                url = string.Format(baseUrl, "livro/edicoes", id);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(GetContent(url, ref httpCode));

                if (httpCode == 200)
                {
                    Book book = null;

                    HtmlNode title = doc.DocumentNode.SelectSingleNode("//div[preceding-sibling::div[@id='menubusca']]//div[position()=1]//h1");
                    string bookTitle = string.Empty;
                    if (title != null)
                        bookTitle = title.InnerText;

                    HtmlNode subtitle = doc.DocumentNode.SelectSingleNode("//div[preceding-sibling::div[@id='menubusca']]//div[position()=1]//h2");
                    string bookSubTitle = string.Empty;
                    if (subtitle != null)
                        bookSubTitle = subtitle.InnerText;

                    HtmlNode author = doc.DocumentNode.SelectSingleNode("//div[preceding-sibling::div[@id='menubusca']]//div[position()=1]//a[@class='l11']");
                    string bookAuthor = string.Empty;
                    if (author != null)
                        bookAuthor = author.InnerText;

                    HtmlNodeCollection editions = doc.DocumentNode.SelectNodes("//div[preceding-sibling::div[@id='menubusca']]//div[position()=3]//div[@style='float:left; font-size:11px; font-family:arial; margin:10px 8px 0px 0px; width:250px; border:red 0px solid; line-height:18px;']");
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
                            book.Id = id;
                            book.Title = bookTitle;
                            book.Author = bookAuthor;
                            book.SubTitle = bookSubTitle;
                            book.Cover = bookCover;

                            foreach (Match m in matches)
                            {
                                if (m.Success && m.Groups.Count == 3)
                                    SetBookData(book, m.Groups[1].Value, m.Groups[2].Value);
                            }

                            books.Add(book);
                        }
                    }

                    CustomSave("Books", oSerializer.Serialize(books), parameters.StartId, parameters.EndId, id);
                }
            }
        }

        private void CustomSave(string area, string json, int startId, int endId, int id)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            string uri = string.Format(@"\Skoob\{2}\{0}-{1}\", startId, endId, area);
            string path = IndexSettings.IndexPath.Replace("\\Index\\", uri);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string filePath = Path.Combine(path, string.Concat(id.ToString(), ".json"));

            if (System.IO.File.Exists(filePath))
            {
                DateTime now = DateTime.Now;
                string bkpPattern = string.Format("{0}-{1}-{2}_{3}-{4}-{5}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                System.IO.File.Copy(filePath, filePath.Replace(".json", "." + bkpPattern + ".json"));
            }

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(json);
            }
        }

        private Book SetBookData(Book book, string field, string value)
        {
            switch (field)
            {
                case "Edição": book.Edition = SafeConvert.ToInt(value); break;
                case "Editora": book.Publisher = value; break;
                case "ISBN":
                    ISBN isbn = new ISBN(value);
                    book.ISBN10 = isbn.ISBN10;
                    book.ISBN13 = isbn.ISBN13; 
                    break;
                case "Ano": book.Year = SafeConvert.ToInt(value); break;
                case "Páginas": book.Pages = SafeConvert.ToInt(value); break;
                case "Tradutor": book.Translators = value; break;
            }
            return book;
        }
        #endregion

        #region Authors
        public void ParseAuthors(object info)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            Parameters parameters = (Parameters)info;
            string url = string.Empty;
            int httpCode = 0;
            for (int id = parameters.StartId; id <= parameters.EndId; id++)
            {
                url = string.Format(baseUrl, "autor", id);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(GetContent(url, ref httpCode));

                if (httpCode == 200)
                {
                    Author author = new Author();
                    author.Id = id;

                    HtmlNode image = doc.DocumentNode.SelectSingleNode("//div[preceding-sibling::div[@id='breadCrumb']]//div[position()=1]//img[position()=1]");
                    author.Image = string.Empty;
                    if (image != null)
                        author.Image = image.Attributes["src"].Value;

                    HtmlNode name = doc.DocumentNode.SelectSingleNode("//div[preceding-sibling::div[@id='breadCrumb']]//div[position()=1]//h1");
                    author.Name = string.Empty;
                    if (name != null)
                        author.Name = name.InnerText;

                    HtmlNode description = doc.DocumentNode.SelectSingleNode("//div[preceding-sibling::div[@id='breadCrumb']]//div[position()=1]//div[@id='biografia']");
                    author.Description = string.Empty;
                    if (description != null)
                        author.Description = CleanData(description.InnerText).Substring(10);

                    HtmlNode data = doc.DocumentNode.SelectSingleNode("//div[preceding-sibling::div[@id='breadCrumb']]//div[position()=1]//div[@style='border:#000 0px solid; float:right; width:175px; color:#666666;  font-family:arial; font-size:11px; line-height:20px;']");

                    string rx = @"<strong>([\w:]+)<\/strong><br>([\w\s,-:\/]+)";
                    MatchCollection matches = Regex.Matches(data.InnerHtml, rx, RegexOptions.IgnoreCase);

                    foreach (Match m in matches)
                    {
                        if (m.Success && m.Groups.Count == 3)
                            SetAuthorData(author, m.Groups[1].Value, m.Groups[2].Value);
                    }

                    rx = "<a href=\"([.\\/\\w:]+)\" class=\"l11out\" target=\"_blank\">([\\w\\/\\s.:!]+)<\\/a>";
                    matches = Regex.Matches(data.InnerHtml, rx, RegexOptions.IgnoreCase);

                    foreach (Match m in matches)
                    {
                        if (m.Success && m.Groups.Count == 3)
                            author.Links.Add(m.Groups[1].Value);
                    }

                    HtmlNode bookCount = doc.DocumentNode.SelectSingleNode("//div[preceding-sibling::div[@id='breadCrumb']]//div[position()=3]//h1[@style='font-size:17px; font-family: trebuchet ms; color:#666666; float:left;']");
                    int count = SafeConvert.ToInt(bookCount.InnerText.Replace("Livros do Autor (","").Replace(")",""));

                    if (count <= 15)
                    {
                        HtmlNodeCollection books = doc.DocumentNode.SelectNodes("//div[preceding-sibling::div[@id='breadCrumb']]//div[position()=3]//div[@style='border:green 0px solid; width:430px; margin: 10px  0px;']//a");

                        if (books != null)
                        {
                            foreach (HtmlNode link in books)
                            {
                                author.Books.Add(link.Attributes["href"].Value.Replace("/livro/", "").Split('-')[0]);
                            }
                        }
                    }
                    else
                    {
                        HtmlDocument booksDoc = new HtmlDocument();
                        booksDoc.LoadHtml(GetContent(string.Format(baseUrl, "autor/livros", id), ref httpCode));

                        if (httpCode == 200)
                        {
                            HtmlNodeCollection books = booksDoc.DocumentNode.SelectNodes("//div[preceding-sibling::div[@id='menubusca']]//div[position()=3]//a");
                            if (books != null)
                            {
                                foreach (HtmlNode link in books)
                                {
                                    author.Books.Add(link.Attributes["href"].Value.Replace("/livro/", "").Split('-')[0]);
                                }
                            }
                        }
                    }


                    CustomSave("Authors", oSerializer.Serialize(author), parameters.StartId, parameters.EndId, id);
                }
            }
        }

        public static string CleanData(string inputString)
        {
            StringBuilder sb = new StringBuilder();

            string[] parts = inputString.Split(new char[] { ' ', '\n', '\t', '\r', '\f', '\v' }, StringSplitOptions.RemoveEmptyEntries);

            int size = parts.Length;
            for (int i = 0; i < size; i++)
                sb.AppendFormat("{0} ", parts[i]);

            return sb.ToString();
        }

        private Author SetAuthorData(Author author, string field, string value)
        {
            field = field.Replace(":","");
            switch (field)
            {
                case "Gêneros": author.Categories = CleanData(value); break;
                case "Nascimento": author.BirthDate = CleanData(value); break;
                case "Local": author.Local = CleanData(value); break;
            }
            return author;
        }
        #endregion

        public string GetContent(string url, ref int httpCode)
        {
            string content = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                httpCode = 200;
                using (WebClient crawler = new WebClient())
                {
                    try
                    {
                        content = new StreamReader(crawler.OpenRead(url), Encoding.Default).ReadToEnd();
                    }
                    catch (WebException error)
                    {
                        HttpWebResponse response = (HttpWebResponse)error.Response;
                        httpCode = (int)response.StatusCode;
                    }
                }
            }
            else httpCode = 404;
            return content;
        }

    }

    class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Author { get; set; }
        public string Cover { get; set; }
        public int Edition { get; set; }
        public string Publisher { get; set; }
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }
        public int Year { get; set; }
        public int Pages { get; set; }
        public string Translators { get; set; }

    }

    class Author
    {
        public Author()
        {
            Links = new List<string>();
            Books = new List<string>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Categories { get; set; }
        public string BirthDate { get; set; }
        public string Local { get; set; }
        public List<string> Links { get; set; }
        public List<string> Books { get; set; }
        public string Image { get; set; }
    }

    class Parameters
    {
        public int StartId { get; set; }
        public int EndId { get; set; }
    }

    class ISBN
    {
        public ISBN(string isbn)
        {
            Value = isbn.Trim();
        }

        public string Value { get; set; }
        public string ISBN10 { 
            get{
                if (Value.Length == 13)
                    return ConvertTo10(Value);
                else return Value;
            } 
        }

        public string ISBN13 { 
            get{
                if (Value.Length == 10)
                    return ConvertTo13(Value);
                else return Value;
            }
        }

        public static string ConvertTo10(string isbn13)
        {
            string isbn10 = string.Empty;
            long temp;

            if (!string.IsNullOrEmpty(isbn13) &&
                isbn13.Length == 13 &&
                Int64.TryParse(isbn13, out temp))
            {
                isbn10 = isbn13.Substring(3, 9);
                int sum = 0;
                for (int i = 0; i < 9; i++)
                    sum += Int32.Parse(isbn10[i].ToString()) * (i + 1);

                int result = sum % 11;
                char checkDigit = (result > 9) ? 'X' : result.ToString()[0];
                isbn10 += checkDigit;
            }

            return isbn10;
        }

        public static string ConvertTo13(string isbn10)
        {
            string isbn13 = string.Empty;
            long temp;

            if (!string.IsNullOrEmpty(isbn10) &&
                isbn10.Length == 10 &&
                Int64.TryParse(isbn10.Substring(0, 9), out temp))
            {
                int result = 0;
                isbn13 = "978" + isbn10.Substring(0, 9);
                for (int i = 0; i < isbn13.Length; i++)
                    result += int.Parse(isbn13[i].ToString()) * ((i % 2 == 0) ? 1 : 3);

                int checkDigit = (10 - (result % 10)) % 10;
                isbn13 += checkDigit.ToString();
            }

            return isbn13;
        }
    }
}
