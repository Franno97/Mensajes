using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Servicios.SharePoint.Application.Requests
{
    /// <summary>
    /// Clase request que obtiene los links configurados 
    /// </summary>
    public class PeticionLink: PeticionBase
    {
        public string Titulo { get; set; }
    }
}
