using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public class IfStmt : Stmt
    {
        public Expr E { get; }

        public Body Body { get; set; }

        public IfStmt(int lineNumber, Expr e, Body body)
            : base(lineNumber)
        {
            this.E = e;
            this.Body = body;
        }

        public override string ToString()
        {
            return base.ToString() + " If Stmt: " + E.ToString() + " Body: " + Body.ToString();
        }
    }
}
