using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCrawler.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string HtmlContent { get; set; }

        public string ImgUrls { get; set; }
        public string Url { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
