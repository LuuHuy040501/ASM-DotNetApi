using ContentCrawler.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCrawler.Crawler
{
    public class VnExpressCrawler : ICrawler
    {
        public Article CrawlArticle(string url)
        {
            //load html document
            HtmlDocument document = LoadHtml(url);
            //get title
            var title = CrawlTitle(document);
            //get description
            var description = CrawlDescription(document);
            //get content html
            var htmlContent = CrawlHtmlContent(document);
            //get img
            var imgs = CrawlImgUrls(document);
            Article article = new Article()
            {
                Title = title,
                Description = description,
                HtmlContent = htmlContent,
                ImgUrls = imgs,
                Url = url,
                CreateAt = DateTime.Now

            };

            return article;
        }

        private HtmlDocument LoadHtml(string url)
        {
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
            };

            //Load trang web, nạp html vào document
            HtmlDocument document = htmlWeb.Load(url);
            return document;
        }

        private string CrawlTitle(HtmlDocument document)
        {
            var titleSelector = "h1.title-detail";
            if(titleSelector == null)
            {
                return null;
            }
            var title = document.DocumentNode.QuerySelector(titleSelector).InnerText;
            return title;
        }

        private string CrawlDescription(HtmlDocument document)
        {
            var descriptionSelector = "p.description";
            var description = document.DocumentNode.QuerySelector(descriptionSelector).InnerText;
            return description;
        }
        private string CrawlHtmlContent(HtmlDocument document)
        {
            //get contents
            var contentSelector = "article.fck_detail";
            var contentNodes = document.DocumentNode.QuerySelector(contentSelector);
            var articleChildeNodes = contentNodes.ChildNodes;
            //build html content
            var contentHtml = new StringBuilder();
            foreach (var childNode in articleChildeNodes)
            {
                if (!childNode.Name.Equals("div") && !childNode.Name.Equals("#text"))
                {
                    if (childNode.Name.Equals("p"))
                    {
                        contentHtml.Append($"<p class='article-para'>{childNode.InnerText}</p>");
                        contentHtml.Append("\n");
                    }
                    if (childNode.Name.Equals("figure"))
                    {
                        //get image
                        var imgUrl = childNode.QuerySelector("img").Attributes["data-src"].Value;
                        //get image caption
                        var imgCaption = childNode.QuerySelector("figcaption").InnerHtml;
                        contentHtml.Append("<figure class='article-figure'>");
                        contentHtml.Append("\n");
                        contentHtml.Append($"\t<img class ='article-img' src='{imgUrl}'/>");
                        contentHtml.Append("\n");
                        contentHtml.Append("\t<figcaption class ='article-img-caption'>\n");
                        contentHtml.Append("\t\t");
                        contentHtml.Append(imgCaption.Trim());
                        contentHtml.Append("\n\t</figcaption>");
                        contentHtml.Append("\n");
                        contentHtml.Append("</figure>");
                        contentHtml.Append("\n");
                    }
                }

            }
            return contentHtml.ToString();
        }

        private string CrawlImgUrls(HtmlDocument document)
        {
            //get images if exist
            //use set to avoid duplicate
            var imgUrlSet = new HashSet<string>();
            var urlList = new StringBuilder();
            //get img nodes
            var imgSelector = "article.fck_detail figure img";
            var imgNodes = document.DocumentNode.QuerySelectorAll(imgSelector).ToList();
            if (imgNodes.Count == 0)
            {
                return urlList.ToString();
            }
            //add url to set
            foreach (var imgNode in imgNodes)
            {

                var imgUrl = imgNode.Attributes["data-src"].Value;
                imgUrlSet.Add(imgUrl);
            }
            //convert to string separated by comma to save in db
            foreach (var url in imgUrlSet)
            {
                urlList.Append(url);
                urlList.Append(",");
            }
            //remove the last comma
            urlList.Length--;
            return urlList.ToString();
        }

    }
}
