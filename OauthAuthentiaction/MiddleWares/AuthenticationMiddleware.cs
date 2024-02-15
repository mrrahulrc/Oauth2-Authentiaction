using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OauthAuthentiaction.Constants;
using System;
using Microsoft.AspNetCore.DataProtection;

namespace OauthAuthentiaction.MiddleWares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public AuthenticationMiddleware(RequestDelegate requestDelegate, IDataProtectionProvider dataProtectionProvider)
        {
            _next = requestDelegate;
            _dataProtectionProvider = dataProtectionProvider;
        }

        public async Task InvokeAsync(HttpContext context, IHttpClientFactory httpClientFactory) 
        {
            try
            {
                String path = context.Request.Path.ToString().ToLower();
                if (UnProtectedRoutes.routes.Any(ele => ele.Equals(path)))
                {
                    await _next(context);
                }
                else if (context.Request.Cookies.TryGetValue("authToken", out string encToken))
                {
                    String token = _dataProtectionProvider.CreateProtector("thisissecretkey").Unprotect(encToken);
                    String url = String.Format(OAuthUrl.userInforUrl, token);

                    using (HttpClient client = httpClientFactory.CreateClient())
                    {
                        HttpResponseMessage res = await client.GetAsync(url);

                        if (res.IsSuccessStatusCode)
                        {
                            JObject resp = JsonConvert.DeserializeObject(await res.Content.ReadAsStringAsync()) as JObject;

                            context.Items["email"] = resp["email"];

                            await _next(context);
                        }
                        else
                        {
                            redirectToLoginPage(context);
                            return;
                        }
                    }
                }
                else
                {
                    redirectToLoginPage(context);
                }
            }
            catch (Exception e)
            {
                redirectToErrorPage(context);
            }
            
        }

        public void redirectToLoginPage( HttpContext context)
        {
            context.Response.Redirect("/Home/Index");
            return;
        }

        public void redirectToErrorPage(HttpContext context)
        {
            context.Response.Redirect("/error/error");
            return;
        }
    }
}
