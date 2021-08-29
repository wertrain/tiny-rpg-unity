using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class BattleSceneManager : MonoBehaviour
{
    /// <summary>
    /// ステートマシン
    /// </summary>
    private IceMilkTea.Core.ImtStateMachine<BattleSceneManager> _stateMachine;

    /// <summary>
    /// 
    /// </summary>
    private List<BattlerBase> _battlers;

    /// <summary>
    /// 
    /// </summary>
    private enum StateEventId : int
    {
        Enter,
        CommandSelect,
        Close,
        Max
    }

    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = new IceMilkTea.Core.ImtStateMachine<BattleSceneManager>(this);
        _stateMachine.AddAnyTransition<EnterState>((int)StateEventId.Enter);
        _stateMachine.AddAnyTransition<CommandSelectState>((int)StateEventId.CommandSelect);

        _stateMachine.SetStartState<EnterState>();
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Update();
    }

    /// <summary>
    /// Enter ステート
    /// </summary>
    private class EnterState : IceMilkTea.Core.ImtStateMachine<BattleSceneManager>.State
    {
        protected internal override void Enter()
        {
            var mainCamera = GameObject.Find("Main Camera");
            var playable = Resources.Load<PlayableAsset>("Timelines/Battles/MainCameraEnterField");

            var playableDirector = mainCamera.AddComponent<PlayableDirector>();
            playableDirector.playableAsset = playable;

            playableDirector.SetGenericBinding(playable.outputs.First().sourceObject, mainCamera.GetComponent<Animator>());
            playableDirector.Play();
        }

        protected internal override void Update()
        {
            Context._stateMachine.SendEvent((int)StateEventId.CommandSelect);
        }
    }

    /// <summary>
    /// CommandSelect ステート
    /// </summary>
    private class CommandSelectState : IceMilkTea.Core.ImtStateMachine<BattleSceneManager>.State
    {
        protected internal override void Enter()
        {

        }

        protected internal override void Update()
        {

        }
    }
}
