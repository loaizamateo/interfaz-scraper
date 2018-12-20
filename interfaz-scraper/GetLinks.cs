using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interfaz_scraper
{
    class GetLinks
    {
        public static DataTable getAllDocument()
        {
            DataTable dataTable = new DataTable("Parametro");
            dataTable.Columns.Add("ID", typeof(string));
            dataTable.Columns.Add("Pagina", typeof(string));


            var res = ConnectionToES.EsClient().Search<Links>(s => s
                .Index("links")
                .Type("scraper")
                .Size(0)
                .Aggregations(q => 
                    q.Terms("my_agg", st => 
                    st.Field("pagina.keyword")
                    )
                )
            );

            foreach (var hit in res.Hits)
            {
                dataTable.Rows.Add(hit.Id.ToString(), hit.Source.pagina.ToString());
            }


            return dataTable;
        }
    }
}
