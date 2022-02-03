using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public class IntConst : Expr
    {
        public int Val { get; set; }

        public TokenType Type {get;}
            
        public IntConst(int lineNumber, int val)
            : base(lineNumber)
        {
            this.Type = TokenType.INT;
            this.Val = val;
        }

        public override string ToString()
        {
            return "Int Constant: " + Val.ToString();
        }

        public override dynamic Result()
        {
            return Val;
        }
    }
}
