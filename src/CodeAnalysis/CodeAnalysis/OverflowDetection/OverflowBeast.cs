using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public class OverflowBeast
    {
        public AST Ast { get; }
        public OverflowBeast(AST ast)
        {
            this.Ast = ast;
        }

        public List<ListViewObject> ValueFlowAnalysis()
        {
            FuncDecl fd = FindMain();
            List<ListViewObject> err = new List<ListViewObject>();
            AnalyseBody(fd.FuncBody, ref err);
            
            return err;
        }

        /*
         * Finds main method in the ast
         */
        public FuncDecl FindMain()
        {
            foreach (FuncDecl fd in Ast.functions)
            {
                if (fd.Name == "main")
                    return fd;
            }

            return null;
        }

        private void AnalyseBody(Body b, ref List<ListViewObject> err)
        {
            
            foreach (Stmt s in b.Stmts)
            {

                if (s.GetType() == typeof(VarDecl))
                {
                    VarDecl vd = (VarDecl)s;

                    try
                    {
                        if (vd.VarExpr != null)
                            vd.Value = vd.VarExpr.Result();

                        if (vd.Value == null)
                            err.Add(new ListViewObject(vd.lineNumber, "Uninitialized or Overflowed Value"));
                        else
                        {
                            vd.IsInit = true;
                            //err.Add(new ListViewObject(vd.lineNumber, Convert.ToString(vd.Value)));
                        }
                    }
                    catch (ArgumentNullException e)
                    {
                        err.Add(new ListViewObject(vd.lineNumber, "Uninitialized or Overflowed Value"));
                    }
                    catch (OverflowException e)
                    {
                        err.Add(new ListViewObject(vd.lineNumber, "Integer Overflow"));
                    }

                }

                else if (s.GetType() == typeof(FunctionCall))
                {
                    FunctionCall st = (FunctionCall)s;
                    if (st.Name == "strcpy") //Args: dest, source
                    {
                        
                        ArrayDecl e1 = (ArrayDecl)(((Variable)st.ArgList[0]).Decl);
                        ArrayDecl e2 = (ArrayDecl)(((Variable)st.ArgList[1]).Decl);

                        if (e1.Len <= e2.Content.Length)
                        {
                            err.Add(new ListViewObject(st.lineNumber, "Destination Array Overflow"));
                        }
                    }
                    else if (st.Name == "strcat")
                    {
                        ArrayDecl e1 = (ArrayDecl)(((Variable)st.ArgList[0]).Decl);
                        ArrayDecl e2 = (ArrayDecl)(((Variable)st.ArgList[1]).Decl);

                        if (e1.Len <= e1.Content.Length + e2.Content.Length)
                        {
                            err.Add(new ListViewObject(st.lineNumber, "Destination Array Overflow"));
                        }
                    }
                }

                else if (s.GetType() == typeof(AssignStmt))
                {
                    AssignStmt st = (AssignStmt)s;
                    if (st.Val.GetType() == typeof(Variable))
                    {

                        VarDecl vd = ((Variable)st.Val).Decl;
                    
                        try
                        {
                            vd.Value = st.E.Result();
                        
                            if (vd.Value == null)
                                err.Add(new ListViewObject(st.lineNumber, "Uninitialized or Overflowed Value"));
                            else
                            {
                                vd.IsInit = true;
                                //err.Add(new ListViewObject(st.lineNumber, Convert.ToString(vd.Value)));
                            }
                        }
                        catch (ArgumentNullException e)
                        {
                            err.Add(new ListViewObject(st.lineNumber, "Uninitialized or Overflowed Value"));
                        }
                        catch (OverflowException e)
                        {
                            err.Add(new ListViewObject(st.lineNumber, "Integer Overflow"));
                        }
                    }
                    else if (st.Val.GetType() == typeof(ArrayDeref))
                    {
                        ArrayDeref adf = (ArrayDeref)st.Val;
                        ArrayDecl vd = adf.Decl;

                        try
                        {
                            var res = adf.Result();
                            var ix = adf.Expr.Result();

                            vd.Content[ix] = st.E.Result();
                            vd.InitList[ix] = true;
                        }
                        catch (ArgumentNullException e)
                        {
                            err.Add(new ListViewObject(st.lineNumber, "Uninitialized or Overflowed Value"));
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            err.Add(new ListViewObject(st.lineNumber, "Index Out Of Bounds"));
                        }
                        catch (OverflowException e)
                        {
                            err.Add(new ListViewObject(st.lineNumber, "Integer Overflow"));
                        }
                    }
                }

                else if (s.GetType() == typeof(IfStmt))
                {
                    IfStmt st = (IfStmt)s;

                    try
                    {
                        var res = st.E.Result();
                        //err.Add(new ListViewObject(st.lineNumber, Convert.ToString(res)));
                        if (res != 0)
                            AnalyseBody(st.Body, ref err);
                    }
                    catch (ArgumentNullException e)
                    {
                        err.Add(new ListViewObject(st.lineNumber, "Uninitialized or Overflowed Value"));
                    }
                    catch (OverflowException e)
                    {
                        err.Add(new ListViewObject(st.lineNumber, "Integer Overflow"));
                    }
                }

                else if (s.GetType() == typeof(WhileStmt))
                {
                    WhileStmt st = (WhileStmt)s;

                    try
                    {
                        while (st.E.Result() == 1)
                        {
                            AnalyseBody(st.Body, ref err);
                        }
                        var res = st.E.Result();
                        //err.Add(new ListViewObject(st.lineNumber, Convert.ToString(res)));
                    }
                    catch (ArgumentNullException e)
                    {
                        err.Add(new ListViewObject(st.lineNumber, "Uninitialized or Overflowed Value"));
                    }
                    catch (OverflowException e)
                    {
                        err.Add(new ListViewObject(st.lineNumber, "Integer Overflow"));
                    }
                }
            }
        }

        public static bool ArithOverflowTest(dynamic a, dynamic b, TokenType dataType, TokenType opr)
        {
            if (dataType != TokenType.INT)
                throw new NotImplementedException();

            switch(opr)
            {
                case TokenType.PLUS:
                    if (((b > 0) && (a > int.MaxValue - b)) || ((b < 0) && (a < int.MinValue - b)))
                        return true;
                    else
                        return false;
                case TokenType.MINUS:
                    if (((b < 0) && (a > int.MaxValue + b)) || ((b > 0) && (a < int.MinValue + b)))
                        return true;
                    else
                        return false;

                case TokenType.MULT:
                    // Check for two's complement
                    if (((a == -1) && (b == int.MinValue)) || ((b == -1) && (a == int.MinValue))
                            || (a > 0 && b > 0 && ((a > int.MaxValue / b) || (a < int.MinValue / b)))
                            || (a < 0 && b < 0 && ((a > int.MaxValue / b) || (a < int.MinValue / b))))
                        return true;
                    else
                        return false;
                case TokenType.DIV:
                    if ((b == -1) && (a == int.MinValue))
                        return true;
                    else
                        return false;
            }

            return false;
        }
    }
}
