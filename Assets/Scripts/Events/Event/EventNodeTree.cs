using Events.Event.Nodes;
using Events.Event.Nodes.Commons;
using UnityEngine;

namespace Events.Event
{
    /// <summary>
    /// 
    /// </summary>
    public class EventNodeTree
    {
        /// <summary>
        /// 変換情報
        /// </summary>
        public Foundations.EventTransform Transform { get;set; }

        /// <summary>
        /// ルートノード
        /// </summary>
        public RootNode RootNode { get; protected set; }

        /// <summary>
        /// 自身を保持するイベントシーン
        /// </summary>
        public EventSceneController SceneController { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="eventManager"></param>
        public EventNodeTree(EventSceneController eventScene)
        {
            SceneController = eventScene;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootNode"></param>
        /// <returns></returns>
        public bool CreateNodeFrom(RootNode rootNode)
        {
            RootNode = rootNode;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        public void ForEachNodes(System.Action<EventNodeBase> func)
        {
            System.Action<EventNodeBase> recursive = null;

            recursive = (node) =>
            {
                func(node);

                foreach (var child in node.Children)
                {
                    recursive(child);
                }
            };

            recursive(RootNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="transform"></param>
        public void Update(float time)
        {
            if (RootNode == null) return;

            UpdateAll(RootNode, time, Transform);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        private void UpdateAll(EventNodeBase node, float time, Foundations.EventTransform transform)
        {
            node.Transform = transform;
            node.Update(time);

            foreach (var child in node.Children)
            {
                UpdateAll(child, time, node.Transform);
            }

            node.UpdateAfterChildren(time);
        }
    }
}
