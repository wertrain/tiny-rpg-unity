using Events.Event.Nodes.Commons;

namespace Events.Event.Nodes
{
    /// <summary>
    /// イベントクリップを保持するノード
    /// </summary>
    public class ClipNode : EventNodeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public EventClipBase EventClip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private enum Flags
        {
            FirstUpdated = 1 << 0
        }

        /// <summary>
        /// 
        /// </summary>
        private uint _flag;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClipNode(EventNodeTree nodeTree)
            : base(nodeTree)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="transform"></param>
        override public void Update(float time)
        {
            if (EventClip.StartTime <= time && EventClip.EndTime > time)
            {
                if ((_flag & (uint)Flags.FirstUpdated) == 0)
                {
                    _flag |= (uint)Flags.FirstUpdated;

                    if (!EventClip.FirstUpdate(time))
                    {
                        return;
                    }
                }

                EventClip.Update(time);
            }
            else
            {
                if ((_flag & (uint)Flags.FirstUpdated) > 0)
                {
                    _flag &= ~(uint)Flags.FirstUpdated;

                    EventClip.LastUpdate(time);
                }
            }
        }
    }
}
