using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    /// <summary>
    /// 
    /// </summary>
    public interface EventCallback
    {
        /// <summary>
        /// イベントの開始を通知
        /// </summary>
        /// <param name="sceneController"></param>
        void OnStartEvent(Event.EventSceneController sceneController);

        /// <summary>
        /// イベントの終了を通知
        /// </summary>
        /// <param name="sceneController"></param>
        void OnEndEvent(Event.EventSceneController sceneController);
    }

    public class EventManager : MonoBehaviour
    {
        /// <summary>
        /// イベントが操作するカメラ
        /// </summary>
        public GameObject EventCamera;

        /// <summary>
        /// 
        /// </summary>
        public Event.EventSceneController SceneController { get; private set; }

        /// <summary>
        /// 有効なカメラを取得
        /// </summary>
        public Camera ActiveCamera
        {
            get
            {
                var camera = EventCamera?.GetComponent<Camera>();

                if (camera == null)
                {
                    camera = Camera.main;
                }

                return camera;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public EventManager()
        {
            SceneController = new Event.EventSceneController(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateEvent(float time)
        {
            SceneController.Update(Time.deltaTime);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Play()
        {
            SceneController.SetPlaybackState(Event.EventSceneController.PlaybackStatus.Play);
        }

        /// <summary>
        /// コールバックを追加
        /// </summary>
        /// <param name="callback"></param>
        public void AddCallback(EventCallback callback)
        {
            SceneController?.EventCallbacks.Add(callback);
        }
    }
}
