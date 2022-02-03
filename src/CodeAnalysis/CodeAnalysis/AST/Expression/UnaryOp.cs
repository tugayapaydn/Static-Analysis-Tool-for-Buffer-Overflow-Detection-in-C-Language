using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public class UnaryOp : Expr
    {
        public TokenType Opr { get; }
        public Expr E1 { get; }

        public UnaryOp(int lineNumber, TokenType opr, Expr e1)
            :base(lineNumber)
        {
            this.Opr = opr;
            this.E1 = e1;
        }

        public override dynamic Result()
        {
            dynamic l = E1.Result();

            if (l == null)
                return null;

            switch (Opr)
            {
                case TokenType.INCR:
                    return l + 1;
                case TokenType.DECR:
                    return l - 1;
                default:
                    return null;
            }
        }
    }
}
