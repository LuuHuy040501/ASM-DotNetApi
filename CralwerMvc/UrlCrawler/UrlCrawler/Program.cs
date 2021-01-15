using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UrlCrawler
{
    class Program
    {
        static  void Main(string[] args)
        {
            string url = "https://vnexpress.net/suc-khoe/dinh-duong";
            var listUrl = GetArticleUrl(url);
            Console.WriteLine("Crawled succesfully " + listUrl.Count + " article link from " + url);
            PublishUrlsToQueue(listUrl);
            Console.WriteLine("Task completed press any key to exit....");
            Console.ReadLine();
            
        }

        private static HashSet<string> GetArticleUrl(string journalUrl)
        {
            //sử dụng set để k bị duplicate
            HashSet<string> setLink = new HashSet<string>();
            
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
            };

            //Load trang web, nạp html vào document
            HtmlDocument document = htmlWeb.Load(journalUrl);
            //lấy ra toàn bộ thẻ a
            var aItems = document.DocumentNode.QuerySelectorAll("a").ToList();
            //lấy aritcle url và lưu vào set
            foreach (var item in aItems)
            {
                var link = item.Attributes["href"].Value;
                //loại link k có .html và có #
                if (link.Contains(".html") && !link.Contains("#"))
                {
                    setLink.Add(link);
                }

            }
    
            return setLink;
        }

        private static void PublishUrlsToQueue(IEnumerable<string> listUrl)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            foreach (var url in listUrl)
            {


                channel.QueueDeclare(queue: "amqHelloRabbitMq",
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);


                var body = Encoding.UTF8.GetBytes(url);
                
                channel.BasicPublish(exchange: "",
                                     routingKey: "amqHelloRabbitMq",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine("Sent successfully url {0}", url);
            }
            channel.Close();
            connection.Close();
        }
    }
}
