using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public abstract class Expr : Stmt
    {
        public Expr(int lineNumber)
            : base(lineNumber)
        {
        }

        public abstract dynamic Result();

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
