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
}
