using Microsoft.SharePoint.Client;
using Mre.Servicios.SharePoint.Application.Responses;
using System.Collections.Generic;

namespace Mre.Servicios.SharePoint.Application.Queries
{
    public class TipoDocumentalQuery
    {
        /// <summary>
        /// Método que obtiene los Tipos documentales según la configuración de requisitos
        /// </summary>
        /// <param name="clientContext"></param>
        /// <param name="titulo"></param>
        /// <returns></returns>
        public static List<RespuestaTipoDocumental> ObtenerTipoDocumental(ClientContext clientContext, string titulo)
        {
            // Obtenemos el contenido de los TiposDocumentales y configuramos las condiciones con que se van a recuperar
            var lista = clientContext.Web.Lists.GetByTitle("TiposDocumentales");
            CamlQuery query = new CamlQuery
            {
                ViewXml = "<View>" +
                                "<Query>" +
                                    "<Where>" +
                                       "<And>" +
                                          "<Eq>" +
                                                  "<FieldRef Name = 'Title'/>" +
                                                  "<Value Type='Text'>" + titulo + "</Value>" +
                                              "</Eq>" +
                                               "<Eq>" +
                                                  "<FieldRef Name = 'Habilitado'/>" +
                                                  "<Value Type='Text'>true</Value>" +
                                            "</Eq>" +
                                         "</And>" +
                                    "</Where>" +
                                "</Query>" +
                                "<RowLimit>20</RowLimit>" +
                            "</View>"
            };

            var listaItems = lista.GetItems(query);
            clientContext.Load(listaItems);
            clientContext.ExecuteQuery();

            List<RespuestaTipoDocumental> resultado = new List<RespuestaTipoDocumental>();

            // Barremos la lista de registros obtenidos, convirtiendo en un tipo Lista RespuestaTipoDocumental
            foreach (var elemento in listaItems)
            {
                var item = Utiles.Utiles.ConvertirDiccionarioEnObjeto<RespuestaTipoDocumental>(elemento.FieldValues);
                item.Mensaje = "OK";
                resultado.Add(item);
            };

            // En caso de no obtener registros, devolvemos un objeto con el tipo requerido pero con un mensaje de error
            if (resultado.Count == 0)
                return new List<RespuestaTipoDocumental> { new RespuestaTipoDocumental { Mensaje = $"Error, no se encontraron resultados que cumplan con los parámetros." } };

            return resultado;
        }

    }
}
