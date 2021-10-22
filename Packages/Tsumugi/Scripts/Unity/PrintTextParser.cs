using System.Collections.Generic;
using System.Linq;
using Tsumugi.Localize;
using Tsumugi.Log;
using Tsumugi.Text.Lexing;

namespace Tsumugi.Unity
{
    public class PrintTextParser
    {
        /// <summary>
        /// 現在のトークン
        /// </summary>
        public Token CurrentToken { get; set; }

        /// <summary>
        /// 次のトークン
        /// </summary>
        public Token NextToken { get; set; }

        /// <summary>
        /// 字句解析
        /// </summary>
        public PrintTextLexer Lexer { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="lexer"></param>
        public PrintTextParser(PrintTextLexer lexer)
        {
            Lexer = lexer;

            CurrentToken = Lexer.NextToken();
            NextToken = Lexer.NextToken();
        }

        /// <summary>
        /// パース処理
        /// </summary>
        /// <returns></returns>
        public List<Token> Parse()
        {
            List<Token> tokens = new List<Token>();

            while (CurrentToken.Type != TokenType.EOF)
            {
                tokens.Add(CurrentToken);
                ReadToken();
            }

            return tokens;
        }

        /// <summary>
        /// トークンを一つ読み出す
        /// </summary>
        private void ReadToken()
        {
            CurrentToken = NextToken;
            NextToken = Lexer.NextToken();
        }

        /// <summary>
        /// 次のトークンが期待するものであれば読み飛ばす
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool ExpectPeek(TokenType type)
        {
            if (NextToken.Type == type)
            {
                ReadToken();
                return true;
            }

            return false;
        }

    }
}
