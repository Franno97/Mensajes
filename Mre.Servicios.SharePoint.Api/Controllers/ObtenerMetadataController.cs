using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Microsoft.SharePoint.Client;
using System.Security;
using Mre.Servicios.SharePoint.Application;
using Mre.Servicios.SharePoint.Application.Requests;
using System.Threading.Tasks;
using Mre.Servicios.SharePoint.Application.Responses;
using System.Web.Http.Cors;
using System.Reflection;

namespace Mre.Servicios.SharePoint.Api.Controllers
{
  [RoutePrefix("api")]
  public class ObtenerMetadataController : ApiController
  {

    #region Metadata
    /// <summary>
    /// Método que devuelve los tipos documentales según las condiciones del ciudadano (Personal - Familiar)
    /// </summary>
    /// <param name="nombreArchivo"></param>
    /// <returns></returns>
    [Route("obtenerMetadata")]
    [HttpGet]
    [HttpOptions]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
    public string TipoDocumental(string nombreArchivo)
    {
      // Objeto con las credenciales
      PeticionTipoDocumental peticion = new PeticionTipoDocumental
      {
        SiteURL = new Uri("http://172.31.3.27"),
        Environmentvalue = "onpremises",
        Username = "jfloachamin",
        Password = "Consultor1*",
        Dominio = "mre",
        Titulo = nombreArchivo
      };

      //Obteniendo el contexto de datos del Sharepoint, autenticando al usuario
      ClientContext contexto;
      try
      {
        contexto = Application.Authentication.Autenticar.AutenticarUsuario((PeticionBase)peticion);
      }
      catch (Exception ex)
      {
        return $"Error: {ex.Message}.";
      }

      //Notificamos al usuario que no se obtuvo el contexto 
      if (contexto == null)
        return $"Error en el proceso de autenticación.";

      // Retornamos el resultado de la consulta al SharePoint
      var datosObtenidos = Application.Queries.MetadataQuery.ObtenerMetadata(contexto, peticion.Titulo); ;

      var primerItem = datosObtenidos.FirstOrDefault();

      var resultado = ObjetoToString(primerItem);

      return resultado;


    }

    private string ObjetoToString(RespuestaMetadata primerItem)
    {
      string valor = string.Empty;

      Type myType = primerItem.GetType();
      IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

      foreach (PropertyInfo prop in props)
      {
        object propValue = prop.GetValue(primerItem, null);

        // Do something with propValue
        if (propValue != null && prop.Name != "FileLeafRef" && prop.Name != "Mensaje" && !propValue.ToString().Contains("1/0001"))
          valor += $"{prop.Name}: {propValue}{Environment.NewLine}";
      }

      return valor;
    }
    #endregion

  }
}
