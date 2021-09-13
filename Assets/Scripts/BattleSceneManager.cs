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
    /// 現在の行動対象
    /// </summary>
    private Battler _currentBattler;

    /// <summary>
    /// 現在の攻撃対象
    /// </summary>
    private List<Battler> _currentTargets;

    /// <summary>
    /// ターン数
    /// </summary>
    private int _turnCount;

    /// <summary>
    /// 
    /// </summary>
    private enum StateEventId : int
    {
        Enter,
        BattlerChange,
        CommandSelect,
        AutoCommandSelect,
        EnemySelect,
        CharacterAction,
        EnemyAction,
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
        public BattlerData Data { get; set; }
    }

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Random.InitState(100);

        _stateMachine = new IceMilkTea.Core.ImtStateMachine<BattleSceneManager>(this);
        _stateMachine.AddAnyTransition<EnterState>((int)StateEventId.Enter);
        _stateMachine.AddAnyTransition<BattlerChangeState>((int)StateEventId.BattlerChange);
        _stateMachine.AddAnyTransition<CommandSelectState>((int)StateEventId.CommandSelect);
        _stateMachine.AddAnyTransition<EnemySelectState>((int)StateEventId.EnemySelect);
        _stateMachine.AddAnyTransition<CharacterActionState>((int)StateEventId.CharacterAction);
        _stateMachine.AddAnyTransition<AutoCommandSelectState>((int)StateEventId.AutoCommandSelect);
        _stateMachine.AddAnyTransition<EnemyActionState>((int)StateEventId.EnemyAction);
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
                Data = member
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
                Data = EnemyDatabase.GetEnemyData(EnemyDatabase.EnemyIds.Goblin),
            });
        }

        _allBattlers = new List<Battler>();
        _allBattlers.AddRange(_battlers);
        _allBattlers.AddRange(_enemys);

        _allBattlers.Sort((a, b) => 
        {
            return b.Status.Agility - a.Status.Agility;
        });

        _windows = new List<GameObject>();
        _windows.Add(GameObject.Find("CommandSelect"));
        _windows.Add(GameObject.Find("EnemySelect"));

        for (int index = 0; index < 4; ++index)
        {
            var number = index + 1;
            var button = GameObject.Find("Canvas/EnemySelect/Enemy" + number);
            if (_enemys.Count <= index)
            {
                button.SetActive(false);
                continue;
            }
            var text = GameObject.Find("Canvas/EnemySelect/Enemy" + number + "/Text");
            text.GetComponent<Text>().text = _enemys[index].Data.Name;
        }

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
        private float _poseTimeOffset;
        private bool _posePlayed;

        protected internal override void Enter()
        {
            var mainCamera = GameObject.Find("Main Camera");
            var playable = Resources.Load<PlayableAsset>("Timelines/Battles/MainCameraEnterField");

            var playableDirector = mainCamera.AddComponent<PlayableDirector>();
            playableDirector.playableAsset = playable;

            playableDirector.SetGenericBinding(playable.outputs.First().sourceObject, mainCamera.GetComponent<Animator>());
            playableDirector.stopped += (args) =>
            {
                Context._stateMachine.SendEvent((int)StateEventId.BattlerChange);
            };
            playableDirector.Play();

            _poseTimeOffset = 0;
            _posePlayed = false;
            Context._turnCount = 0;
        }

        protected internal override void Update()
        {
            if (!_posePlayed && _poseTimeOffset > 0.15f)
            {
                foreach (var evemy in Context._enemys)
                {
                    evemy.BattlerComponent.PlayAnimation("Attack");
                }
                _posePlayed = true;
            }
            _poseTimeOffset += Time.deltaTime;
        }
    }

    /// <summary>
    /// BattlerChange ステート
    /// </summary>
    private class BattlerChangeState : IceMilkTea.Core.ImtStateMachine<BattleSceneManager>.State
    {
        protected internal override void Enter()
        {
            // 最初のターン
            if (Context._currentBattler == null)
            {
                Context._currentBattler = Context._allBattlers[0];
            }
            else
            {
                var index = Context._allBattlers.FindIndex((battler) =>
                {
                    return battler == Context._currentBattler;
                });

                if (++index >= Context._allBattlers.Count)
                {
                    // ターン一巡
                    index = 0;
                    ++Context._turnCount;
                    Context.OnTurnChange();
                }

                Context._currentBattler = Context._allBattlers[index];
            }

            if (Context._currentBattler.BattlerComponent is BattlerPlayer)
            {
                Context._stateMachine.SendEvent((int)StateEventId.CommandSelect);
            }
            else
            {
                Context._stateMachine.SendEvent((int)StateEventId.AutoCommandSelect);
            }

            Context._currentTargets = new List<Battler>();
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
    /// AutoCommandSelect ステート
    /// </summary>
    private class AutoCommandSelectState : IceMilkTea.Core.ImtStateMachine<BattleSceneManager>.State
    {
        protected internal override void Enter()
        {
            // ここに攻撃パターンの AI を実装
            var index = (int)UnityEngine.Random.Range(0, Context._battlers.Count);
            Context._currentTargets.Add(Context._battlers[index]);
            Context._stateMachine.SendEvent((int)StateEventId.EnemyAction);
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

            for (int index = 0; index < Context._enemys.Count; ++index)
            {
                var target = Context._enemys[index];
                var button = GameObject.Find(enemySelect.name + "/Enemy" + (++index));
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnPress(button, target);
                });
            }
        }

        protected internal override void Exit()
        {
            Context._windows[(int)WindowId.EnemySelect].SetActive(false);
            Context._windows[(int)WindowId.CommandSelect].SetActive(false);
        }

        private void OnPress(GameObject button, Battler target)
        {
            switch (button.name)
            {
                case "Enemy1":
                    Context._stateMachine.ClearStack();
                    Context._stateMachine.SendEvent((int)StateEventId.CharacterAction);
                    Context._currentTargets.Add(target);
                    break;
            }

        }
    }

    /// <summary>
    /// CharacterAction ステート
    /// </summary>
    private class CharacterActionState : IceMilkTea.Core.ImtStateMachine<BattleSceneManager>.State
    {
        private bool _damageActionPlayed;
        private float _damageTimeOffset;

        protected internal override void Enter()
        {
            _damageActionPlayed = false;
            _damageTimeOffset = 0;
            Context._currentBattler.BattlerComponent.PlayAnimation("Attack");
        }

        protected internal override void Update()
        {
            if (!_damageActionPlayed)
            {
                if (_damageTimeOffset > 0.5f)
                {
                    foreach (var target in Context._currentTargets)
                    {
                        target.BattlerComponent.PlayAnimation("DamageLight");
                    }
                    _damageActionPlayed = true;
                }
            }
            else
            {
                foreach (var target in Context._currentTargets)
                {
                    if (target.BattlerComponent.IsPlayingAnimation("DamageLight"))
                        return;
                }
                Context._stateMachine.SendEvent((int)StateEventId.BattlerChange);
            }
            _damageTimeOffset += Time.deltaTime;
        }
    }

    /// <summary>
    /// EnemyAction ステート
    /// </summary>
    private class EnemyActionState : IceMilkTea.Core.ImtStateMachine<BattleSceneManager>.State
    {
        private bool _damageActionPlayed;
        private float _damageTimeOffset;

        protected internal override void Enter()
        {
            _damageActionPlayed = false;
            _damageTimeOffset = 0;
            Context._currentBattler.BattlerComponent.PlayAnimation("Attack");
        }

        protected internal override void Update()
        {
            if (!_damageActionPlayed)
            {
                if (_damageTimeOffset > 0.5f)
                {
                    foreach (var target in Context._currentTargets)
                    {
                        target.BattlerComponent.PlayAnimation("DamageLight");
                    }
                    _damageActionPlayed = true;
                }
            }
            else
            {
                foreach (var target in Context._currentTargets)
                {
                    if (target.BattlerComponent.IsPlayingAnimation("DamageLight"))
                        return;
                }
                Context._stateMachine.SendEvent((int)StateEventId.BattlerChange);
            }
            _damageTimeOffset += Time.deltaTime;
        }
    }

    /// <summary>
    /// ターン変更イベント
    /// </summary>
    private void OnTurnChange()
    {

    }
}
