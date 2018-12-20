using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interfaz_scraper
{
    class GetParametros
    {
        public static DataTable getAllDocument()
        {
            DataTable dataTable = new DataTable("Parametro");
            dataTable.Columns.Add("ID", typeof(string));
            dataTable.Columns.Add("Parametro", typeof(string));
            dataTable.Columns.Add("Descripcion", typeof(string));


            var res = ConnectionToES.EsClient().Search<Parametro>(s => s
            .Index("parametros")
            .Type("scraper")
            .From(0)
            .Size(1000)
            .Query(q => q.MatchAll()));

            foreach (var hit in res.Hits)
            {
                dataTable.Rows.Add(hit.Id.ToString(), hit.Source.parametro.ToString(), hit.Source.descripcion.ToString());
            }


            return dataTable;
        }
    }
}
