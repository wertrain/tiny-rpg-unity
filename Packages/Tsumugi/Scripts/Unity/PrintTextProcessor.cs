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
                Invalid,
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

            int left = index;
            foreach (var token in _tokens)
            {
                switch (token.Type)
                {
                    case Text.Lexing.TokenType.Text:
                        if (token.Literal.Length < left)
                        {
                            stringBuilder.Append(token.Literal);
                        }
                        else
                        {
                            stringBuilder.Append(token.Literal.Substring(0, left));
                        }
                        left -= token.Literal.Length;
                        break;

                    case Text.Lexing.TokenType.Tag:
                        if (GetTag(token, out var tag))
                        {
                            _activeTags.Add(new UnityRichTextTag()
                            {
                                Tag = tag
                            });
                        }
                        break;

                    case Text.Lexing.TokenType.TagEnd:
                        if (GetTag(token, out var endTag))
                        {
                            for (int i = _activeTags.Count - 1; i >= 0; i--)
                            {
                                if (_activeTags[i].Tag == endTag)
                                {
                                    _activeTags.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                        break;
                }


                if (left <= 0) break;
            }

            return string.Empty;
        }

        private bool GetTag(Text.Lexing.Token token, out UnityRichTextTag.TagType tag)
        {
            switch (token.Literal.ToLower())
            {
                case "size":
                    tag = UnityRichTextTag.TagType.Size;
                    break;
                default:
                    tag = UnityRichTextTag.TagType.Invalid;
                    return false;
            }

            return true;
        }

        private List<Text.Lexing.Token> _tokens;

        private List<UnityRichTextTag> _activeTags;
    }
}
