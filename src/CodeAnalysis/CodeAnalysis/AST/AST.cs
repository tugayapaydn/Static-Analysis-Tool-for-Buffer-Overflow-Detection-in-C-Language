using System.Collections.Generic;
using System;
using System.Linq;

namespace CodeAnalysis
{
    public class AST
    {
        public List<FuncDecl> functions = new List<FuncDecl>();

        public void PrintTree()
        {
            foreach (FuncDecl fd in functions)
            {
                Console.WriteLine(fd.ToString());
            }
        }

        //Stack array syntax int arr[5];
        public Node CreateSyntaxTree(string filename)
        {
            Tokenizer tk = new Tokenizer();
            List<List<Token>> tokenList = tk.TokenizeFile(filename);

            if (tokenList == null || tokenList.Count == 0)
                return null;

            Node n;
            
            for (int i = 0; i < tokenList.Count; i++)
            {
                if ((n = GetFuncDecl(tokenList[i], i)) != null)
                {
                    FuncDecl fd = (FuncDecl)n;
                    int startb = i+1;

                    Body b = GetBody(tokenList, ref startb, fd.ArgList, new Body(0));

                    if (b == null)
                        return null;

                    fd.FuncBody = b;
                    functions.Add(fd);
                    i = startb;
                }
            }

            return null;
        }

        // Body start with { in a single line and ends with } in a single line.
        private Body GetBody(List<List<Token>> tokenList, ref int start, VarDecl[] argList, Body outerBody)
        {
            if (tokenList[start][0].Type == TokenType.LCBRACKET)
            {
                Body b = new Body(start);

                foreach (VarDecl vd in argList)
                {
                    b.AddVarDecl(vd);
                }

                foreach (VarDecl vd in outerBody.VarDecls)
                {
                    b.AddVarDecl(vd);
                }

                Node n;
                int i = start + 1;

                for (; i < tokenList.Count && (tokenList[i].Count == 0 || tokenList[i].Count > 0 && tokenList[i][0].Type != TokenType.RCBRACKET); i++)
                {
                    if (tokenList[i].Count > 0)
                    {
                        if ((n = GetVarDecl(tokenList[i], i+1, b)) != null)
                        {
                            VarDecl vd = (VarDecl)n;
                            b.AddStmt(vd);
                            b.AddVarDecl(vd);
                        }
                    
                        else if ((n = GetAssignStmt(tokenList[i], i+1, b)) != null)
                        {
                            AssignStmt astm = (AssignStmt)n;
                            b.AddStmt(astm);
                        }

                        else if ((n = GetIfStmt(tokenList[i], i + 1, b)) != null)
                        {
                            IfStmt st = (IfStmt)n;

                            int curr = i + 1;
                            st.Body = GetBody(tokenList, ref curr, new VarDecl[0], b);
                            i = curr;
                            b.AddStmt(st);
                        }

                        else if ((n = GetWhileStmt(tokenList[i], i + 1, b)) != null)
                        {
                            WhileStmt st = (WhileStmt)n;

                            int curr = i + 1;
                            st.Body = GetBody(tokenList, ref curr, new VarDecl[0], b);
                            i = curr;
                            b.AddStmt(st);
                        }

                        else if ((n = ReadFuncCall(tokenList[i], 0, i+1, b)) != null)
                        {
                            FunctionCall fc = (FunctionCall)n;
                            b.AddStmt(fc);
                        }

                        else
                        {
                            string err = "GetBody: Could not parse token list (at " + i.ToString() + "): ";
                            foreach (Token t in tokenList[i])
                            {
                                err += t.Value + " ";
                            }
                            Console.WriteLine(err);
                        }
                    }
                }
                start = i;
                return b;
            }

            return null;
        }

        // int func_name(args)
        private FuncDecl GetFuncDecl(List<Token> tokenList, int lineNumber)
        {
            if (tokenList.Count > 3 && Tokenizer.IsDataType(tokenList[0].Type) && tokenList[1].Type == TokenType.VARIABLE && tokenList[2].Type == TokenType.LPARANTH)
            {
                VarDecl[] args = ReadFuncArgs(tokenList, 2, lineNumber);
                
                return new FuncDecl(lineNumber, tokenList[0].Type, tokenList[1].Value, args);
            }

            return null;
        }

        private VarDecl GetVarDecl(List<Token> tokenList, int lineNumber, Body b)
        {
            if (tokenList.Count >= 3 && Tokenizer.IsDataType(tokenList[0].Type))
            {
                if (tokenList[1].Type == TokenType.VARIABLE)
                {
                    // int a;
                    if (tokenList[2].Type == TokenType.SEMICOL)
                    {
                        return new VarDecl(lineNumber, tokenList[0].Type, tokenList[1].Value);
                    }

                    else if (tokenList[2].Type == TokenType.ASSIGN)
                    {
                        Expr e = GetExpr(tokenList, lineNumber, 3, b);
                        return new VarDecl(lineNumber, tokenList[0].Type, tokenList[1].Value, e);
                    }

                    // int a[3];
                    else if (tokenList.Count >= 5 && tokenList[2].Type == TokenType.LBRACKET && tokenList[3].Type == TokenType.NUMB && tokenList[4].Type == TokenType.RBRACKET)
                    {
                        int arrlen = int.Parse(tokenList[3].Value);

                        if (tokenList[5].Type == TokenType.ASSIGN)
                        {
                            object[] content = ReadArrayContent(tokenList, 6, tokenList[0].Type, arrlen);

                            return new ArrayDecl(lineNumber, tokenList[0].Type, tokenList[1].Value, arrlen, content, Enumerable.Repeat(true, arrlen).ToArray(), false);
                        }

                        else if (tokenList[5].Type == TokenType.SEMICOL)
                        {
                            return new ArrayDecl(lineNumber, tokenList[0].Type, tokenList[1].Value, arrlen, new object[arrlen], Enumerable.Repeat(false, arrlen).ToArray(), false);
                        }

                    }

                    // int a[]
                    else if (tokenList.Count > 5 && tokenList[2].Type == TokenType.LBRACKET && tokenList[3].Type == TokenType.RBRACKET && tokenList[4].Type == TokenType.ASSIGN)
                    {
                        object[] content = ReadArrayContent(tokenList, 5, tokenList[0].Type);
                        return new ArrayDecl(lineNumber, tokenList[0].Type, tokenList[1].Value, content.Length, content, Enumerable.Repeat(true, content.Length).ToArray(), false);
                    }

                }

                // int * a
                else if (tokenList[1].Type == TokenType.MULT && tokenList[2].Type == TokenType.VARIABLE)
                {
                    // int *a;
                    if (tokenList[2].Type == TokenType.SEMICOL)
                    {
                        return new VarDecl(lineNumber, tokenList[0].Type, tokenList[1].Value, null, false, false, true);
                    }
                }

            }
            // DataType VariableName Ex: int a
            return null;
        }

        private AssignStmt GetAssignStmt(List<Token> tokenList, int lineNumber, Body b)
        {
            if (tokenList.Count >= 3 && tokenList[0].Type == TokenType.VARIABLE && tokenList[1].Type == TokenType.ASSIGN)
            {
                Expr e = GetExpr(tokenList, lineNumber, 2, b);
                VarDecl var = b.GetVarDecl(tokenList[0].Value);
                if (var == null)
                    return null;

                return new AssignStmt(lineNumber, e, new Variable(lineNumber, var.Name, var));
            }
            else if (tokenList.Count >= 3 
                        && tokenList[0].Type == TokenType.VARIABLE 
                        && tokenList[1].Type == TokenType.LBRACKET 
                        && (tokenList[2].Type == TokenType.NUMB || tokenList[2].Type == TokenType.VARIABLE)
                        && tokenList[3].Type == TokenType.RBRACKET
                        && tokenList[4].Type == TokenType.ASSIGN)
            {
                
                Expr ix = GetValue(tokenList[2], tokenList[1], b, lineNumber);
                Expr e = GetExpr(tokenList, lineNumber, 5, b);
                ArrayDecl decl = (ArrayDecl)b.GetVarDecl(tokenList[0].Value);
                
                return new AssignStmt(lineNumber, e, new ArrayDeref(lineNumber, decl.Name, decl, ix));
            }

            return null;
        }

        private IfStmt GetIfStmt(List<Token> tokenList, int lineNumber, Body b)
        {
            if (tokenList.Count == 0 || tokenList[0].Type != TokenType.IF)
                return null;

            if (tokenList[1].Type == TokenType.LPARANTH)
            {
                Expr e = GetExpr(tokenList, lineNumber, 2, b);
                return new IfStmt(lineNumber, e, null);
            }

            return null;
        }

        private WhileStmt GetWhileStmt(List<Token> tokenList, int lineNumber, Body b)
        {
            if (tokenList.Count == 0 || tokenList[0].Value != "while")
                return null;

            if (tokenList[1].Type == TokenType.LPARANTH)
            {
                Expr e = GetExpr(tokenList, lineNumber, 2, b);
                return new WhileStmt(lineNumber, e, null);
            }

            return null;
        }

        private Expr GetExpr(List<Token> tokenList, int lineNumber, int start, Body b)
        {
            Expr l = null, r = null;

            for (int i = start; i < tokenList.Count; i++)
            {
                if (Tokenizer.IsBinaryOp(tokenList[i].Type))
                {
                    if (l == null)
                        l = GetValue(tokenList[i - 1], tokenList[i - 2], b, lineNumber);

                    if (l != null)
                    {
                        r = GetValue(tokenList[i + 1], tokenList[i - 2], b, lineNumber);
                        l = new BinaryOp(lineNumber, tokenList[i].Type, l, r);
                        r = null;
                        i++;
                    }
                    else
                    {
                        return GetValue(tokenList[i + 1], tokenList[i], b, lineNumber);
                    }
                }

                else if (tokenList[i].Type == TokenType.COMMA || tokenList[i].Type == TokenType.SEMICOL || tokenList[i].Type == TokenType.RPARANTH || tokenList[i].Type == TokenType.RBRACKET)
                {
                    if (l == null)
                        l = GetValue(tokenList[i - 1], tokenList[i - 2], b, lineNumber);

                    return l;
                }
            }

            return l;
        }

        private Expr GetValue(Token token, Token prev, Body b, int lineNumber)
        {
            switch (token.Type)
            {
                case TokenType.NUMB:
                    if (prev.Type == TokenType.MINUS)
                        return new IntConst(lineNumber, int.Parse(token.Value.Insert(0, "-")));
                    else
                        return new IntConst(lineNumber, int.Parse(token.Value));
                case TokenType.VARIABLE:
                    VarDecl vd = b.GetVarDecl(token.Value);
                    return new Variable(lineNumber, vd.Name, vd);
            }

            return null;
        }

        private VarDecl[] ReadFuncArgs(List<Token> tokenList, int start, int lineNumber)
        {
            if (tokenList[start].Type != TokenType.LPARANTH)
                return null;

            List<VarDecl> contents = new List<VarDecl>();

            for (int i = start+1; i < tokenList.Count ; i += 2)
            {
                if (tokenList[i].Type == TokenType.RPARANTH)
                    return contents.ToArray();

                else if (i+2 >= tokenList.Count || !Tokenizer.IsDataType(tokenList[i].Type) || tokenList[i+1].Type != TokenType.VARIABLE)
                    return null;

                else
                    contents.Add(new VarDecl(lineNumber, tokenList[i].Type, tokenList[i + 1].Value)); // DataType, variable name

                
                if (tokenList[i+2].Type != TokenType.RPARANTH && tokenList[i+2].Type != TokenType.COMMA)
                    return null;
             
                else if (tokenList[i+2].Type == TokenType.COMMA)
                    i++;
            }

            return null;
        }

        private object[] ReadArrayContent(List<Token> tokenList, int start, TokenType dataType, int len = 0)
        {
            if (tokenList[start].Type == TokenType.LCBRACKET)
                return ReadArrayContentNumb(tokenList, start, dataType, len);

            else if (tokenList[start].Type == TokenType.STRING)
                return ReadArrayContentStr(tokenList, start, dataType, len);

            else
                return new List<object>().ToArray();
        }

        private object[] ReadArrayContentNumb(List<Token> tokenList, int start, TokenType dataType, int len = 0)
        {
            List<object> contents = new List<object>();

            for (int i = start + 1; tokenList[i].Type != TokenType.RCBRACKET; i++)
            {
                if (tokenList[i].Type == TokenType.NUMB)
                {
                    dynamic v = ParseString(tokenList[i].Value, dataType);
                    contents.Add(v);
                }
            }

            if (len > 0)
            {
                for (int i = len; i > contents.Count; i--)
                {
                    contents.Add(0);
                }
            }

            return contents.ToArray();
        }

        private object[] ReadArrayContentStr(List<Token> tokenList, int start, TokenType dataType, int len = 0)
        {
            List<object> contents = new List<object>();

            string str = tokenList[start].Value;
            for (int i = 1; i < str.Length-1; i++)
            {
                contents.Add(str[i]);
            }

            return contents.ToArray();
        }

        private FunctionCall ReadFuncCall(List<Token> tokenList, int start, int lineNumber, Body b)
        {
            if (tokenList[start].Type != TokenType.VARIABLE && tokenList[start + 1].Type != TokenType.LPARANTH)
                return null;

            Expr[] e = ReadFuncCallArgs(tokenList, start + 2, lineNumber, b);

            return new FunctionCall(lineNumber, tokenList[start].Value, e, null);
        }
        
        private Expr[] ReadFuncCallArgs(List<Token> tokenList, int start, int lineNumber, Body b)
        {
            List<Expr> exprl = new List<Expr>();

            for (int i = start; i < tokenList.Count && tokenList[i].Type != TokenType.RPARANTH; i++)
            {
                Expr e = GetValue(tokenList[i], tokenList[i - 1], b, lineNumber);
                
                if (e != null)
                    exprl.Add(e);
            }
            
            return exprl.ToArray();
        }

        private dynamic ParseString(string s, TokenType t)
        {
            switch(t)
            {
                case TokenType.INT:
                    return int.Parse(s);
                case TokenType.LONG:
                    return long.Parse(s);
                case TokenType.DOUBLE:
                    return double.Parse(s);
                default:
                    return null;
            }
        }
    }
}
