using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events.Event
{
    /// <summary>
    /// �^�C�����C���Ǘ�
    /// </summary>
    public class EventTimeline
    {
        /// <summary>
        /// ���݂̍Đ�����
        /// </summary>
        public float CurrentTime { get; set; }

        /// <summary>
        /// �J�n����
        /// </summary>
        public float StartTime { get; set; }

        /// <summary>
        /// �I������
        /// </summary>
        public float EndTime { get; set; }

        /// <summary>
        /// ���g��ێ�����C�x���g�V�[��
        /// </summary>
        private EventSceneController _scene;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="eventManager"></param>
        public EventTimeline(EventSceneController eventScene)
        {
            _scene = eventScene;
        }

        /// <summary>
        /// ���Ԃ�i�߂�
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
