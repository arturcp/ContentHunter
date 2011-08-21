
using ContentHunter.Web.Models.Util;
namespace ContentHunter.Web.Models
{
    public class CrawlerResult
    {
        public CrawlerResult()
        {
            ErrorCode = Enum.ErrorCodes.NoErrors;
            Title = Content = Message = ErrorMessage = string.Empty;
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public string Message { get; set; }
        public ContentHunter.Web.Models.Util.Enum.ErrorCodes ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}