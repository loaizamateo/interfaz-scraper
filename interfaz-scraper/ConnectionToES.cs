using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interfaz_scraper
{
    class ConnectionToES
    {
        public static ElasticClient EsClient()
        {
            ConnectionSettings connectionSettings;
            ElasticClient elasticClient;
            connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200/"));
            elasticClient = new ElasticClient(connectionSettings);
            return elasticClient;
        }
    }
}
