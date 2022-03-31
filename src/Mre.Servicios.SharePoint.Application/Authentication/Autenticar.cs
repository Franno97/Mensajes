using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Net;
using Microsoft.SharePoint.Client;
using Mre.Servicios.SharePoint.Application.Requests;

namespace Mre.Servicios.SharePoint.Application.Authentication
{
    public class Autenticar
    {
        public static ClientContext AutenticarUsuario(PeticionBase peticionMensaje)
        {
            ClientContext context = null;
            string environmentvalue = peticionMensaje.Environmentvalue;
            string username = peticionMensaje.Username;
            string password = peticionMensaje.Password;
            string dominio = peticionMensaje.Dominio;
            Uri targetSiteUrl = peticionMensaje.SiteURL;

            try
            {
                // Based on the environmentvalue provided it execute the function.
                if (string.Compare(environmentvalue, "onpremises", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    context = LogOn(username, password, dominio, targetSiteUrl);
                    // isAuthenticated = true;
                    // You can write additional methods here which you want to use after authentication
                }
                else if (string.Compare(environmentvalue, "o365", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    context = O365LogOn(username, password, targetSiteUrl);
                    // isAuthenticated = true;
                    // You can write additional methods here which you want to use after authentication
                }
                else if (string.Compare(environmentvalue, "extranet", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    context = ExtranetLogOn(username, password, targetSiteUrl);
                    // isAuthenticated = true;
                    // You can write additional methods here which you want to use after authentication
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return context;
        }

        private static ClientContext LogOn(string userName, string password, string dominio, Uri url)
        {
            ClientContext clientContext = null;
            ClientContext ctx;
            try
            {
                clientContext = new ClientContext(url);

                // Condition to check whether the user name is null or empty.
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    SecureString securestring = new SecureString();
                    password.ToCharArray().ToList().ForEach(s => securestring.AppendChar(s));
                    clientContext.Credentials = new System.Net.NetworkCredential(userName, securestring, dominio);


                    clientContext.Load(clientContext.Web);
                    clientContext.ExecuteQuery();
                    var titulo = clientContext.Web.Title;


                }
                else
                {
                    clientContext.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                    clientContext.ExecuteQuery();
                }

                ctx = clientContext;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (clientContext != null)
                {
                    clientContext.Dispose();
                }
            }

            return ctx;
        }
        private static ClientContext O365LogOn(string userName, string password, Uri url)
        {
            ClientContext clientContext = null;
            ClientContext ctx = null;
            try
            {
                clientContext = new ClientContext(url);

                // Condition to check whether the user name is null or empty.
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    SecureString securestring = new SecureString();
                    password.ToCharArray().ToList().ForEach(s => securestring.AppendChar(s));
                    clientContext.Credentials = new SharePointOnlineCredentials(userName, securestring);
                    clientContext.ExecuteQuery();
                }
                else
                {
                    clientContext.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                    clientContext.ExecuteQuery();
                }
                ctx = clientContext;
            }
            finally
            {
                if (clientContext != null)
                {
                    clientContext.Dispose();
                }
            }
            return ctx;
        }
        private static ClientContext ExtranetLogOn(string userName, string password, Uri url)
        {
            ClientContext clientContext = null;
            ClientContext ctx;
            try
            {
                clientContext = new ClientContext(url);

                // Condition to check whether the user name is null or empty.
                if (!string.IsNullOrEmpty(userName))
                {
                    NetworkCredential networkCredential = new NetworkCredential(userName, password);
                    CredentialCache cc = new CredentialCache();
                    cc.Add(url, "NTLM", networkCredential);
                    clientContext.Credentials = cc;
                    clientContext.ExecuteQuery();
                }
                else
                {
                    CredentialCache cc = new CredentialCache();
                    cc.Add(url, "NTLM", System.Net.CredentialCache.DefaultNetworkCredentials);
                    clientContext.Credentials = cc;
                    clientContext.ExecuteQuery();
                }
                ctx = clientContext;
            }
            finally
            {
                if (clientContext != null)
                {
                    clientContext.Dispose();
                }
            }
            return ctx;
        }

    }
}
