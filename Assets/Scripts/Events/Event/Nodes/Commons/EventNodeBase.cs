using System.Collections.Generic;

namespace Events.Event.Nodes.Commons
{
    /// <summary>
    /// ���ׂẴm�[�h�̊��N���X
    /// </summary>
    public class EventNodeBase
    {
        /// <summary>
        /// �e�c���[
        /// </summary>
        public EventNodeTree Owner { get; set; }

        /// <summary>
        /// �C�x���g�}�l�[�W���[���擾
        /// </summary>
        public EventManager EventManager { get { return Owner?.SceneController?.Manager; } }

        /// <summary>
        /// �e�m�[�h
        /// </summary>
        public EventNodeBase Parent { get; set; }

        /// <summary>
        /// �q�m�[�h
        /// </summary>
        public List<EventNodeBase> Children { get; private set; }

        /// <summary>
        /// �ϊ����
        /// </summary>
        public Foundations.EventTransform Transform { get; set; }
        
        /// <summary>
        /// �^�O
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="nodeTree"></param>
        public EventNodeBase(EventNodeTree nodeTree)
        {
            Owner = nodeTree;
            Children = new List<EventNodeBase>();
        }

        /// <summary>
        /// �X�V����
        /// </summary>
        /// <param name="time"></param>
        /// <param name="transform"></param>
        public virtual void Update(float time)
        {

        }

        /// <summary>
        /// �q�m�[�h�X�V��̍X�V����
        /// </summary>
        /// <param name="time"></param>
        /// <param name="transform"></param>
        public virtual void UpdateAfterChildren(float time)
        {

        }
    }
}
