using Events.Event.Nodes.Commons;
using UnityEngine;

namespace Events.Event.Nodes.Clips
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicCameraClip : EventClipBase
    {
        /// <summary>
        /// 開始トランスフォーム
        /// </summary>
        public Pose BeforePose;

        /// <summary>
        /// 終了トランスフォーム
        /// </summary>
        public Pose AfterPose;

        /// <summary>
        /// 
        /// </summary>
        private CameraNode.CameraPose _cameraPose;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BasicCameraClip(ClipNode clipNode)
            : base(clipNode)
        {
            BeforePose = new Pose();
            AfterPose = new Pose();

            BeforePose.position = AfterPose.position = Vector3.zero;
            BeforePose.rotation = AfterPose.rotation = Quaternion.identity;

            _cameraPose = new CameraNode.CameraPose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        override public bool FirstUpdate(float time)
        {
            var camera = Node.Parent as CameraNode;
            return camera.AddCamera(_cameraPose) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        override public void Update(float time)
        {
            var camera = EventManager.ActiveCamera;
            var transform = Node.Transform;

            var beforePosition = transform.Pose.position + BeforePose.position;
            var beforeRotation = transform.Pose.rotation * BeforePose.rotation;

            var afterPosition = transform.Pose.position + AfterPose.position;
            var afterRotation = transform.Pose.rotation * AfterPose.rotation;

            var ratio = GetTimeRatio(time);

            var pose = _cameraPose.Pose;
            pose.position = Vector3.Slerp(beforePosition, afterPosition, ratio);
            pose.rotation = Quaternion.Slerp(beforeRotation, afterRotation, ratio);
            _cameraPose.Pose = pose;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTime"></param>
        override public void LastUpdate(float time)
        {
            var camera = Node.Parent as CameraNode;
            camera.RemoveCamera(_cameraPose);
        }
    }
}
