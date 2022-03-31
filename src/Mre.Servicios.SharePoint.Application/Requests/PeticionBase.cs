using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Servicios.SharePoint.Application.Requests
{
    /// <summary>
    /// Clase base con las propiedades de autenticación
    /// </summary>
    public class PeticionBase
    {
        public Uri SiteURL { get; set; }
        public string Environmentvalue { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Dominio { get; set; }
    }
}
