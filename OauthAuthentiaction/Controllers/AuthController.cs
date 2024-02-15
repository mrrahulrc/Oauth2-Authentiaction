using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OauthAuthentiaction.Constants;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.JavaScript;
using System.Text;

namespace OauthAuthentiaction.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult oauth([FromServices] IConfiguration configuration)
        {

            QueryBuilder que = new QueryBuilder();
            que.Add("response_type","code");
            que.Add("client_id", configuration.GetValue<string>("Authentication:client_id"));
            que.Add("scope", String.Join(' ',OAuthScope.scope));
            que.Add("redirect_uri", configuration.GetValue<string>("Authentication:redirect_uri"));

            return Redirect(OAuthUrl.authCodeUrl + que.ToQueryString());
        }

        public async Task<IActionResult> token([FromServices] IConfiguration configuration, [FromServices] IDataProtectionProvider dataProtectionProvider)
        {

            String code = HttpContext.Request.Query["code"];

            HttpClient httpClient = new HttpClient();
            JObject obj = new JObject();
            obj.Add("grant_type", "authorization_code");
            obj.Add("code", code);
            obj.Add("redirect_uri", configuration.GetValue<string>("Authentication:redirect_uri"));
            obj.Add("client_id", configuration.GetValue<string>("Authentication:client_id"));
            obj.Add("client_secret", configuration.GetValue<string>("Authentication:client_secret"));

            StringContent str = new StringContent(obj.ToString(), Encoding.UTF8, new MediaTypeHeaderValue("application/json"));

            HttpResponseMessage res =  await httpClient.PostAsync("https://oauth2.googleapis.com/token", str);

            String response;
            if ( res.IsSuccessStatusCode )
            {
                response = await res.Content.ReadAsStringAsync();
                JObject? resp = JsonConvert.DeserializeObject(response) as JObject;
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddMinutes(5);

                String accesstoken = Convert.ToString(resp["access_token"]);
                string encAccessToken = dataProtectionProvider.CreateProtector("thisissecretkey").Protect(accesstoken);

                HttpContext.Response.Cookies.Append("authToken", encAccessToken);
            }
            else
            {
                response = res.StatusCode.ToString();
                return RedirectToAction("index", "Home");

            }

            return RedirectToAction("Index", "Profile");
        }
    }
}
