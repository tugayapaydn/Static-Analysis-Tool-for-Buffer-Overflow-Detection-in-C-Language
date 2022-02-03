using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public class ArrayDeref : LValue
    {
        public string Name { get; set; }
        public ArrayDecl Decl { get; }
        public Expr Expr { get; }

        public ArrayDeref(int lineNumber, string name, ArrayDecl decl, Expr expr)
            : base(lineNumber)
        {
            this.Name = name;
            this.Decl = decl;
            this.Expr = expr;
        }

        public override string ToString()
        {
            return "ArrayDeref: " + Name + "[" + Expr.ToString() + "]";
        }

        public override dynamic Result()
        {
            var res = Expr.Result();

            if (res < 0 || res >= Decl.Len)
                throw new IndexOutOfRangeException();

            if (Decl.InitList[res] == false)
                throw new ArgumentNullException();
            else
                return Decl.Content[res];
        }
    }
}
