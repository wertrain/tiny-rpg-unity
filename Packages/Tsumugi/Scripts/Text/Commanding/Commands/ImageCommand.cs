using System;
using System.Collections.Generic;
using System.Text;

namespace Tsumugi.Text.Commanding.Commands
{
    /// <summary>
    /// 画像コマンド
    /// </summary>
    public class ImageCommand : CommandBase
    {
        /// <summary>
        /// ファイルパス
        /// </summary>
        public ReferenceVariable<string> Storage { get; set; }

        /// <summary>
        /// レイヤー番号
        /// </summary>
        public ReferenceVariable<int> Layer { get; set; }

        /// <summary>
        /// 表示フラグ
        /// </summary>
        public ReferenceVariable<bool> Visible { get; set; }

        /// <summary>
        /// 色（#RRGGBB 形式）
        /// </summary>
        public ReferenceVariable<string> Color { get; set; }

        /// <summary>
        /// 不透明度
        /// </summary>
        public ReferenceVariable<uint> Opacity { get; set; }
    }
}
