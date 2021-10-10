﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tsumugi.Text.Executing
{
    /// <summary>
    /// フォント
    /// </summary>
    public class Font
    {
        /// <summary>
        /// 文字サイズ
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// フォント名
        /// </summary>
        public string Face { get; set; }

        /// <summary>
        /// 文字色
        /// </summary>
        public uint Color { get; set; }

        /// <summary>
        /// ルビのサイズ
        /// </summary>
        public int RubySize { get; set; }

        /// <summary>
        /// ルビの位置オフセット
        /// </summary>
        public int RubyOffset { get; set; }

        /// <summary>
        /// ルビのフォント名
        /// </summary>
        public string RubyFace { get; set; }

        /// <summary>
        /// 影をつけるか
        /// </summary>
        public bool Shadow { get; set; }

        /// <summary>
        /// 影の色
        /// </summary>
        public uint ShadowColor { get; set; }

        /// <summary>
        /// 縁取りするか
        /// </summary>
        public bool Edge { get; set; }

        /// <summary>
        /// 縁取りの色
        /// </summary>
        public uint EdgeColor { get; set; }

        /// <summary>
        /// 太字にするか
        /// </summary>
        public bool Bold { get; set; }

        /// <summary>
        /// 外部フォントファイル名
        /// </summary>
        public string FontFilePath { get; set; }
    }

    /// <summary>
    /// 画像
    /// </summary>
    public class Image
    {
        /// <summary>
        /// ファイルパス
        /// </summary>
        public string Storage { get; set; }

        /// <summary>
        /// レイヤー番号
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// 表示フラグ
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 色（#RRGGBB 形式）
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 不透明度
        /// </summary>
        public uint Opacity { get; set; }
    }
}
