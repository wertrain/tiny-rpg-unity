using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events.Event
{
    /// <summary>
    /// イベントシーンクラス
    /// </summary>
    public class EventSceneController
    {
        /// <summary>
        /// 再生状態
        /// </summary>
        public enum PlaybackStatus : int
        {
            Idle,
            Play,
            Pause,
            Stop,
            Max
        }

        /// <summary>
        /// 自身を保持するイベントマネージャー
        /// </summary>
        public EventManager Manager { get; private set; }

        /// <summary>
        /// イベントの構成要素
        /// </summary>
        public EventNodeTree NodeTree { get; private set; }

        /// <summary>
        /// イベントのタイムライン
        /// </summary>
        public EventTimeline Timeline { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public List<EventCallback> EventCallbacks { get; private set; }

        /// <summary>
        /// ステートマシン
        /// </summary>
        private Foundations.StateMachine<EventSceneController> _stateMachine;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="eventManager"></param>
        public EventSceneController(EventManager eventManager)
        {
            Manager = eventManager;

            NodeTree = new EventNodeTree(this);
            Timeline = new EventTimeline(this);

            _stateMachine = new Foundations.StateMachine<EventSceneController>(this);
            _stateMachine.AddAnyTransition<IdleState>((int)PlaybackStatus.Idle);
            _stateMachine.SetStartState<PlayState>();

            EventCallbacks = new List<EventCallback>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootNode"></param>
        public bool CreateScene(Nodes.RootNode rootNode)
        {
            if (!NodeTree.CreateNodeFrom(rootNode)) return false;

            Timeline.StartTime = 0f;

            float endTime = 0f;
            NodeTree.ForEachNodes((node) =>
            {
                if (node is Nodes.ClipNode clipNode)
                {
                    endTime = Mathf.Max(endTime, clipNode.EventClip.EndTime);
                }
            });
            Timeline.EndTime = endTime;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SetPlaybackState(PlaybackStatus playbackStatus)
        {
            return _stateMachine.SendEvent((int)playbackStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            _stateMachine.Update();
        }

        /// <summary>
        /// Idle ステート
        /// </summary>
        private class IdleState : Foundations.StateMachine<EventSceneController>.State
        {
            protected internal override void Update()
            {

            }
        }

        /// <summary>
        /// Play ステート
        /// </summary>
        private class PlayState : Foundations.StateMachine<EventSceneController>.State
        {
            protected internal override void Enter()
            {
                Context.Timeline.CurrentTime = Context.Timeline.StartTime;

                foreach (var callback in Context.EventCallbacks)
                {
                    callback.OnStartEvent(Context);
                }
            }

            protected internal override void Update()
            {
                var transform = new Foundations.EventTransform(Context.Manager.gameObject.transform);
                Context.NodeTree.Transform = transform;
                Context.NodeTree.Update(Context.Timeline.CurrentTime);

                if (Context.Timeline.Step(Time.deltaTime))
                {
                    Context.SetPlaybackState(PlaybackStatus.Stop);

                    foreach (var callback in Context.EventCallbacks)
                    {
                        callback.OnEndEvent(Context);
                    }
                }
            }
        }
    }
}