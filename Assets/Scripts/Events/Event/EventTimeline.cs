using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events.Event
{
    /// <summary>
    /// タイムライン管理
    /// </summary>
    public class EventTimeline
    {
        /// <summary>
        /// 現在の再生時間
        /// </summary>
        public float CurrentTime { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public float StartTime { get; set; }

        /// <summary>
        /// 終了時間
        /// </summary>
        public float EndTime { get; set; }

        /// <summary>
        /// 自身を保持するイベントシーン
        /// </summary>
        private EventSceneController _scene;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="eventManager"></param>
        public EventTimeline(EventSceneController eventScene)
        {
            _scene = eventScene;
        }

        /// <summary>
        /// 時間を進める
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public bool Step(float deltaTime)
        {
            CurrentTime += deltaTime;
            return EndTime < CurrentTime;
        }
    }
}
