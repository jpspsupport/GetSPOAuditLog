/*
 This Sample Code is provided for the purpose of illustration only and is not intended to be used in a production environment. 
 THIS SAMPLE CODE AND ANY RELATED INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, 
 INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.  

 We grant you a nonexclusive, royalty-free right to use and modify the sample code and to reproduce and distribute the object 
 code form of the Sample Code, provided that you agree: 
    (i)   to not use our name, logo, or trademarks to market your software product in which the sample code is embedded; 
    (ii)  to include a valid copyright notice on your software product in which the sample code is embedded; and 
    (iii) to indemnify, hold harmless, and defend us and our suppliers from and against any claims or lawsuits, including 
          attorneys' fees, that arise or result from the use or distribution of the sample code.

Please note: None of the conditions outlined in the disclaimer above will supercede the terms and conditions contained within 
             the Premier Customer Services Description.
 */

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GetSPOAuditLogWinForm
{
    public static class O365APIHelper
    {

        const string clientid = "INPUT_YOUR_CLIENT_ID";
        const string redirecturi = "INPUT_YOUR_REDIRECCT_URL";

        // Use the following, if you do not want to use SSO account.
        //const string loginname = "admin@tenant.onmicrosoft.com";

        private static object lockForAccessTokenHashtable = new object();
        private static Hashtable AccessTokenCache = new Hashtable();

        private static async Task<string> GetAccessToken(string resource, string clientid, string redirecturi)
        {
            lock (lockForAccessTokenHashtable)
            {
                if (AccessTokenCache.ContainsKey(resource))
                {
                    return AccessTokenCache[resource].ToString();
                }
            }

            AuthenticationContext authenticationContext = new AuthenticationContext("https://login.microsoftonline.com/common");
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync(
                resource,
                clientid,
                new Uri(redirecturi),
                new PlatformParameters(PromptBehavior.Auto)
            // Use the following, if you do not want to use SSO account.
            //, new UserIdentifier(loginname, UserIdentifierType.RequiredDisplayableId)
            );
            AccessTokenCache.Add(resource, authenticationResult.AccessToken);
            return authenticationResult.AccessToken;
            
        }



        private static HttpResponseMessage HttpSend(HttpMethod method, string apiUrl, string Content = null)
        {

            HttpResponseMessage response = null;

            Task.Run(async () =>
            {
                string resource = (new Uri(apiUrl)).Scheme + "://" + (new Uri(apiUrl)).Host;
                string AccessToken = await GetAccessToken(resource, clientid, redirecturi);

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                    HttpRequestMessage request = new HttpRequestMessage(
                        method,
                        new Uri(apiUrl)
                    );
                    if (Content != null)
                    {
                        request.Content = new StringContent(Content);
                    }

                    response = await httpClient.SendAsync(request);
                }
            }).Wait();
            Debug.WriteLine(string.Format("{0}\t{1}\t{2}", method, apiUrl, response.StatusCode.ToString()));

            return response;

        }
        
        public static HttpResponseMessage Get(string apiUrl)
        {
            return HttpSend(HttpMethod.Get, apiUrl);
        }

        public static HttpResponseMessage Post(string apiUrl)
        {
            return HttpSend(HttpMethod.Post, apiUrl);
        }


    }
}
