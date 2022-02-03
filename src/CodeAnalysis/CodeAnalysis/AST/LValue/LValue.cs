using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public class LValue : Expr
    {
        public LValue (int lineNumber)
            :base(lineNumber)
        {

        }

        public override dynamic Result()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
