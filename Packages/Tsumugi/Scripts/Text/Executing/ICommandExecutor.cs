namespace Tsumugi.Text.Executing
{
    /// <summary>
    /// コマンド実行の基底クラス
    /// </summary>
    public interface ICommandExecutor
    {
        /// <summary>
        /// 文字列の表示
        /// </summary>
        /// <param name="text"></param>
        void PrintText(string text);

        /// <summary>
        /// 改行
        /// </summary>
        void StartNewLine();

        /// <summary>
        /// キー入力待ち
        /// </summary>
        void WaitAnyKey();

        /// <summary>
        /// 改ページ
        /// </summary>
        void StartNewPage();

        /// <summary>
        /// 指定時間の待ち
        /// </summary>
        void WaitTime(int millisec);

        /// <summary>
        /// フォント設定
        /// </summary>
        /// <param name="font"></param>
        void SetFont(Font font);

        /// <summary>
        /// デフォルトフォント設定
        /// </summary>
        /// <param name="font"></param>
        void SetDefaultFont(Font font);

        /// <summary>
        /// 画面を揺らす
        /// </summary>
        /// <param name="millisec"></param>
        /// <param name="powerH"></param>
        /// <param name="powerV"></param>
        void Quake(int millisec, int powerH, int powerV);

        /// <summary>
        /// 画面を揺らし終了待ち
        /// </summary>
        /// <param name="canSkip"></param>
        void WaitQuake(bool canSkip);

        /// <summary>
        /// 文字送り速度
        /// </summary>
        /// <param name="speed"></param>
        void Delay(int speed);

        /// <summary>
        /// 名前を設定
        /// </summary>
        /// <param name="text"></param>
        void SetName(string text);

        /// <summary>
        /// 画像
        /// </summary>
        /// <param name="image"></param>
        void SetImage(Image image);

        /// <summary>
        /// フォントを初期化
        /// </summary>
        void ResetFont();
    }
}
