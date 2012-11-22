using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Instagram.Photos.AuthService
{
    /// <summary>
    /// COntroller which will take care of the authentication
    /// </summary>
    public class InstagramController : ApiController
    {
        public InstagramController()
        {
            Console.Write("This was created");
        }

        /// <summary>
        /// Authentication action
        /// </summary>
        /// <param name="code">code parameter that will be passed</param>
        /// <returns></returns>
        public string Auth(string code)
        {
            return code;
        }
    }

    /// <summary>
    /// The Self host service class
    /// </summary>
    public class InstagramAuthService
    {
        /// <summary>
        /// Self host configuration
        /// </summary>
        private HttpSelfHostConfiguration Configuration { get; set; }
        /// <summary>
        /// Self host server 
        /// </summary>
        private HttpSelfHostServer Server { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="address">THe http address to bind</param>
        public InstagramAuthService(Uri address)
        {
            Configuration = new HttpSelfHostConfiguration(address);

            Configuration.Routes.MapHttpRoute("default", "{controller}/{action}/{code}", new { code = RouteParameter.Optional });

            Server = new HttpSelfHostServer(Configuration);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~InstagramAuthService()
        {
            Close();
        }

        /// <summary>
        /// Open the server (async)
        /// </summary>
        public void Open()
        {
            try
            {
                Server.OpenAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("There was a problem while trying to open the server, verify you have administrative rights");
            }

        }

        /// <summary>
        /// Close the server
        /// </summary>
        public void Close()
        {
            try
            {
                if (Server != null)
                    Server.CloseAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("There was a problem while trying to close the server");
            }
            
        }
    }

}
