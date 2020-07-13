using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Api
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 


    public class ReImClaims
    {
        public int RequestId { get; set; }
        public string RequestNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestStatusName { get; set; }
        public string PolicyNumber { get; set; }
        public string ClaimTypeName { get; set; }
        public double ExpectedAmount { get; set; }
        public object ActualAmount { get; set; }
        public double VATAmount { get; set; }
        public object CreateDate { get; set; }
        public ClientDTO ClientDTO { get; set; }

    }

    public class ClaimsResponse
    {
        public List<ReImClaims> MyArray { get; set; }

    }

}
