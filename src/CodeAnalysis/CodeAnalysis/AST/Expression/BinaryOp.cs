using System;

namespace CodeAnalysis
{
    public class BinaryOp : Expr
    {
        public TokenType Opr { get; }
        
        public Expr El { get; }
        
        public Expr Er { get; }

        public BinaryOp (int lineNumber, TokenType opr, Expr el, Expr er)
            : base(lineNumber)
        {
            this.Opr = opr;
            this.El = el;
            this.Er = er;
        }

        public override dynamic Result()
        {
            dynamic l = El.Result();
            dynamic r = Er.Result();

            if (l == null || r == null)
                throw new ArgumentNullException();
            
            switch (Opr)
            {
                case TokenType.PLUS:
                    if (OverflowBeast.ArithOverflowTest(El.Result(), Er.Result(), TokenType.INT, TokenType.PLUS))
                        throw new OverflowException();
                    
                    return El.Result() + Er.Result();
                case TokenType.MINUS:
                    if (OverflowBeast.ArithOverflowTest(El.Result(), Er.Result(), TokenType.INT, TokenType.MINUS))
                        throw new OverflowException();
                    
                    return El.Result() - Er.Result();
                case TokenType.MULT:
                    if (OverflowBeast.ArithOverflowTest(El.Result(), Er.Result(), TokenType.INT, TokenType.MULT))
                        throw new OverflowException();

                    return El.Result() * Er.Result();
                case TokenType.DIV:
                    if (OverflowBeast.ArithOverflowTest(El.Result(), Er.Result(), TokenType.INT, TokenType.DIV))
                        throw new OverflowException();

                    return El.Result() / Er.Result();

                case TokenType.GT:
                    return Convert.ToInt32(El.Result() > Er.Result());
                case TokenType.GTE:
                    return Convert.ToInt32(El.Result() >= Er.Result());
                case TokenType.LT:
                    return Convert.ToInt32(El.Result() < Er.Result());
                case TokenType.LTE:
                    return Convert.ToInt32(El.Result() <= Er.Result());
                case TokenType.EQL:
                    return Convert.ToInt32(El.Result() == Er.Result());

                default:
                    throw new ArgumentNullException();
            }
        }

        public override string ToString()
        {
            return base.ToString() + " Binary Op: " + El.ToString() + " " + Opr.ToString() + " " + Er.ToString();
        }
    }
}