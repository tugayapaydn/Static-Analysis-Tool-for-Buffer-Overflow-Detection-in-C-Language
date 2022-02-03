using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public class Variable : LValue
    {
        public string Name { get; set; }
        
        public VarDecl Decl { get; set; }
        
        public Variable(int lineNumber, string name, VarDecl decl)
            :base(lineNumber)
        {
            this.Name = name;
            this.Decl = decl;
        }

        public override string ToString()
        {
            return "Variable " + Name;
        }

        public override dynamic Result()
        {
            if (Decl.IsInit != false)
                return Decl.Value;
            else
                return null;
        }
    }
}