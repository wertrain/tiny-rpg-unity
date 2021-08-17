using System.Collections;
using System.Collections.Generic;
using Events.Event;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldSceneManager : MonoBehaviour, Events.EventCallback
{
    /// <summary>
    /// 
    /// </summary>
    private Events.EventManager _eventManager;

    /// <summary>
    /// 
    /// </summary>
    private GameObject _followCamera;

    // Start is called before the first frame update
    void Start()
    {
        _eventManager = GetComponent<Events.EventManager>();
#if false
        var eventTree = _eventManager.SceneController.NodeTree;

        var rootNode = new Events.Event.Nodes.RootNode(eventTree);
        {
            var cameraNode = new Events.Event.Nodes.CameraNode(eventTree);
            {
                var clipNode = new Events.Event.Nodes.ClipNode(eventTree);
                {
                    var basicCameraClip = new Events.Event.Nodes.Clips.BasicCameraClip(clipNode);
                    clipNode.EventClip = basicCameraClip;
                    basicCameraClip.StartTime = 2.0f;
                    basicCameraClip.EndTime = 5.0f;
                    //basicCameraClip.AfterPose.position = new Vector3(0, 10, 0);
                    basicCameraClip.BeforePose.position = new Vector3(18.8719997f, 1.78799999f, 33.3120003f);
                    basicCameraClip.BeforePose.rotation = Quaternion.Euler(20.9046822f, 177.285477f, 357.955048f);
                    basicCameraClip.AfterPose.position = new Vector3(19.44f, 9.41f, 41f);
                    basicCameraClip.AfterPose.rotation = Quaternion.Euler(45, 183f, 0);
                }
                cameraNode.Children.Add(clipNode);
                clipNode.Parent = cameraNode;
            }
            rootNode.Children.Add(cameraNode);
            cameraNode.Parent = rootNode;
        }
        _eventManager.SceneController.CreateScene(rootNode);
        _eventManager.AddCallback(this);

        //_eventManager.gameObject.transform.position = Camera.main.transform.position;
        //_eventManager.gameObject.transform.rotation = Camera.main.transform.rotation;

        //_eventManager.Play();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("MainMenuScene");
        }

        _eventManager.UpdateEvent(Time.deltaTime);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneController"></param>
    public void OnStartEvent(EventSceneController sceneController)
    {
        _followCamera = GameObject.Find("PlayerFollowCamera");
        _followCamera.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneController"></param>
    public void OnEndEvent(EventSceneController sceneController)
    {
        _followCamera.SetActive(true);
    }
}
