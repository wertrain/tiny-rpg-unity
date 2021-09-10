using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tsumugi.Text.Executing;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BattleSceneManager : MonoBehaviour
{
    /// <summary>
    /// ステートマシン
    /// </summary>
    private IceMilkTea.Core.ImtStateMachine<BattleSceneManager> _stateMachine;

    /// <summary>
    /// 
    /// </summary>
    private List<Battler> _battlers;

    /// <summary>
    /// 
    /// </summary>
    private List<Battler> _enemys;

    /// <summary>
    /// 
    /// </summary>
    private List<Battler> _allBattlers;

    /// <summary>
    /// 
    /// </summary>
    private List<GameObject> _windows;

    /// <summary>
    /// 
    /// </summary>
    private enum StateEventId : int
    {
        Enter,
        CommandSelect,
        EnemySelect,
        CharacterAction,
        Close,
        Max
    }

    /// <summary>
    /// 
    /// </summary>
    private enum WindowId : int
    {
        CommandSelect,
        EnemySelect,
        Max
    }

    /// <summary>
    /// 戦闘中のバトルキャラクター定義
    /// </summary>
    private class Battler
    {
        /// <summary>
        /// 
        /// </summary>
        public GameObject BattlerObject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BattlerBase BattlerComponent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CharacterData.Status Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CharacterData.Status BaseStatus { get; set; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = new IceMilkTea.Core.ImtStateMachine<BattleSceneManager>(this);
        _stateMachine.AddAnyTransition<EnterState>((int)StateEventId.Enter);
        _stateMachine.AddAnyTransition<CommandSelectState>((int)StateEventId.CommandSelect);
        _stateMachine.AddAnyTransition<EnemySelectState>((int)StateEventId.EnemySelect);
        _stateMachine.AddAnyTransition<CharacterActionState>((int)StateEventId.CharacterAction);
        _stateMachine.SetStartState<EnterState>();

        _battlers = new List<Battler>();
        for (int index = 0; index < MyGameManager.Instance.PartyMembers.Length; ++index)
        {
            var member = MyGameManager.Instance.PartyMembers[index];
            if (member == null) continue;
            var number = index + 1;
            _battlers.Add(new Battler()
            {
                BattlerObject = GameObject.Find("Characters/Character" + number),
                BattlerComponent = GameObject.Find("Characters/Character" + number).GetComponent<BattlerPlayer>(),
                Status = member.CurrentStatus,
                BaseStatus = member.BaseStatus
            });
        }

        _enemys = new List<Battler>();
        for (int index = 0; index < 1; ++index)
        {
            var number = index + 1;
            _enemys.Add(new Battler()
            {
                BattlerObject = GameObject.Find("Enemys/Enemy" + number),
                BattlerComponent = GameObject.Find("Enemys/Enemy" + number).GetComponent<BattlerEnemy>(),
                Status = EnemyDatabase.GetBaseStatus(EnemyDatabase.EnemyIds.Goblin),
                BaseStatus = EnemyDatabase.GetBaseStatus(EnemyDatabase.EnemyIds.Goblin),
            });
        }

        _allBattlers.Add(_battlers[0]);

        _windows = new List<GameObject>();
        _windows.Add(GameObject.Find("CommandSelect"));
        _windows.Add(GameObject.Find("EnemySelect"));

        _windows[(int)WindowId.CommandSelect].SetActive(false);
        _windows[(int)WindowId.EnemySelect].SetActive(false);
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
            var commandSelect = Context._windows[(int)WindowId.CommandSelect];
            commandSelect.SetActive(true);
            var position = commandSelect.transform.position;
            var defaultX = position.x;
            position.x = -400.0f;
            commandSelect.transform.position = position;

            LeanTween.moveX(commandSelect, defaultX, 0.5f).setOnComplete(() =>
            {
                var allButtons = new List<string>{ "AttackButton", "SkillButton", "DefenseButton" };
                foreach(var buttonName in allButtons)
                {
                    var button = GameObject.Find(commandSelect.name + "/" + buttonName);
                    button.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        OnPress(button);
                    });
                }
            });
        }

        protected internal override void Update()
        {

        }

        protected internal override void Exit()
        {
            var commandSelect = Context._windows[(int)WindowId.CommandSelect];

            var allButtons = new List<string> { "AttackButton", "SkillButton", "DefenseButton" };
            foreach (var buttonName in allButtons)
            {
                var button = GameObject.Find(commandSelect.name + "/" + buttonName);
                button.GetComponent<Button>().onClick.RemoveAllListeners();
            }

            Context._windows[(int)WindowId.CommandSelect].SetActive(false);
        }

        private void OnPress(GameObject button)
        {
            switch (button.name)
            {
                case "AttackButton":
                    Context._stateMachine.PushState();
                    Context._stateMachine.SendEvent((int)StateEventId.EnemySelect);
                    break;
            }

        }
    }

    /// <summary>
    /// EnemySelect ステート
    /// </summary>
    private class EnemySelectState : IceMilkTea.Core.ImtStateMachine<BattleSceneManager>.State
    {
        protected internal override void Enter()
        {
            var enemySelect = Context._windows[(int)WindowId.EnemySelect];
            enemySelect.SetActive(true);

            for (int index = 0; index < 4; ++index)
            {
                var button = GameObject.Find(enemySelect.name + "/Enemy" + (++index));
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    int enemyIndex = index;
                    OnPress(button, enemyIndex);
                });
            }
        }

        protected internal override void Exit()
        {
            Context._windows[(int)WindowId.EnemySelect].SetActive(false);
            Context._windows[(int)WindowId.CommandSelect].SetActive(false);
        }

        private void OnPress(GameObject button, int index)
        {
            switch (button.name)
            {
                case "Enemy1":
                    Context._stateMachine.ClearStack();
                    Context._stateMachine.SendEvent((int)StateEventId.CharacterAction);
                    Debug.Log(button.name);
                    break;
            }

        }
    }

    /// <summary>
    /// CharacterAction ステート
    /// </summary>
    private class CharacterActionState : IceMilkTea.Core.ImtStateMachine<BattleSceneManager>.State
    {
        private float _damegeTimeOffset;

        protected internal override void Enter()
        {
            _damegeTimeOffset = 0;
            Context._battlers[0].BattlerComponent.PlayAnimation("Attack");
        }

        protected internal override void Update()
        {
            _damegeTimeOffset += Time.deltaTime;
            if (_damegeTimeOffset > 0.5f)
            {
                Context._enemys[0].BattlerComponent.PlayAnimation("DamageLight");
                Context._stateMachine.SendEvent((int)StateEventId.CommandSelect);
            }
        }
    }
}
