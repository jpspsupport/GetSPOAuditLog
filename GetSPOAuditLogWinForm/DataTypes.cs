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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetSPOAuditLogWinForm
{
    class DataTypes
    {
    }

    public class Organization
    {
        public string id { get; set; }
    }

    public class Organizations
    {
        public List<Organization> value;
    }

    public class Subscription
    {
        public string ContentType { get; set; }
        public string Status { get; set; }
       
    }

    public class Content
    {
        public DateTime ContentCreated { get; set; }
        public DateTime ContentExpiration { get; set; }
        public string ContentId { get; set; }
        public string ContentType { get; set; }
        public string ContentUri { get; set; }

    }

    public class Audit
    {
        public string ClientIP { get; set; }
        public string CorrelationId { get; set; }
        public DateTime CreationTime { get; set; }
        public string EventData { get; set; }
        public string EventSource { get; set; }
        public string Id { get; set; }
        public string ItemType { get; set; }
        public string ObjectId { get; set; }
        public string Operation { get; set; }
        public string OrganizationId { get; set; }
        public string RecordType { get; set; }
        public string UserAgent { get; set; }
        public string UserId { get; set; }
        public string UserKey { get; set; }
        public string UserType { get; set; }
        public string Version { get; set; }
        public string Workload { get; set; }

    }

}
