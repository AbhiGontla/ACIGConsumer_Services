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

    public class UpdateClaimRequest
    {
        public int RequestId { get; set; }
        public string Comment { get; set; }
        public List<RequestFileDTO> RequestFileList { get; set; }
    }
    public class RequestFile
    {
        public int FileId { get; set; }

        //  public string RequestNumber { get; set; }
        //  public string IDNumber { get; set; }
        public string FilePath { get; set; }
        public string MyFile { get; set; }
        public string FileName { get; set; }
        public string FileDesc { get; set; }
        public string Comment { get; set; }
    }
}
