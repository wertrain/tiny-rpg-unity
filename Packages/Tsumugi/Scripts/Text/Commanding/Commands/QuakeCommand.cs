namespace Tsumugi.Text.Commanding.Commands
{
    /// <summary>
    /// 画面揺らしコマンド
    /// </summary>
    public class QuakeCommand : CommandBase
    {
        /// <summary>
        /// 揺らし時間
        /// </summary>
        public ReferenceVariable<int> Time { get; set; }

        /// <summary>
        /// 水平方向揺れ
        /// </summary>
        public ReferenceVariable<int> PowerH { get; set; }

        /// <summary>
        /// 垂直方向揺れ
        /// </summary>
        public ReferenceVariable<int> PowerV { get; set; }
    }
}
