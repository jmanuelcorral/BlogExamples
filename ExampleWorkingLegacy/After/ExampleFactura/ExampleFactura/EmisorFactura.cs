using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ExampleFactura
{

    public class Factura
    {
        public int Id { get; set; }
        /* Aqui van otras propiedades que obviamos */
        public Boolean Emitida { get; set; }
        public DateTime FechaEmision { get; set; }
    }

    /// <summary>
    /// Codigo Intesteable, Muy Acoplado y muy facil de ver hoy en día, 
    /// Considerado Codigo Legacy desde que es picado
    /// </summary>
    public class EmisorFactura
    {
        public ILog logger { get; set; }
        
        public EmisorFactura()
        {
            this.logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public Factura EmiteFactura(int idFactura)
        {
            var miFactura = LoadFactura(idFactura);
            miFactura.Emitida = true;
            miFactura.FechaEmision = DateTime.Now;
            if (SaveFactura(miFactura) > 0)
            {
                logger.Info("Salvado de Factura Emitida " + idFactura);
                return miFactura;
            }
            else
            {
                logger.Info("Error salvando Factura Emitida " + idFactura);
                return miFactura;
            }
        }

        private Factura LoadFactura(int factura)
        {
            string FacturasConnection = ConfigurationManager.ConnectionStrings["FacturasCS"].ConnectionString;
            SqlDataAdapter custAdapter = new SqlDataAdapter("SELECT * FROM dbo.Facturas Where idFactura=" + factura + "", FacturasConnection);
            DataSet Facturas = new DataSet();
            custAdapter.Fill(Facturas, "Facturas");
            Factura miFactura = null;
            if (Facturas.Tables.Count>0 && Facturas.Tables["Facturas"].Rows.Count>0)
                foreach (DataRow pRow in Facturas.Tables["Facturas"].Rows)
                {
                   miFactura = new Factura();
                    miFactura.Id = Int32.Parse(pRow["id"].ToString());
                    miFactura.Emitida = Boolean.Parse(pRow["Emitida"].ToString());
                    miFactura.FechaEmision = DateTime.Parse(pRow["FechaEmision"].ToString());
                }
            return miFactura;
        }

        private int SaveFactura(Factura factura2Save)
        {
            ///Codigo que se conecta a la Base de datos por adonet, por poner un ejemplo, en el peor de los casos será una stored
            string FacturasConnection = ConfigurationManager.ConnectionStrings["FacturasCS"].ConnectionString;

        }
    }
}
