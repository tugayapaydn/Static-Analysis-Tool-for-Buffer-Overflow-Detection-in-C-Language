using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public class Body : Node
    {
        public List<VarDecl> VarDecls {get; }
        public List<Stmt> Stmts { get; }

        public Body (int lineNumber)
            : base(lineNumber)
        {
            this.Stmts = new List<Stmt>();
            this.VarDecls = new List<VarDecl>();
        }

        public void AddStmt(Stmt s)
        {
            Stmts.Add(s);
        }

        public void AddVarDecl(VarDecl vd)
        {
            VarDecls.Add(vd);
        }

        public VarDecl GetVarDecl(string name)
        {
            foreach (VarDecl vd in VarDecls)
            {
                if (vd.Name == name)
                    return vd;
            }

            return null;
        }

        public override string ToString()
        {
            string ret = "";
            foreach (Stmt s in Stmts)
            {
                ret += s.ToString() + '\n';
            }
            return ret;
        }
    }
}
