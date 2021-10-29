using System;
using System.Collections.Generic;
using System.Text;

namespace Tsumugi.Unity
{
    public class PrintTextProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        public int TextLength { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PrintTextProcessor()
        {
            _tokens = new LinkedList<Text.Lexing.Token>();
            _activeTags = new List<UnityRichTextTag>();
            _unclosedTags = new List<UnityRichTextTag>();
            TextLength = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public void AddText(string text)
        {
            var lexer = new PrintTextLexer(text);
            var parser = new PrintTextParser(lexer);

            var tokens = parser.Parse();
            foreach (var token in tokens)
            {
                if (token.Type == Text.Lexing.TokenType.Text)
                    TextLength += token.Literal.Length;
                _tokens.AddLast(token);
            }
        }

        /// <summary>
        /// テキストをクリアする
        /// 未処理のタグはそのまま残留させる
        /// </summary>
        public void ClearText()
        {
            UpdateUnclosedTags(_tokens);
            TextLength = 0;
            _tokens.Clear();
        }

        /// <summary>
        /// テキストとタグ情報をクリアする
        /// </summary>
        public void Clear()
        {
            _unclosedTags.Clear();
            TextLength = 0;
            _tokens.Clear();
        }

        /// <summary>
        /// 指定されたインデックス番号までの文字を取得する
        /// 途中でタグが存在した場合、自動で終端用のタグを設定する
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetText(int index)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // 前回のテキストで残っているタグの処理とマージ
            foreach (var tag in _unclosedTags)
            {
                tag.BeginTag(stringBuilder);
            }
            _activeTags.Clear();
            _activeTags.AddRange(_unclosedTags);

            int left = index;

            var tokenNode = _tokens.First;
            while (tokenNode != null)
            {
                var token = tokenNode.Value;

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
                        ProcessTag(tokenNode, _activeTags, stringBuilder);
                        break;
                }

                // 抜けるときはすべてのタグを閉じる
                if (left <= 0)
                {
                    foreach (var tag in _activeTags)
                    {
                        tag.EndTag(stringBuilder);
                    }
                    break;
                }

                tokenNode = tokenNode.Next;
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="stringBuilder"></param>
        private void ProcessTag(LinkedListNode<Text.Lexing.Token> tokenNode, List<UnityRichTextTag> tags, StringBuilder stringBuilder)
        {
            var token = tokenNode.Value;
            var tagName = token.Literal.ToLower();

            // 閉じタグの場合、保持しているタグのリストからそのタグを消去
            if (tagName[0] == '/')
            {
                tagName = tagName.Substring(1);
                if (UnityRichTextTag.GetTag(tagName, out var tag))
                {
                    for (int i = tags.Count - 1; i >= 0; i--)
                    {
                        if (tags[i].Tag == tag)
                        {
                            tags[i].EndTag(stringBuilder);
                            tags.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            // 新しいタグの場合、タグのリストに保存する
            else
            {
                if (UnityRichTextTag.GetTag(token.Literal, out var tag))
                {
                    var textTag = new UnityRichTextTag(tag);

                    var nextToken = tokenNode.Next?.Value;
                    if (nextToken.Type == Text.Lexing.TokenType.TagAttributeValue)
                    {
                        textTag.Value = nextToken.Literal;
                    }

                    textTag.BeginTag(stringBuilder);
                    tags.Add(textTag);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        private void UpdateUnclosedTags(LinkedList<Text.Lexing.Token> tokens)
        {
            var stringBuilder = new StringBuilder();

            var tokenNode = tokens.First;
            while (tokenNode != null)
            {
                var token = tokenNode.Value;

                switch (token.Type)
                {
                    case Text.Lexing.TokenType.Tag:
                        ProcessTag(tokenNode, _unclosedTags, stringBuilder);
                        break;
                }

                tokenNode = tokenNode.Next;
            }
        }

        /// <summary>
        /// 分解後のトークン
        /// </summary>
        private LinkedList<Text.Lexing.Token> _tokens;

        /// <summary>
        /// アクティブなリッチテキストタグ
        /// </summary>
        private List<UnityRichTextTag> _activeTags;

        /// <summary>
        /// アクティブなリッチテキストタグ
        /// </summary>
        private List<UnityRichTextTag> _unclosedTags;

        /// <summary>
        /// リッチテキストを表すクラス
        /// </summary>
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

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="type"></param>
            public UnityRichTextTag(TagType type)
            {
                Tag = type;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="stringBuilder"></param>
            public void BeginTag(StringBuilder stringBuilder)
            {
                switch (Tag)
                {
                    case TagType.Bold: stringBuilder.Append("<b>"); break;
                    case TagType.Italic: stringBuilder.Append("<i>"); break;
                    case TagType.Size: stringBuilder.Append($"<size={Value}>"); break;
                    case TagType.Color: stringBuilder.Append($"<color={Value}>"); break;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="stringBuilder"></param>
            public void EndTag(StringBuilder stringBuilder)
            {
                switch (Tag)
                {
                    case TagType.Bold: stringBuilder.Append("</b>"); break;
                    case TagType.Italic: stringBuilder.Append("</i>"); break;
                    case TagType.Size: stringBuilder.Append("</size>"); break;
                    case TagType.Color: stringBuilder.Append("</color>"); break;
                }
            }

            /// <summary>
            /// タグ名から有効なタグを判定する
            /// </summary>
            /// <param name="tagName"></param>
            /// <param name="tag"></param>
            /// <returns></returns>
            public static bool GetTag(string tagName, out TagType tag)
            {
                switch (tagName.ToLower())
                {
                    case "i":
                        tag = TagType.Italic;
                        break;
                    case "color":
                        tag = TagType.Color;
                        break;
                    case "b":
                        tag = TagType.Bold;
                        break;
                    case "size":
                        tag = TagType.Size;
                        break;
                    default:
                        tag = TagType.Invalid;
                        return false;
                }

                return true;
            }
        }
    }
}
