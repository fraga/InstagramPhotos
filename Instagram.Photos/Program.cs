using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;
using Instagram.Photos.AuthService;
using System.Windows.Forms;
using System.Net.Http;
using System.Configuration;

namespace Instagram.Photos
{
    class Program
    {
        static void Main(string[] args)
        {
            //read from application configuration
            string clientId = ConfigurationManager.AppSettings["clientId"];
            string clientSecret = ConfigurationManager.AppSettings["clientSecret"];
            string tagName = ConfigurationManager.AppSettings["tagName"];
            string outputDir = ConfigurationManager.AppSettings["outputDir"];
            string wcfHostAddress = ConfigurationManager.AppSettings["wcfHostAddress"];

            //if (!Directory.Exists(outputDir))
            //    throw new DirectoryNotFoundException("Dir was not found");
            
            //following code needs to be implemented if we need to collect the images from closed users
            //try to get user authentication
            //Application.Run(new InsntagramAuthForm(String.Format("https://api.instagram.com/oauth/authorize?client_id={0}&client_secret={1}&grant_type=authorization_code&redirect_uri=http://localhost:8080/Auth&redirect_type=code", client_id, client_secret));

            //Try to post the code to server to get authorization code
            //NameValueCollection parameters = new NameValueCollection();
            //parameters.Add("client_id", client_id);
            //parameters.Add("client_secret", clientSecret);
            //parameters.Add("grant_type", "authorization_code");
            //parameters.Add("redirect_uri", "http://localhost:8080");
            //var code = "";
            //parameters.Add("response_type", code);
            
            //WebClient client = new WebClient();
            //var result = client.UploadValues("https://api.instagram.com/oauth/access_token/", parameters);

            //var response = System.Text.Encoding.Default.GetString(result);
            //Console.Read();

            WebRequest webRequest = null;
            string nextPageUrl = String.Empty;


            do
            {
                if (webRequest == null && string.IsNullOrEmpty(nextPageUrl))
                    webRequest = HttpWebRequest.Create(String.Format("https://api.instagram.com/v1/tags/{0}/media/recent?client_id={1}", tagName, clientId));
                else
                    webRequest = HttpWebRequest.Create(nextPageUrl);

                var responseStream = webRequest.GetResponse().GetResponseStream();
                Encoding encode = System.Text.Encoding.Default;


                using (StreamReader reader = new StreamReader(responseStream, encode))
                {
                    JToken token = JObject.Parse(reader.ReadToEnd());
                    var pagination = token.SelectToken("pagination");

                    if (pagination != null && pagination.SelectToken("next_url") != null)
                    {
                        nextPageUrl = pagination.SelectToken("next_url").ToString();
                    }
                    else
                    {
                        nextPageUrl = null;
                    }

                    var images = token.SelectToken("data").ToArray();

                    foreach (var image in images)
                    {
                        var imageUrl = image.SelectToken("images").SelectToken("standard_resolution").SelectToken("url").ToString();

                        if (String.IsNullOrEmpty(imageUrl))
                            Console.WriteLine("broken image URL");

                        var imageResponse = HttpWebRequest.Create(imageUrl).GetResponse().GetResponseStream();

                        var imageId = image.SelectToken("id");

                        using(var imageWriter = new StreamWriter(String.Format("{0}\\{1}.jpg", outputDir, imageId)))
                        {
                            imageResponse.CopyTo(imageWriter.BaseStream);
                            imageResponse.Flush();
                            Console.WriteLine("copied {0}", imageId);
                        }

                    }


                }

            }
            while (!String.IsNullOrEmpty(nextPageUrl));
            
            Console.Read();
        }


    }
}
