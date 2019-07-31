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
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GetSPOAuditLogWinForm
{
    public static class SPOAuditHelper
    {
        const string GetOrganizationAPIUrl = "https://graph.microsoft.com/v1.0/organization/";
        const string SubscriptionAPIUrl = "https://manage.office.com/api/v1.0/{0}/activity/feed/subscriptions/{1}?contentType={2}";
        const string SPOAlertContentType = "Audit.SharePoint";

        const string StartMethod = "start";
        const string ListMethod = "list";
        const string ContentMethod = "content";

        const string EnabledString = "enabled";



        public static string OrganizationId = "";
        private static string GetOriganizationId()
        {
            var response = O365APIHelper.Get(GetOrganizationAPIUrl);
            Organizations orgs = JsonConvert.DeserializeObject<Organizations>(response.Content.ReadAsStringAsync().Result);
            return orgs.value[0].id;
        }

        private static bool GetSubscriptionExists()
        {
            var response = O365APIHelper.Get(string.Format(SubscriptionAPIUrl, OrganizationId, ListMethod, SPOAlertContentType));
            List<Subscription> subs = JsonConvert.DeserializeObject<List<Subscription>>(response.Content.ReadAsStringAsync().Result);
            foreach (var sub in subs)
            {
                if ((sub.ContentType == SPOAlertContentType) && (sub.Status.ToLower() == EnabledString))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool ProvSubscriptionIfNecessary()
        {
            if (!GetSubscriptionExists())
            {
                var response = O365APIHelper.Post(string.Format(SubscriptionAPIUrl, OrganizationId, StartMethod, SPOAlertContentType));
                return true;
            }
            return false;
        }

        public static bool Init()
        {
            OrganizationId = GetOriganizationId();
            return ProvSubscriptionIfNecessary();
        }

        public static List<Audit> GetAuditData()
        {
            List<Audit> retaudits = new List<Audit>();

            var response = O365APIHelper.Get(string.Format(SubscriptionAPIUrl, OrganizationId, ContentMethod, SPOAlertContentType));
            List<Content> contents = JsonConvert.DeserializeObject<List<Content>>(response.Content.ReadAsStringAsync().Result);
            foreach (var content in contents)
            {
                var response2 = O365APIHelper.Get(content.ContentUri);
                List<Audit> audits = JsonConvert.DeserializeObject<List<Audit>>(response2.Content.ReadAsStringAsync().Result);
                retaudits.AddRange(audits);
            }
            return retaudits;

        }


    }
}
