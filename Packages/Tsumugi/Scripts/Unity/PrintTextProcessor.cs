using System;
using System.Collections.Generic;
using System.Text;

namespace Tsumugi.Unity
{
    public class PrintTextProcessor
    {
        private class UnityRichTextTag
        {
            /// <summary>
            /// Unity のリッチテキストで対応しているタグ
            /// </summary>
            public enum TagType
            {
                Bold,
                Italic,
                Size,
                Color,
            }

            /// <summary>
            /// タグ
            /// </summary>
            public TagType Tag { get; set; }

            /// <summary>
            /// タグの値
            /// </summary>
            public string Value { get; set; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PrintTextProcessor()
        {
            _tokens = new List<Text.Lexing.Token>();
            _activeTags = new List<UnityRichTextTag>();
        }

        public void AddText(string text)
        {
            var lexer = new PrintTextLexer(text);
            var parser = new PrintTextParser(lexer);

            var tokens = parser.Parse();
            _tokens.AddRange(tokens);
        }

        public string GetText(int index)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var token in _tokens)
            {
                switch (token.Type)
                {
                    case Text.Lexing.TokenType.Text:

                        break;

                    case Text.Lexing.TokenType.Tag:

                        break;
                }
            }

            return string.Empty;
        }
        private List<Text.Lexing.Token> _tokens;

        private List<UnityRichTextTag> _activeTags;
    }
}
