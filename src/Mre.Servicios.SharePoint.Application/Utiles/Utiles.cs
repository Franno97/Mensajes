using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Mre.Servicios.SharePoint.Application.Utiles
{
  public static class Utiles
  {
    /// <summary>
    /// Este método sirve para convertir una diccionario de datos de la siguiente estructura: <![CDATA[<string, object>]]>; en
    /// una clase relacionada con los datos que tiene el diccionario, pues intentará mediante la clave que tiene
    /// el diccionario utilizarla como el atributo de la clase, y de esta manera asignar el valor que tiene el diccionario.
    /// </summary>
    /// <typeparam name="T">Este parámetro contiene el objeto que será devuelto al final de la conversión.</typeparam>
    /// <param name="p_diccionario">Este parámetro contiene los datos que serán convertidos, estos datos vienen en la forma de un diccionario: <![CDATA[Dictionary<string, object>]]>.</param>
    /// <returns>Retorna un objeto de la clase enviada con la información del diccionario.</returns>
    public static T ConvertirDiccionarioEnObjeto<T>(Dictionary<string, object> p_diccionario) where T : new()
    {
      var objeto = new T();
      PropertyInfo[] propiedades = objeto.GetType().GetProperties();
      foreach (PropertyInfo propiedad in propiedades)
      {
        try
        {
          if (!p_diccionario.Any(x => x.Key.Equals(propiedad.Name, StringComparison.InvariantCultureIgnoreCase)))
            continue;
          KeyValuePair<string, object> item = p_diccionario.First(x => x.Key.Equals(propiedad.Name, StringComparison.InvariantCultureIgnoreCase));
          // Buscar que tipo de propiedad es la propiedad actual (int, string, double?, etc).
          Type tipoPropiedad = objeto.GetType().GetProperty(propiedad.Name).PropertyType;
          // Arreglar las propiedades que pueden ser nulas.
          Type nuevoTipo = Nullable.GetUnderlyingType(tipoPropiedad) ?? tipoPropiedad;
          // Convertir el tipo al que debe ser.
          object nuevoValor = Convert.ChangeType(item.Value, nuevoTipo);
          objeto.GetType().GetProperty(propiedad.Name).SetValue(objeto, nuevoValor, null);
        }
        catch (Exception)
        {
        }
      }
      return objeto;
    }

    /// <summary>
    /// Este método sirve para convertir un objeto instanciado de una clase T en un diccionario de datos de la siguiente 
    /// estructura: <![CDATA[<string, object>]]>. Donde el parámetro string contiene el nombre del campo y el parámetro object contiene
    /// el valor del campo. El resultado es un diccionario sin distinción de mayúsculas o minúsculas en la clave (nombre del campo).
    /// </summary>
    /// <typeparam name="T">Este parámetro contiene el objeto que será devuelto al final de la conversión.</typeparam>
    /// <param name="p_objeto">Este parámetro contiene los datos que serán convertidos</param>
    /// <returns>Retorna un diccionario con la información enviada en el parámetro objeto.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IDictionary<string, object> ConvertirObjetoEnDiccionario<T>(object p_objeto)
    {
      if (p_objeto == null)
        throw new ArgumentNullException("p_objeto", "El parámetro p_objeto no puede tener un valor nulo.");

      var diccionario = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

      foreach (PropertyDescriptor propiedad in TypeDescriptor.GetProperties(p_objeto))
        AgregarPropiedadAlDiccionario<object>(propiedad, p_objeto, diccionario);

      return diccionario;
    }

    /// <summary>
    /// Este método agrega una propiedad de un objeto a un elemento de un diccionario.
    /// </summary>
    /// <typeparam name="T">Este parámetro contiene la plantilla del objeto que esta siendo procesado.</typeparam>
    /// <param name="p_propiedad">Este parámetro contiene la propiedad que será agregada al diccionario.</param>
    /// <param name="p_objeto">Este parámetro contiene el objeto donde está el valor de la propiedad.</param>
    /// <param name="p_diccionario">Este parámetro contiene el diccionario donde se agregará la propiedad con su valor.</param>
    private static void AgregarPropiedadAlDiccionario<T>(PropertyDescriptor p_propiedad, object p_objeto, Dictionary<string, object> p_diccionario)
    {
      object valor = p_propiedad.GetValue(p_objeto);
      if (valor is T)
        p_diccionario.Add(p_propiedad.Name, (T)valor);
    }


  }
}
