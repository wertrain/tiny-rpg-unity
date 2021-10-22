using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tsumugi.Localize;
using Tsumugi.Text.Commanding;

namespace Tsumugi.Unity
{
    public class PrintTextLexer
    {
        /// <summary>
        /// コントロール文字の定義
        /// </summary>
        class ControlCharacter
        {
            /// <summary>
            /// タグの開始文字
            /// </summary>
            public const char TagStart = '<';

            /// <summary>
            /// タグの終了文字
            /// </summary>
            public const char TagEnd = '>';

            /// <summary>
            /// タグと属性のセパレータ
            /// </summary>
            public const char TagAttributeSeparator = ' ';

            /// <summary>
            /// 割り当て記号
            /// </summary>
            public const char Assignment = '=';
        }

        /// <summary>
        /// 字句解析用の文字列リーダー
        /// </summary>
        private Script.Lexing.LexingStringReader Reader { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="script"></param>
        public PrintTextLexer(string script)
        {
            Reader = new Script.Lexing.LexingStringReader(script);
        }

        /// <summary>
        /// 次のトークンを取り出す
        /// </summary>
        /// <returns></returns>
        public Text.Lexing.Token NextToken()
        {
            SkipNewLine();

            Text.Lexing.Token token = null;

            int next = Reader.Peek();

            if (next < 0)
            {
                token = CreateToken(Text.Lexing.TokenType.EOF, string.Empty);
            }
            else
            {
                char c = (char)next;

                switch (c)
                {
                    case ControlCharacter.TagStart:
                        Reader.ReadChar();
                        token = ReadTag();
                        break;

                    case ControlCharacter.TagEnd:
                        token = CreateToken(Text.Lexing.TokenType.TagEnd, string.Empty);
                        break;

                    case ControlCharacter.TagAttributeSeparator:
                        if (PrevToken?.Type != Text.Lexing.TokenType.Tag && PrevToken?.Type != Text.Lexing.TokenType.TagAttributeValue) goto default;
                        Reader.ReadChar();
                        token = ReadTagAttributeName();
                        break;

                    case ControlCharacter.Assignment:
                        if (PrevToken?.Type != Text.Lexing.TokenType.TagAttributeName) goto default;
                        Reader.ReadChar();
                        token = ReadTagAttributeValue();
                        break;

                    default:
                        token = ReadText();
                        break;
                }
            }

            Reader.Read();

            return PrevToken = token;
        }

        /// <summary>
        /// 前回のトークン
        /// </summary>
        private Text.Lexing.Token PrevToken { get; set; }

        /// <summary>
        /// トークン作成
        /// </summary>
        /// <param name="type">トークンのタイプ</param>
        /// <param name="literal">トークンのリテラル</param>
        /// <returns>トークン</returns>
        private Text.Lexing.Token CreateToken(Text.Lexing.TokenType type, string literal)
        {
            return new Text.Lexing.Token(type, literal, Reader.GetLexingPosition());
        }

        /// <summary>
        /// ラベルの見出し読み込み
        /// </summary>
        /// <returns></returns>
        private string ReadLabelHeadlineText()
        {
            var label = new StringBuilder();

            while (!IsWhiteSpace(Reader.PeekChar()) && !IsControlCharacter(Reader.PeekChar()))
            {
                label.Append(Reader.ReadChar());
            }

            Reader.Seek(-1, SeekOrigin.Current);

            return label.ToString();
        }

        /// <summary>
        /// 空白文字まで文字列として読み込み
        /// </summary>
        /// <returns></returns>
        private string ReadTextToWhiteSpace()
        {
            Reader.ReadChar();

            var label = new StringBuilder();

            while (!IsWhiteSpace(Reader.PeekChar()))
            {
                label.Append(Reader.ReadChar());
            }

            Reader.Seek(-1, SeekOrigin.Current);

            return label.ToString();
        }

        /// <summary>
        /// タグの読み込み
        /// </summary>
        /// <returns></returns>
        private Text.Lexing.Token ReadTag()
        {
            var tag = new StringBuilder();

            char c = char.MaxValue;

            while ((c = Reader.PeekChar()) != ControlCharacter.TagEnd)
            {
                // 改行または終端が先に見つかった場合は、不正なトークン
                if (c == '\r' || c == '\n' || c == char.MaxValue)
                {
                    return CreateToken(Text.Lexing.TokenType.Illegal, c.ToString());
                }
                // 属性が見つかれば終了
                else if (c == ControlCharacter.TagAttributeSeparator)
                {
                    break;
                }
                tag.Append(Reader.ReadChar());
            }

            Reader.Seek(-1, SeekOrigin.Current);

            return CreateToken(Text.Lexing.TokenType.Tag, tag.ToString());
        }

        /// <summary>
        /// タグ属性名として読み込み
        /// </summary>
        /// <returns></returns>
        private Text.Lexing.Token ReadTagAttributeName()
        {
            SkipWhiteSpace();

            var text = new StringBuilder();

            char c = char.MaxValue;
            while ((c = Reader.PeekChar()) != ControlCharacter.TagEnd)
            {
                // 改行または終端が先に見つかった場合は、不正なトークン
                if (c == '\r' || c == '\n' || c == char.MaxValue)
                {
                    return CreateToken(Text.Lexing.TokenType.Illegal, c.ToString());
                }
                // 代入記号の前までの空白は許可
                else if (IsWhiteSpace(c))
                {
                    Reader.ReadChar();
                    continue;
                }
                // 代入記号が見つかれば終了
                else if (c == ControlCharacter.Assignment)
                {
                    break;
                }

                text.Append(Reader.ReadChar());
            }

            Reader.Seek(-1, SeekOrigin.Current);

            return CreateToken(Text.Lexing.TokenType.TagAttributeName, text.ToString());
        }

        /// <summary>
        /// タグ属性値として読み込み
        /// </summary>
        /// <returns></returns>
        private Text.Lexing.Token ReadTagAttributeValue()
        {
            SkipWhiteSpace();

            var text = new StringBuilder();

            // 属性値が " で始まるかチェックする
            bool isStringValue = false;
            if (isStringValue = (Reader.PeekChar() == '"'))
            {
                // 読み飛ばす
                Reader.ReadChar();
            }

            char c = char.MaxValue;
            while ((c = Reader.PeekChar()) != ControlCharacter.TagEnd)
            {
                // エスケープシーケンス
                char next = Reader.PeekChar(1);
                if (c == '\\' && (IsControlCharacter(next) || next == '"'))
                {
                    // 読み飛ばす
                    Reader.ReadChar();
                }
                if (c == '\r' || c == '\n' || c == char.MaxValue) 
                {
                    // 改行または終端が先に見つかった時 Tag の場合はエラーとしたい規則だが、TagLine の場合は許可される
                    break;
                }
                // 文字列の終了記号で終了
                if (isStringValue)
                {
                    if (c == '"')
                    {
                        // 読み飛ばす
                        Reader.ReadChar();
                        break;
                    }
                }
                // 属性が見つかれば終了
                else if (c == ControlCharacter.TagAttributeSeparator) break;

                text.Append(Reader.ReadChar());
            }

            Reader.Seek(-1, SeekOrigin.Current);

            return CreateToken(Text.Lexing.TokenType.TagAttributeValue, text.ToString());
        }

        /// <summary>
        /// テキストとして読み込み
        /// </summary>
        /// <returns></returns>
        private Text.Lexing.Token ReadText()
        {
            var text = new StringBuilder();

            char c = char.MinValue;

            while ((c = Reader.PeekChar()) != char.MaxValue)
            {
                var next = Reader.PeekChar(1);
                // エスケープシーケンス
                if (c == '\\' && IsControlCharacter(next))
                {
                    // 読み飛ばす
                    Reader.ReadChar();
                }
                else if (IsControlCharacter(c))
                {
                    Reader.Seek(-1, SeekOrigin.Current);
                    break;
                }

                text.Append(Reader.ReadChar());

                SkipNewLine();
            }

            return CreateToken(Text.Lexing.TokenType.Text, text.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool IsControlCharacter(char c)
        {
            return (c == ControlCharacter.TagStart);
        }

        /// <summary>
        /// 空白文字・改行文字かを判定
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool IsWhiteSpace(char c)
        {
            return (c == ' ' || c == '\t' || c == '\r' || c == '\n');
        }

        /// <summary>
        /// 改行の間、リーダーを進める
        /// </summary>
        private void SkipNewLine()
        {
            var next = Reader.PeekChar();

            while (next == '\r' || next == '\n')
            {
                Reader.ReadChar();
                next = Reader.PeekChar();
            }
        }

        /// <summary>
        /// 空白・タブ・改行などの間、リーダーを進める
        /// </summary>
        private void SkipWhiteSpace()
        {
            var next = Reader.PeekChar();

            while (next == ' '
                || next == '\t'
                || next == '\r'
                || next == '\n')
            {
                Reader.ReadChar();
                next = Reader.PeekChar();
            }
        }
    }
}