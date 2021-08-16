namespace Events.Event.Nodes.Commons
{
    /// <summary>
    /// イベントクリップの基底クラス
    /// 開始時間と終了時間を持つ
    /// </summary>
    public class EventClipBase
    {
        /// <summary>
        /// 自身を所有するノード
        /// </summary>
        public ClipNode Node { get; private set; }

        /// <summary>
        /// イベントマネージャーを取得
        /// </summary>
        public EventManager EventManager { get { return Node?.EventManager; } }

        /// <summary>
        /// 開始時間
        /// </summary>
        public float StartTime { get; set; }

        /// <summary>
        /// 終了時間
        /// </summary>
        public float EndTime { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventClipBase(ClipNode clipNode)
        {
            Node = clipNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public float GetTimeRatio(float time)
        {
            return (time - StartTime) / (EndTime - StartTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        virtual public bool FirstUpdate(float time)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        virtual public void Update(float time)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTime"></param>
        virtual public void LastUpdate(float time)
        {

        }
    }

}