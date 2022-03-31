using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Servicios.SharePoint.Application.Requests
{
    /// <summary>
    /// Clase request que tiene las propiedades propias de la petición del mensaje
    /// </summary>
    public class PeticionMensaje : PeticionBase
    {

        public string Modulo { get; set; }
        public string Pagina { get; set; }
    }
}
