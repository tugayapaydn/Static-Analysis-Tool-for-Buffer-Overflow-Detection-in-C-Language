using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public class VarDecl : Stmt
    {
        public TokenType DataType { get; set; }
       
        public string Name {  get; set; }
        
        public Expr VarExpr { get; set; }
        
        public bool IsArray { get; set; }
       
        public bool IsPointer { get; set; }

        public bool IsInit { get; set; }

        public dynamic Value { get; set; }

        public VarDecl(int lineNumber, TokenType dataType, string name, Expr VarExpr = null, bool isInit = false, bool isArray = false, bool isPointer = false)
            :base (lineNumber)
        {
            this.Name = name;
            this.DataType = dataType;
            this.VarExpr = VarExpr;
            this.IsArray = isArray;
            this.IsPointer = isPointer;
            this.IsInit = isInit;
            this.Value = null;
        }

        public override string ToString()
        {
            if (VarExpr != null)
                return base.ToString() + "Variable Declaration: '" + Name + "' -> " + DataType.ToString() + " (Pointer= " + IsPointer.ToString() + ", Array= " + IsArray.ToString() + ")" + " - Expr: " + VarExpr.ToString();
            
            else
                return base.ToString() + "Variable Declaration: '" + Name + "' -> " + DataType.ToString() + " (Pointer= " + IsPointer.ToString() + ", Array= " + IsArray.ToString() + ")" + " - Expr: Null";
        }
    }
}
