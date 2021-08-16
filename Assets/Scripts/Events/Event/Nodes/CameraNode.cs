using Events.Event.Nodes.Commons;
using System.Collections.Generic;
using UnityEngine;

namespace Events.Event.Nodes
{
    /// <summary>
    /// 
    /// </summary>
    public class CameraNode : EventNodeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public class CameraPose
        {
            /// <summary>
            /// 
            /// </summary>
            public Pose Pose;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CameraPose()
            {
                Pose = new Pose();
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CameraPose(Transform transform) : this()
            {
                Pose.position = transform.position;
                Pose.rotation = transform.rotation;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<CameraPose> _cameras;

        /// <summary>
        /// 
        /// </summary>
        private CameraPose _defaultCameraPose;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CameraNode(EventNodeTree nodeTree)
            : base(nodeTree)
        {
            _cameras = new List<CameraPose>();

            var camera = EventManager.ActiveCamera;
            _defaultCameraPose = new CameraPose(camera.gameObject.transform);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pose"></param>
        /// <returns></returns>
        public int AddCamera(CameraPose pose)
        {
            _cameras.Add(pose);
            return _cameras.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pose"></param>
        public void RemoveCamera(CameraPose pose)
        {
            _cameras.Remove(pose);

            if (_cameras.Count == 0)
            {
                var camera = EventManager.ActiveCamera;
                camera.gameObject.transform.position = _defaultCameraPose.Pose.position;
                camera.gameObject.transform.rotation = _defaultCameraPose.Pose.rotation;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        public override void UpdateAfterChildren(float time)
        {
            if (_cameras.Count > 0)
            {
                var camera = EventManager.ActiveCamera;

                var currentPose = _cameras[_cameras.Count - 1];
                camera.gameObject.transform.position = currentPose.Pose.position;
                camera.gameObject.transform.rotation = currentPose.Pose.rotation;
            }
        }
    }
}
