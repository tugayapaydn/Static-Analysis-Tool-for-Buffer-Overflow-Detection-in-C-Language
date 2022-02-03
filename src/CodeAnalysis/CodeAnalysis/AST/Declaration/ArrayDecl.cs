using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace CodeAnalysis
{
    public class ArrayDecl : VarDecl
    {
        public int Len { get; }
        public dynamic[] Content { get; }
        public bool[] InitList { get; }

        public ArrayDecl(int lineNumber, TokenType retType, string name, int len, dynamic[] content, bool[] initList, bool isPointer)
            : base(lineNumber, retType, name, null, false, true, isPointer)
        {
            this.IsArray = true;
            this.Len = len;
            this.Content = content;
            this.InitList = initList;
        }

        public override string ToString()
        {
            string ret = base.ToString() + ", Len: " + Len.ToString();

            ret += ", Content: ";
            for (int i = 0; i < Content.Length; i++)
            {
                if (i > 0)
                    ret += ", ";

                ret += Convert.ToString(Content[i]);
            }

            return ret;
        }
    }
}

