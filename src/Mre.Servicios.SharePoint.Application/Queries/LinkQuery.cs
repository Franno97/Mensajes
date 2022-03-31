using Microsoft.SharePoint.Client;
using Mre.Servicios.SharePoint.Application.Responses;
using System.Collections.Generic;

namespace Mre.Servicios.SharePoint.Application.Queries
{
    public class LinkQuery
    {
        // Obtenemos el contenido de la biblioteca de Links y configuramos las condiciones con que se van a recuperar
        public static List<RespuestaLink> ObtenerLinks(ClientContext clientContext)
        {
            var lista = clientContext.Web.Lists.GetByTitle("Links");
            CamlQuery query = new CamlQuery
            {
                ViewXml = "<View>" +
                                "<RowLimit>50</RowLimit>" +
                            "</View>"
            };

            var listaItems = lista.GetItems(query);
            clientContext.Load(listaItems);
            clientContext.ExecuteQuery();

            List<RespuestaLink> resultado = new List<RespuestaLink>();

            // Barremos la lista de registros obtenidos, convirtiendo en un tipo Lista RespuestaTipoDocumental
            foreach (var elemento in listaItems)
            {
                var item = Utiles.Utiles.ConvertirDiccionarioEnObjeto<RespuestaLink>(elemento.FieldValues);
                item.Mensaje = "OK";
                resultado.Add(item);
            };

            // En caso de no obtener registros, devolvemos un objeto con el tipo requerido pero con un mensaje de error
            if (resultado.Count == 0)
                return new List<RespuestaLink> { new RespuestaLink { Mensaje = $"Error, no se encontraron resultados que cumplan con los parámetros." } };

            return resultado;
        }
    }
}
