using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Domain
{
    public class Registration : BaseEntity
    {      
        public string DOB { get; set; }
        public string MemberName { get; set; }
        public string MemberMobileNumber { get; set; }
        public string MemberStatus { get; set; }
        public string MemberType { get; set; }
        public string TPAID { get; set; }
        public string TPAName { get; set; }
        public string PolicyNo { get; set; }
        public DateTime? PolicyFromDate { get; set; }
        public DateTime? PolicyToDate { get; set; }
        public string TushfaMemberNo { get; set; }
        public string CardNo { get; set; }
        public string PhoneNoVerification { get; set; }
        public string CreatePin { get; set; }
        public string ConfirmPin { get; set; }
        public DateTime? LoginDateandTime { get; set; }
        public string Iqama_NationalID { get; set; }

        [NotMapped]
        public List<object> Errors { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public string NetworkCode { get; set; }
        public string NetworkName { get; set; }
    }
}
