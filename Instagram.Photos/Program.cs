﻿using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using Instagram.AuthService;
using Newtonsoft.Json.Linq;
using Instagram.Photos.AuthService;
using System.Windows.Forms;
using Instagram.Photos.Properties;

namespace Instagram.Photos
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //read from application configuration
            string clientId = Settings.Default.clientId;
            string clientSecret = Settings.Default.clientSecret;
            string tagName = Settings.Default.tagName;
            string outputDir = Settings.Default.outputDir;
            string hostAddress = Settings.Default.hostAddress;

            //Create server instance
            var instagramService = new InstagramAuthService(new Uri(hostAddress));
            
            //open the authentication server async and wait
            instagramService.Open();

            Console.WriteLine("server opened...");
            


            //try to get user authentication
            Application.Run(new InstagramAuthForm(String.Format("https://api.instagram.com/oauth/authorize?client_id={0}&client_secret={1}&response_type=code&redirect_uri={2}/instagram/Auth&redirect_type=code", clientId, clientSecret, hostAddress)));;

            //Try to post the code to server to get authorization code
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("client_id", clientId);
            parameters.Add("client_secret", clientSecret);
            parameters.Add("grant_type", "authorization_code");
            parameters.Add("redirect_uri", string.Format("{0}/instagram/Auth", hostAddress));
            parameters.Add("code", InstagramCodeContainer.GetInstance().AccessToken);
            
            WebClient client = new WebClient();
            var result = client.UploadValues(new Uri("https://api.instagram.com/oauth/access_token/"), parameters);

            var response = Encoding.Default.GetString(result);

            string responseAccessToken = JObject.Parse(response).SelectToken("access_token").ToString();

            WebRequest webRequest = null;
            string nextPageUrl = String.Empty;

            Console.WriteLine("we are ready to start downloading your photos, please hit enter...");
            Console.Read();

            do
            {
                if (webRequest == null && string.IsNullOrEmpty(nextPageUrl))
                    webRequest = HttpWebRequest.Create(String.Format("https://api.instagram.com/v1/tags/{0}/media/recent?access_token={1}", tagName, responseAccessToken));
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
