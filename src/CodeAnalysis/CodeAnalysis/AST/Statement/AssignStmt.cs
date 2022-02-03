using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public class AssignStmt : Stmt
    {
        public LValue Val { get; }
        public Expr E { get; }

        public AssignStmt (int lineNumber, Expr e, LValue val)
            :base (lineNumber)
        {
            this.E = e;
            this.Val = val;
        }

        public override string ToString()
        {
            return base.ToString() + " Assignment Stmt: " + Val.ToString() + " = " + E.ToString();
        }
    }
}
