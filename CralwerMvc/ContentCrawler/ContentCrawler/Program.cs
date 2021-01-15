using ContentCrawler.Crawler;
using ContentCrawler.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCrawler
{
    class Program
    {
        private static ContentCrawlerDbContext _db = new ContentCrawlerDbContext();
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            ICrawler crawler = new VnExpressCrawler();
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "amqHelloRabbitMq",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var url = Encoding.UTF8.GetString(body);
                  
                    Console.WriteLine("[Received] {0}", url);
                    //check exist url
                    if (UrlIsExisted(url))
                    {
                        Console.WriteLine("[Existed] {0}", url);
                        Console.WriteLine("============================================================================");
                        return;
                    }
                    //craw
                    var article = crawler.CrawlArticle(url);
                    Console.WriteLine("[Crawl success] {0}", article.Title);
                    //save to DB
                    _db.Articles.Add(article);
                    _db.SaveChanges();
                    Console.WriteLine("[Saved sucesssfully] {0}", article.Title);
                    Console.WriteLine("============================================================================");
                };
                channel.BasicConsume(queue: "amqHelloRabbitMq",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

        }

        private static bool UrlIsExisted(string url)
        {
            var listExistUrl = from article in _db.Articles where article.Url == url select article.Url;
            if(listExistUrl.Count() == 0)
            {
                return false;
            } 
            else
            {
                return true;
            }        
        }
        
    }
}
