using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public class ListViewObject
    {
        public int Line { get; }
        public string Detail { get; }
        public string File { get; set; }
        
        public ListViewObject(int line, string detail, string file = null)
        {
            this.Line = line;
            this.Detail = detail;
            this.File = file;
        }

        public bool MyEquals(ListViewObject obj)
        {
            return (obj.Line == this.Line && obj.Detail == this.Detail && obj.File == this.File);
        }
    }
}
