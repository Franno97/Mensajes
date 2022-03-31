using System;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.SharePoint.Client;
using Mre.Servicios.SharePoint.Application.Requests;
using Mre.Servicios.SharePoint.Application.Responses;
using System.Web.Http.Cors;

namespace Mre.Servicios.SharePoint.Api.Controllers
{
    [RoutePrefix("api")]
    public class LinksController : ApiController
    {
        #region Links

        /// <summary>
        /// Método que devuelve los mensajes configurados por módulo y página
        /// </summary>
        /// <param name="modulo"></param>
        /// <param name="pagina"></param>
        /// <returns></returns>
        [Route("links")]
        [HttpGet]
        [HttpOptions]
        [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]

        public List<RespuestaLink> Links()
        {
            // Objeto con las credenciales
            PeticionBase peticion = new PeticionBase
            {
                SiteURL = new Uri("http://172.31.3.27"),
                Environmentvalue = "onpremises",
                Username = "jfloachamin",
                Password = "Consultor1*",
                Dominio = "mre",
            };

            //Obteniendo el contexto de datos del Sharepoint, autenticando al usuario
            ClientContext contexto;
            try
            {
                contexto = Application.Authentication.Autenticar.AutenticarUsuario((PeticionBase)peticion);
            }
            catch (Exception ex)
            {
                return new List<RespuestaLink> { new RespuestaLink { Mensaje = $"Error: {ex.Message}." } };
            }

            //Notificamos al usuario que no se obtuvo el contexto 
            if (contexto == null)
                return new List<RespuestaLink> { new RespuestaLink { Mensaje = $"Error en el proceso de autenticación." } };

            // Retornamos el resultado de la consulta al SharePoint
            return Application.Queries.LinkQuery.ObtenerLinks(contexto);
        }

        #endregion
    }
}
