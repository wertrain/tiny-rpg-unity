namespace Events.Event.Nodes.Commons
{
    /// <summary>
    /// �C�x���g�N���b�v�̊��N���X
    /// �J�n���ԂƏI�����Ԃ�����
    /// </summary>
    public class EventClipBase
    {
        /// <summary>
        /// ���g�����L����m�[�h
        /// </summary>
        public ClipNode Node { get; private set; }

        /// <summary>
        /// �C�x���g�}�l�[�W���[���擾
        /// </summary>
        public EventManager EventManager { get { return Node?.EventManager; } }

        /// <summary>
        /// �J�n����
        /// </summary>
        public float StartTime { get; set; }

        /// <summary>
        /// �I������
        /// </summary>
        public float EndTime { get; set; }

        /// <summary>
        /// �R���X�g���N�^
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