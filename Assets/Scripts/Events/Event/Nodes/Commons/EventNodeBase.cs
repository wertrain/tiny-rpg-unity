using System.Collections.Generic;

namespace Events.Event.Nodes.Commons
{
    /// <summary>
    /// すべてのノードの基底クラス
    /// </summary>
    public class EventNodeBase
    {
        /// <summary>
        /// 親ツリー
        /// </summary>
        public EventNodeTree Owner { get; set; }

        /// <summary>
        /// イベントマネージャーを取得
        /// </summary>
        public EventManager EventManager { get { return Owner?.SceneController?.Manager; } }

        /// <summary>
        /// 親ノード
        /// </summary>
        public EventNodeBase Parent { get; set; }

        /// <summary>
        /// 子ノード
        /// </summary>
        public List<EventNodeBase> Children { get; private set; }

        /// <summary>
        /// 変換情報
        /// </summary>
        public Foundations.EventTransform Transform { get; set; }
        
        /// <summary>
        /// タグ
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="nodeTree"></param>
        public EventNodeBase(EventNodeTree nodeTree)
        {
            Owner = nodeTree;
            Children = new List<EventNodeBase>();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="time"></param>
        /// <param name="transform"></param>
        public virtual void Update(float time)
        {

        }

        /// <summary>
        /// 子ノード更新後の更新処理
        /// </summary>
        /// <param name="time"></param>
        /// <param name="transform"></param>
        public virtual void UpdateAfterChildren(float time)
        {

        }
    }
}
