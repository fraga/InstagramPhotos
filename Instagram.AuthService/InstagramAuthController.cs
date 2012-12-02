using System.Web.Http;
using Instagram.AuthService;

namespace Instagram.Photos.AuthService
{
    /// <summary>
    /// COntroller which will take care of the authentication
    /// </summary>
    public class InstagramAuthController : ApiController
    {
        /// <summary>
        /// Authentication action
        /// </summary>
        /// <param name="code">code parameter that will be passed</param>
        /// <returns></returns>
        [HttpGet]
        public void Auth(string code = "")
        {
            if (!string.IsNullOrEmpty(code))
            {
                InstagramCodeContainer.GetInstance().AccessToken = code;
            }
        }
    }
}
