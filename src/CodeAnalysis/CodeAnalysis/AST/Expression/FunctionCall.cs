using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public class FunctionCall : Expr 
    {
        public string Name { get; }
        public FuncDecl Decl { get; }
        public Expr[] ArgList { get; }

        public FunctionCall(int lineNumber, string name, Expr[] argList, FuncDecl funcDecl)
            : base(lineNumber)
        {
            this.Name = name;
            this.ArgList = argList;
            this.Decl = funcDecl;
        }

        public override dynamic Result()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            string ret = base.ToString() + "Function Call: '" + Name + "' -> " + " (";

            foreach (Expr e in ArgList)
            {
                ret += e.ToString() + "  ";
            }
            ret += ")\n";

            return ret;
        }
    }
}
