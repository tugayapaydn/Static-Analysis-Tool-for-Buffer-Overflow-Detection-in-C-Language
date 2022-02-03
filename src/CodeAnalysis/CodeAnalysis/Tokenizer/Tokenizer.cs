using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace CodeAnalysis
{
    public class Tokenizer
    {
        public static TokenType[] dataTypes = new TokenType[]{TokenType.INT, TokenType.DOUBLE, TokenType.LONG, TokenType.CHAR, TokenType.VOID};
        public static TokenType[] binaryOpTypes = new TokenType[] { TokenType.GT, TokenType.GTE, TokenType.EQL, TokenType.LT, TokenType.LTE,
                                                                    TokenType.PLUS, TokenType.MINUS, TokenType.MULT, TokenType.DIV, TokenType.MODULUS};

        public static string[] oprt = { "+", "-", "*", "/", "%", ">", ">=", "<", "<=", "!", "!=", "==", "=" };
        public static char[] delimiter = { ' ', '+', '-', '*', '/', '%', '!', ',', ';', '>', '<', '=', '(', ')', '[', ']', '{', '}' , '"'};
        public static string[] dataType = { "int", "char", "float", "double", "long" };
        
        public List<List<Token>> TokenizeFile(string filename)
        {
            List<List<Token>> tokenizedFile = new List<List<Token>>();
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        tokenizedFile.Add(Tokenize(line));
                    }
                }
                return tokenizedFile;
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not find the file '" + filename + "'.");
                Console.Write(e.ToString());
                return null;
            }
        }

        /**
         * It is assumed that each there are spaces between each tokens.
         */
        private List<Token> Tokenize(string line)
        {
            List<string> wordList = Parse(line);
            List<Token> tokenList = new List<Token>();

            for (int i = 0; i < wordList.Count; i++) {
                string word = wordList[i];
                string nextWord = i + 1 < wordList.Count ? wordList[i + 1] : "";

                switch (word)
                {
                    case "if":
                        tokenList.Add(new Token(TokenType.IF, word));
                        break;
                    case "else":
                        tokenList.Add(new Token(TokenType.ELSE, word));
                        break;
                    case "void":
                        tokenList.Add(new Token(TokenType.VOID, word));
                        break;
                    case "int":
                        tokenList.Add(new Token(TokenType.INT, word));
                        break;
                    case "long":
                        tokenList.Add(new Token(TokenType.INT, word));
                        break;
                    case "double":
                        tokenList.Add(new Token(TokenType.INT, word));
                        break;
                    case "char":
                        tokenList.Add(new Token(TokenType.CHAR, word));
                        break;
                    case "return":
                        tokenList.Add(new Token(TokenType.RETURN, word));
                        break;
                    case "+":
                        tokenList.Add(new Token(TokenType.PLUS, word));
                        break;
                    case "-":
                        tokenList.Add(new Token(TokenType.MINUS, word));
                        break;
                    case "*":
                        tokenList.Add(new Token(TokenType.MULT, word));
                        break;
                    case "/":
                        tokenList.Add(new Token(TokenType.DIV, word));
                        break;
                    case "%":
                        tokenList.Add(new Token(TokenType.MODULUS, word));
                        break;
                    case ">":
                        tokenList.Add(new Token(TokenType.GT, word));
                        break;
                    case "<":
                        tokenList.Add(new Token(TokenType.LT, word));
                        break;
                    case ">=":
                        tokenList.Add(new Token(TokenType.GTE, word));
                        break;
                    case "<=":
                        tokenList.Add(new Token(TokenType.LTE, word));
                        break;
                    case "!":
                        tokenList.Add(new Token(TokenType.NOT, word));
                        break;
                    case "!=":
                        tokenList.Add(new Token(TokenType.NEQL, word));
                        break;
                    case "==":
                        tokenList.Add(new Token(TokenType.EQL, word));
                        break;
                    case "=":
                        tokenList.Add(new Token(TokenType.ASSIGN, word));
                        break;
                    case ";":
                        tokenList.Add(new Token(TokenType.SEMICOL, word));
                        break;
                    case ",":
                        tokenList.Add(new Token(TokenType.COMMA, word));
                        break;
                    case "(":
                        tokenList.Add(new Token(TokenType.LPARANTH, word));
                        break;
                    case ")":
                        tokenList.Add(new Token(TokenType.RPARANTH, word));
                        break;
                    case "{":
                        tokenList.Add(new Token(TokenType.LCBRACKET, word));
                        break;
                    case "}":
                        tokenList.Add(new Token(TokenType.RCBRACKET, word));
                        break;
                    case "[":
                        tokenList.Add(new Token(TokenType.LBRACKET, word));
                        break;
                    case "]":
                        tokenList.Add(new Token(TokenType.RBRACKET, word));
                        break;
                    case "\"":

                    default:
                        if (IsVariable(word))
                            tokenList.Add(new Token(TokenType.VARIABLE, word));
                        else if (IsNumber(word))
                            tokenList.Add(new Token(TokenType.NUMB, word));
                        else if (IsString(word))
                            tokenList.Add(new Token(TokenType.STRING, word));
                        else
                            Console.WriteLine("Failed to parse string: " + word);
                            
                        break;
                }
            }

            return tokenList;
        }

        private List<string> Parse(string line)
        {
            List<string> tokenList = new List<string>();
            string word = "";
            line = line.TrimStart();

            for (int i = 0; i < line.Length; i++)
            {
                if (delimiter.Contains(line[i]))
                {
                    if (word.Length > 0)
                    {
                        tokenList.Add(word);
                        word = "";
                    }
                    if (line[i] != ' ')
                    {
                        if (i + 1 < line.Length && ((line[i] == '<' || line[i] == '>' || line[i] == '!' || line[i] == '=') && line[i + 1] == '='))
                        {
                            tokenList.Add((line[i].ToString() + line[i + 1].ToString()));
                            i++;
                        }
                        else if (line[i] == '\"')
                        {
                            string str = "";
                            bool brk = false;

                            while (i < line.Length && !brk)
                            {
                                str += line[i];
                                i++;

                                if (line[i] == '\"')
                                    brk = true;
                            }

                            str += '\"';
                            tokenList.Add(str);
                        }
                        else
                        {
                            tokenList.Add((line[i].ToString()));
                        }
                    }
                }
                else
                {
                    word += line[i];
                }
            }

            if (word.Length > 0)
            {
                tokenList.Add(word);
                word = "";
            }

            return tokenList;
        }

        private bool IsNumber(string word)
        {
            if (word == null || word.Length == 0)
                return false;

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] < '0' || word[i] > '9')
                    return false;
            }

            return true; ;
        }

        private bool IsVariable(string word)
        {
            if (word == null || word.Length == 0)
                return false;

            if (word[0] < 'a' || word[0] > 'z')
                return false;

            for (int i = 1; i < word.Length; i++)
            {
                if (!(word[i] >= 'a' || word[i] <= 'z' || word[i] >= '0' || word[i] <= '9'))
                    return false;
            }

            return true;
        }

        public static bool IsString(string word)
        {
            if (word[0] == '"' && word[word.Length - 1] == '"')
                return true;

            return false;
        }

        public static bool IsDataType(TokenType tokenType)
        {
            return dataTypes.Contains(tokenType);
        }

        public static bool IsBinaryOp(TokenType tokenType)
        {
            return binaryOpTypes.Contains(tokenType);
        }

    }

    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, string value)
        {
            this.Type = type;
            this.Value = value;
        }
        
        public override string ToString()
        {
            return Value;
        }

    }

    public enum TokenType
    {
        PLUS,
        MINUS,
        DIV,
        MULT,
        MODULUS,
        INCR,
        DECR,
        VARIABLE,
        NUMB,
        STRING,

        INT,
        LONG,
        DOUBLE,
        CHAR,
        VOID,

        LPARANTH,
        RPARANTH,
        LBRACKET,
        RBRACKET,
        LCBRACKET,
        RCBRACKET,
        ASSIGN,
        COMMA,
        SEMICOL,
        LT,
        LTE,
        GT,
        GTE,
        EQL,
        NEQL,
        OPRT,

        KEYWORD,
        IF,
        ELSE,
        WHILE,
        RETURN,
        NOT
    }
}
