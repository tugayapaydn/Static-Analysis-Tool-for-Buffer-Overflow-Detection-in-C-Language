using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace CodeAnalysis
{
    public class FuncDecl : Node
    {
        public string Name { get; }
        public TokenType RetType { get; }
        public Body FuncBody { get; set;}
        public VarDecl[] ArgList { get; }

        public FuncDecl(int lineNumber, TokenType retType, string name, VarDecl[] argList, Body funcBody = null)
            :base (lineNumber)
        {
            this.Name = name;
            this.RetType = retType;
            this.ArgList = argList;
            this.FuncBody = funcBody;
        }

        public override string ToString()
        {
            string ret = base.ToString() + "Function Declaration: '" + Name + "' -> " + RetType.ToString() + " (";

            foreach (VarDecl vd in ArgList)
            {
                ret += vd.ToString() + "  ";
            }
            ret += ")\n";

            return ret + FuncBody.ToString();
        }
    }

    
}
