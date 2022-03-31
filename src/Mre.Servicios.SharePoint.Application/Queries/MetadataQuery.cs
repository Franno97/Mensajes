using Microsoft.SharePoint.Client;
using Mre.Servicios.SharePoint.Application.Responses;
using System.Collections.Generic;
using System.Linq;

namespace Mre.Servicios.SharePoint.Application.Queries
{
  public class MetadataQuery
  {
    /// <summary>
    /// Método que obtiene los Tipos documentales según la configuración de requisitos
    /// </summary>
    /// <param name="clientContext"></param>
    /// <param name="titulo"></param>
    /// <returns></returns>
    public static List<RespuestaMetadata> ObtenerMetadata(ClientContext clientContext, string nombreArchivo)
    {
      string sufijo1 = nombreArchivo.Substring(nombreArchivo.IndexOf('_') + 1);
      string sufijo = sufijo1.Substring(0, sufijo1.IndexOf('.'));
      string biblioteca = "";

      switch (sufijo)
      {
        case "CEDU": { biblioteca = "Cedula"; break; }
        case "APEN": { biblioteca = "AntecedentesPenales"; break; }
        case "COND": { biblioteca = "CarnetDiscapacidad"; break; }
        case "PASP": { biblioteca = "Pasaporte"; break; }
        case "RCON": { biblioteca = "RegistroConsular"; break; }
        case "PAGO":
        case "PAGO1":
        case "PAGO2":
        case "PAGO3":
          {
            biblioteca = "PagoComprobante"; break;
          }
        case "PNAC": { biblioteca = "PartidaNacimiento"; break; }
        case "PMAT": { biblioteca = "PartidaMatrimonio"; break; }
        case "FOTO": { biblioteca = "Foto"; break; }

        default:
          break;
      }

      string consulta = "<Query><Where><Eq><FieldRef Name = 'Title'/><Value Type='Text'>'" + nombreArchivo + "'</Value></Eq></Where></Query>";

      // Obtenemos el contenido de los TiposDocumentales y configuramos las condiciones con que se van a recuperar
      var lista = clientContext.Web.Lists.GetByTitle(biblioteca);
      CamlQuery query = new CamlQuery
      {
        ViewXml = consulta
      };

      //+ "<FieldRef Name='FileLeafRef' />"
      //+ $"<Value Type='File'>{nombreArchivo}</Value>"

      var listaItems = lista.GetItems(query);
      clientContext.Load(listaItems);
      clientContext.ExecuteQuery();

      List<RespuestaMetadata> resultado = new List<RespuestaMetadata>();

      // Barremos la lista de registros obtenidos, convirtiendo en un tipo Lista RespuestaTipoDocumental
      foreach (var elemento in listaItems)
      {
        var item = Utiles.Utiles.ConvertirDiccionarioEnObjeto<RespuestaMetadata>(elemento.FieldValues);
        item.Mensaje = "OK";
        resultado.Add(item);
      };

      // En caso de no obtener registros, devolvemos un objeto con el tipo requerido pero con un mensaje de error
      if (resultado.Count == 0)
        return new List<RespuestaMetadata> { new RespuestaMetadata { Mensaje = $"Error, no se encontraron resultados que cumplan con los parámetros." } };

      return resultado.Where(x => x.FileLeafRef == nombreArchivo).ToList();
    }

  }
}
