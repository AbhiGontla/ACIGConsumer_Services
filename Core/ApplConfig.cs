using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core
{
    public class ApplConfig
    {
        public Url Urls { get; set; }
        public BasicAuth BasicAuth { get; set; }
        public SmsConfig SmsConfig { get; set; }
    }
    
    public class Url
    {
        public string GetApprovals { get; set; }
        public string GetCoverageBalance { get; set; }
        public string GetProvidersList { get; set; }
        public string GetPaidClaims { get; set; }
        public string GetOSClaims { get; set; }
        public string GetReimbursmentClaims { get; set; }
        public string GetReimbursmentDetails { get; set; }
        public string AddReimbursmentClaims { get; set; }
        public string UpdateClaimRequest { get; set; }
        public string RegistrationRequest { get; set; }
        public string PoliciesRequest { get; set; }
        public string TOBRequest { get; set; }
        


    }
    public class BasicAuth
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string T_Username { get; set; }
        public string T_Password { get; set; }       
    }
    public class SmsConfig
    {
        public string url { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string senderName { get; set; }

    }
}
