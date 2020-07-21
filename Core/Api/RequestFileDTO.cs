using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Api
{
    public class RequestFileDTO : BaseEntity
    {
        public int FileId { get; set; }

        //  public string RequestNumber { get; set; }
        //  public string IDNumber { get; set; }
        public string FilePath { get; set; }
        public Byte[] MyFile { get; set; }
        public string FileName { get; set; }
        public string FileDesc { get; set; }
        public string Comment { get; set; }
    }

    public class MRRequestStatusLog
    {
        public int StatusLogId { get; set; }
        public int RequestId { get; set; }
        public int RequestStatusId { get; set; }
        public string Comment { get; set; }
        public int ClientId { get; set; }
        public int EntryEmpId { get; set; }
        public DateTime EntryDate { get; set; }
    }
    public class MRRequestFile
    {
        public int FileId { get; set; }
        public int RequestId { get; set; }
        public string FileDesc { get; set; }
        public string FilePath { get; set; }
        public int ClientId { get; set; }
        public int EntryEmpId { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsClientVisible { get; set; }
        public bool IsActive { get; set; }
        public bool IsBordereau { get; set; }
    }
}
