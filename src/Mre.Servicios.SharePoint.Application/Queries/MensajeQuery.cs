using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Net;
using Microsoft.SharePoint.Client;
using Mre.Servicios.SharePoint.Application.Responses;

namespace Mre.Servicios.SharePoint.Application.Queries
{
    public class MensajeQuery
    {
        /// <summary>
        /// /// Método que obtiene los mensajes según el módulo y página indicados
        /// </summary>
        /// <param name="clientContext"></param>
        /// <param name="modulo"></param>
        /// <param name="pagina"></param>
        /// <returns></returns>
        public static RespuestaMensaje ObtenerMensaje(ClientContext clientContext, string modulo, string pagina)
        {
            // Obtenemos el contenido de los mensajes y configuramos las condiciones con que se van a recuperar
            var lista = clientContext.Web.Lists.GetByTitle("Mensajes");
            CamlQuery query = new CamlQuery
            {
                ViewXml = "<View>" +
                                "<Query>" +
                                    "<Where>" +
                                        "<And>" +
                                            "<Eq>" +
                                                "<FieldRef Name = 'Title'/>" +
                                                "<Value Type='Text'>" + modulo + "</Value>" +
                                            "</Eq>" +
                                            "<Eq>" +
                                                "<FieldRef Name = 'Pagina'/>" +
                                                "<Value Type='Text'>" + pagina + "</Value>" +
                                            "</Eq>" +
                                        "</And>" +
                                    "</Where>" +
                                "</Query>" +
                                "<RowLimit>1</RowLimit>" +
                            "</View>"
            };

            var listaItems = lista.GetItems(query);
            clientContext.Load(listaItems);
            clientContext.ExecuteQuery();

            // Obtenemos el primero elemento de la lista
            var elemento = listaItems.FirstOrDefault();

            // Si no se encontró un elemento, se devuelve error al usuario
            if (elemento == null)
                return new RespuestaMensaje { Mensaje = "Error al obtener la lista de mensajes." };

            var respuesta = Utiles.Utiles.ConvertirDiccionarioEnObjeto<RespuestaMensaje>(elemento.FieldValues);

            return respuesta;
        }


    }
}
