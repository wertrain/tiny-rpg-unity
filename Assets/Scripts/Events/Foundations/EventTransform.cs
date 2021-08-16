using UnityEngine;

namespace Events.Foundations
{
    /// <summary>
    /// 変換情報
    /// </summary>
    public class EventTransform
    {
        /// <summary>
        /// 
        /// </summary>
        public Pose Pose { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Vector3 Scaling { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventTransform()
        {
            Pose = new Pose();
            Scaling = Vector3.one;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="transform"></param>
        public EventTransform(Transform transform)
        {
            Pose = new Pose(transform.position, transform.rotation);
            Scaling = transform.localScale;
        }

        /// <summary>
        /// Transform コンポーネントに値をセット
        /// </summary>
        /// <param name="transform"></param>
        public void ToTransform(Transform transform)
        {
            transform.position = Pose.position;
            transform.rotation = Pose.rotation;
            transform.localScale = Scaling;
        }
    }

}
