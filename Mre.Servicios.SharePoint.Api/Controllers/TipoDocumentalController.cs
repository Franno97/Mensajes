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

namespace Mre.Servicios.SharePoint.Api.Controllers
{
    [RoutePrefix("api")]
    public class TipoDocumentalController : ApiController
    {

        #region TiposDocumentales
        /// <summary>
        /// Método que devuelve los tipos documentales según las condiciones del ciudadano (Personal - Familiar)
        /// </summary>
        /// <param name="titulo"></param>
        /// <returns></returns>
        [Route("tipoDocumental")]
        [HttpGet]
        [HttpOptions]
        [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
        public List<RespuestaTipoDocumental> TipoDocumental(string titulo)
        {
            // Objeto con las credenciales
            PeticionTipoDocumental peticion = new PeticionTipoDocumental
            {
                SiteURL = new Uri("http://172.31.3.27"),
                Environmentvalue = "onpremises",
                Username = "jfloachamin",
                Password = "Consultor1*",
                Dominio = "mre",
                Titulo = titulo
            };

            //Obteniendo el contexto de datos del Sharepoint, autenticando al usuario
            ClientContext contexto;
            try
            {
                contexto = Application.Authentication.Autenticar.AutenticarUsuario((PeticionBase)peticion);
            }
            catch (Exception ex)
            {
                return new List<RespuestaTipoDocumental> { new RespuestaTipoDocumental { Mensaje = $"Error: {ex.Message}." } };
            }

            //Notificamos al usuario que no se obtuvo el contexto 
            if (contexto == null)
                return new List<RespuestaTipoDocumental> { new RespuestaTipoDocumental { Mensaje = $"Error en el proceso de autenticación." } };

            // Retornamos el resultado de la consulta al SharePoint
            return Application.Queries.TipoDocumentalQuery.ObtenerTipoDocumental(contexto, peticion.Titulo);
        }
        #endregion

    }
}
