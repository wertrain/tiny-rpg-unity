using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// ステートマシン
    /// </summary>
    private IceMilkTea.Core.ImtStateMachine<MainMenuController> _stateMachine;

    /// <summary>
    /// 
    /// </summary>
    private enum StateEventId : int
    {
        Idle,
        Open,
        Close,
        Max
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        _stateMachine = new IceMilkTea.Core.ImtStateMachine<MainMenuController>(this);
        _stateMachine.AddAnyTransition<IdleState>((int)StateEventId.Idle);
        _stateMachine.AddAnyTransition<OpenState>((int)StateEventId.Open);
        _stateMachine.AddAnyTransition<CloseState>((int)StateEventId.Close);
        _stateMachine.SetStartState<OpenState>();
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        _stateMachine.Update();
    }

    /// <summary>
    /// Idle ステート
    /// </summary>
    private class IdleState : IceMilkTea.Core.ImtStateMachine<MainMenuController>.State
    {
        protected internal override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Context._stateMachine.SendEvent((int)StateEventId.Close);
            }
        }
    }

    /// <summary>
    /// Open ステート
    /// </summary>
    private class OpenState : IceMilkTea.Core.ImtStateMachine<MainMenuController>.State
    {
        protected internal override void Enter()
        {
            Context.Open();
        }

        protected internal override void Update()
        {
            if (LeanTween.tweensRunning == 0)
            {
                Context._stateMachine.SendEvent((int)StateEventId.Idle);
            }
        }
    }

    /// <summary>
    /// Close ステート
    /// </summary>
    private class CloseState : IceMilkTea.Core.ImtStateMachine<MainMenuController>.State
    {
        protected internal override void Enter()
        {
            Context.Close();
        }

        protected internal override void Update()
        {
            if (LeanTween.tweensRunning == 0)
            {
                SceneManager.UnloadSceneAsync("MainMenuScene");
            }
        }
    }

    void Open()
    {
        var statusWindow = GameObject.Find("StatusWindow");
        {
            var position = statusWindow.gameObject.transform.position;
            var defaultY = position.y;
            position.y = 1000.0f;
            statusWindow.gameObject.transform.position = position;

            var rectTransform = statusWindow.GetComponent<RectTransform>();
            var localScale = rectTransform.localScale;
            var defaultScale = localScale;
            rectTransform.localScale = new Vector3(0.01f, defaultScale.y, defaultScale.z);
            LeanTween.moveY(statusWindow, defaultY, 0.3f).setOnComplete(() =>
            {
                LeanTween.scale(rectTransform, defaultScale, 0.2f).setOnComplete(() =>
                {

                });
            });
        }

        var buttons = new List<string> { "ItemButton", "SkillButton", "EquipmentButton", "StatusButton" };
        for (var index = 0; index < buttons.Count; ++index)
        {
            var button = GameObject.Find(buttons[index]);
            var position = button.gameObject.transform.position;
            var defaultX = position.x;
            position.x = 1600.0f;
            button.gameObject.transform.position = position;
            LeanTween.moveX(button, defaultX, 0.3f).setEase(LeanTweenType.easeInQuad).setDelay(0.15f * index).setOnComplete(() =>
            {

            });
        }
    }

    void Close()
    {
        var statusWindow = GameObject.Find("StatusWindow");
        {
            var position = statusWindow.gameObject.transform.position;

            var rectTransform = statusWindow.GetComponent<RectTransform>();
            var currentScale = rectTransform.localScale;
            LeanTween.scale(rectTransform, new Vector3(0.01f, currentScale.y, currentScale.z), 0.2f).setOnComplete(() =>
            {
                LeanTween.moveY(statusWindow, 1000.0f, 0.3f).setOnComplete(() =>
                {

                });
            });
        }

        var buttons = new List<string> { "ItemButton", "SkillButton", "EquipmentButton", "StatusButton" };
        for (var index = 0; index < buttons.Count; ++index)
        {
            var button = GameObject.Find(buttons[index]);
            LeanTween.moveX(button, 1600.0f, 0.3f).setEase(LeanTweenType.easeInQuad).setDelay(0.15f * index).setOnComplete(() =>
            {

            });
        }
    }
}
