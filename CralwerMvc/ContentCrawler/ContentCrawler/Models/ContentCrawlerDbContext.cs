using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCrawler.Models
{
    public class ContentCrawlerDbContext : DbContext
    {
        // Build connection string
        private SqlConnectionStringBuilder _builder;
        private static string DATA_SOURCE = @"HUY-LAPTOP\SQLEXPRESS"; //sql server name
        private static string INITIAL_CATALOG = "CrawlerDotNetMVC"; //satabase name want to create
        private static bool INTERGEATED_SERCURITY = true;

        public ContentCrawlerDbContext()
        {
            //config db context to connect sql server
            _builder = new SqlConnectionStringBuilder();
            _builder.DataSource = DATA_SOURCE;
            _builder.InitialCatalog = INITIAL_CATALOG;
            _builder.IntegratedSecurity = INTERGEATED_SERCURITY;
            this.Database.Connection.ConnectionString = _builder.ConnectionString;
        }
        public DbSet<Article> Articles { get; set; }
    }
}
