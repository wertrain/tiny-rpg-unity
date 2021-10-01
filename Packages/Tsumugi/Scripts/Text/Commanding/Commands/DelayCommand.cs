namespace Tsumugi.Text.Commanding.Commands
{
    /// <summary>
    /// 文字送り速度設定コマンド
    /// </summary>
    public class DelayCommand : CommandBase
    {
        /// <summary>
        /// 文字送り速度
        /// </summary>
        public ReferenceVariable<int> Speed { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="speed">参照解決済み変数</param>
        public DelayCommand(ReferenceVariable<int> speed)
        {
            Speed = speed;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="speed">変数名</param>
        public DelayCommand(string speed) : this(new ReferenceVariable<int>(speed)) { }
    }
}
