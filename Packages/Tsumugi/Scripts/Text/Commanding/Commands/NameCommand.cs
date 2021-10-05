namespace Tsumugi.Text.Commanding.Commands
{
    /// <summary>
    /// 名前設定コマンド
    /// </summary>
    public class NameCommand : CommandBase
    {
        /// <summary>
        /// 名前
        /// </summary>
        public ReferenceVariable<string> Text { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="speed">参照解決済み変数</param>
        public NameCommand(ReferenceVariable<string> text)
        {
            Text = text;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="speed">変数名</param>
        public NameCommand(string name)
        {
            Text = new ReferenceVariable<string>(name);
        }
    }
}
