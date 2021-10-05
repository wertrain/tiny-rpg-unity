namespace Tsumugi.Text.Commanding.Commands
{
    /// <summary>
    /// 画面揺らし終了待ちコマンド
    /// </summary>
    public class WaitQuakeCommand : CommandBase
    {
        /// <summary>
        /// スキップできるかどうか
        /// </summary>
        public bool CanSkip { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="canSkip">スキップできるかどうか</param>
        public WaitQuakeCommand(bool canSkip)
        {
            CanSkip = canSkip;
        }
    }
}
