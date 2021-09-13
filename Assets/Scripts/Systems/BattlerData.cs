using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlerData
{
    /// <summary>
    /// キャラクターステータス
    /// </summary>
    public class Status
    {
        /// <summary>
        /// レベル
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// ヒットポイント
        /// </summary>
        public int HitPoint { get; set; }

        /// <summary>
        /// マジックポイント
        /// </summary>
        public int MagicPoint { get; set; }

        /// <summary>
        /// 攻撃力
        /// </summary>
        public int Attack { get; set; }

        /// <summary>
        /// 防御力
        /// </summary>
        public int Defense { get; set; }

        /// <summary>
        /// 知力
        /// </summary>
        public int Intelligence { get; set; }

        /// <summary>
        /// 魔法抵抗力
        /// </summary>
        public int Resist { get; set; }

        /// <summary>
        /// 素早さ
        /// </summary>
        public int Agility { get; set; }
    }

    /// <summary>
    /// 状態の定義
    /// </summary>
    public enum Conditions : int
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,

        /// <summary>
        /// 毒
        /// </summary>
        Poison = 1 << 0
    }

    /// <summary>
    /// キャラクター名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 基本のステータス
    /// </summary>
    public Status BaseStatus { get; set; }

    /// <summary>
    /// 現在のステータス
    /// </summary>
    public Status CurrentStatus { get; set; }

    /// <summary>
    /// 状態
    /// </summary>
    public int Condition { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Sprite Face { get; set; }
}
