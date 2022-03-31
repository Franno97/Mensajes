using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Servicios.SharePoint.Application.Responses
{
  public class RespuestaMetadata
  {
    public string FileLeafRef { get; set; }
    public string Mensaje { get; set; }
    public string IdentificadorCiudadano { get; set; }
    public string NumeroRegistro { get; set; }
    public string Nombres { get; set; }
    public string Lugar { get; set; }
    public DateTime FechaEmision { get; set; }
    public string NumeroPasaporte { get; set; }
    public DateTime FechaCaducidad { get; set; }
    public string NumeroIdentificacion { get; set; }
    public string NumeroCarnet { get; set; }
    public string NumeroTransaccion { get; set; }
    public string NumeroCuenta { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Pais { get; set; }
    public string Padres { get; set; }

  }
}
