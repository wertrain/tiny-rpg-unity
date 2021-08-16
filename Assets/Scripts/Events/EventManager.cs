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
        /// �C�x���g�̊J�n��ʒm
        /// </summary>
        /// <param name="sceneController"></param>
        void OnStartEvent(Event.EventSceneController sceneController);

        /// <summary>
        /// �C�x���g�̏I����ʒm
        /// </summary>
        /// <param name="sceneController"></param>
        void OnEndEvent(Event.EventSceneController sceneController);
    }

    public class EventManager : MonoBehaviour
    {
        /// <summary>
        /// �C�x���g�����삷��J����
        /// </summary>
        public GameObject EventCamera;

        /// <summary>
        /// 
        /// </summary>
        public Event.EventSceneController SceneController { get; private set; }

        /// <summary>
        /// �L���ȃJ�������擾
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
        /// �R�[���o�b�N��ǉ�
        /// </summary>
        /// <param name="callback"></param>
        public void AddCallback(EventCallback callback)
        {
            SceneController?.EventCallbacks.Add(callback);
        }
    }
}
