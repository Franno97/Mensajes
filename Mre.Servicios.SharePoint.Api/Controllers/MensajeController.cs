using System;
using System.Linq;
using System.Net;
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
    public class MensajeController : ApiController
    {
        #region Mensajes

        /// <summary>
        /// Método que devuelve los mensajes configurados por módulo y página
        /// </summary>
        /// <param name="modulo"></param>
        /// <param name="pagina"></param>
        /// <returns></returns>
        [Route("mensaje")]
        [HttpGet]
        [HttpOptions]
        [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
        public RespuestaMensaje Mensaje(string modulo, string pagina)
        {
            // Objeto con las credenciales
            PeticionMensaje peticion = new PeticionMensaje
            {
                SiteURL = new Uri("http://172.31.3.27"),
                Environmentvalue = "onpremises",
                Username = "jfloachamin",
                Password = "Consultor1*",
                Dominio = "mre",
                Modulo = modulo,
                Pagina = pagina
            };

            //Obteniendo el contexto de datos del Sharepoint, autenticando al usuario
            ClientContext contexto;
            try
            {
                contexto = Application.Authentication.Autenticar.AutenticarUsuario((PeticionBase)peticion);
            }
            catch (Exception ex)
            {
                return new RespuestaMensaje { Mensaje = $"Error: {ex.Message}." };
            }

            //Notificamos al usuario que no se obtuvo el contexto 
            if (contexto == null)
                return new RespuestaMensaje { Mensaje = $"Error en el proceso de autenticación." };

            // Retornamos el resultado de la consulta al SharePoint
            return Application.Queries.MensajeQuery.ObtenerMensaje(contexto, peticion.Modulo, peticion.Pagina);
        }

        #endregion

    }
}
