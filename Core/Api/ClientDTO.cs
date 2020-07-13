using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Api
{
   public class ClientDTO:BaseEntity
    {
        public string ClientId { get; set; }
        
        public string ClientName { get; set; }
        public string IDNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
      //  public int? GenderId { get; set; }
        public string  GenderName { get; set; }
        public string IBANNumber { get; set; }
        public string  BankName { get; set; }

    }
    public class MRClient : BaseEntity
    {
        public string ClientName { get; set; }
        public string IDNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        //  public int? GenderId { get; set; }
        public int? GenderId { get; set; }
        public string IBANNumber { get; set; }
        public string BankName { get; set; }

    }
}
