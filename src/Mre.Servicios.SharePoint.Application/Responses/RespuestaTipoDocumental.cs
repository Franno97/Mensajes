using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Servicios.SharePoint.Application.Responses
{
  /// <summary>
  /// Clase RespuestaTipoDocumental
  /// </summary>
  public class RespuestaTipoDocumental
  {
    public string Mensaje { get; set; }

    public string Title { get; set; }

    public string CodigoTipo { get; set; }

    public string CodigoImagenDocumento { get; set; }

    public bool Habilitado { get; set; }

    //public Microsoft.SharePoint.Client.FieldUrlValue ImagenDocumento { get; set; }

    public bool Obligatorio { get; set; }

    public string TipoDocumento { get; set; }

    public string Requisito { get; set; }

    public string ImagenNombre { get; set; }

    public string IconoNombre { get; set; }

    public int Edad { get; set; }

    public string TipoDocumental { get; set; }

  }
}
