using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Instagram.Photos.AuthService
{
    
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
        /// Open the server (async) and wait
        /// </summary>
        public void Open()
        {
            try
            {
                Server.OpenAsync().Wait();
            }
            catch (Exception)
            {
                Console.WriteLine("There was a problem while trying to open the server, verify you have administrative rights");
            }

        }

        /// <summary>
        /// Close the server async and wait
        /// </summary>
        public void Close()
        {
            try
            {
                if (Server != null)
                    Server.CloseAsync().Wait();
            }
            catch (Exception)
            {
                Console.WriteLine("There was a problem while trying to close the server");
            }
            
        }

    }

}
