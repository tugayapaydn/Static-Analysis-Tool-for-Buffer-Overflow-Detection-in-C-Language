using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public abstract class Stmt : Node
    {
        public Stmt next { get; set;}

        public Stmt (int lineNumber)
            :base (lineNumber)
        {

        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
