using System;
using System.Collections;
using System.Collections.Generic;
using Tsumugi.Text.Executing;
using UnityEngine;

namespace Tsumugi.Unity
{
    public class CommandExecutor : ICommandExecutor
    {
        /// <summary>
        ///  �������葬�x�����l
        /// </summary>
        private static readonly float DefaultDelaySpeed = 0.01f;

        public void PrintText(string text)
        {
            _activeText = text.Replace(" ", "\u00A0"); // �m�[�u���[�N�X�y�[�X�ɒu��������

            _stateMachine.SendEvent((int)StateEventId.Printing);
        }

        public void SetDefaultFont(Tsumugi.Text.Executing.Font font)
        {
            throw new System.NotImplementedException();
        }

        public void SetFont(Tsumugi.Text.Executing.Font font)
        {
            throw new System.NotImplementedException();
        }

        public void StartNewLine()
        {
            _stateMachine.SendEvent((int)StateEventId.Newline);
        }

        public void StartNewPage()
        {
            _textComponent.text = string.Empty;
        }

        public void WaitAnyKey()
        {
            _stateMachine.SendEvent((int)StateEventId.WaitKay);
        }

        public void WaitTime(int millisec)
        {
            _waitTime = millisec / 1000.0f;

            _stateMachine.SendEvent((int)StateEventId.WaitTime);
        }

        public void Quake(int millisec, int powerH, int powerV)
        {
            float time = 0;
            float seconds = millisec / 1000.0f;

            var defaultTextPosition = new Vector2(_textComponent.rectTransform.anchoredPosition.x, _textComponent.rectTransform.anchoredPosition.y);

            var defaultTextWindowPosition = _messageWindow == null ? Vector2.zero : _messageWindow.GetComponent<RectTransform>().anchoredPosition;
            var defaultNamePosition = _nameTextComponent == null ? Vector2.zero : _nameTextComponent.GetComponent<RectTransform>().anchoredPosition;
            var defaultNameWindowPosition = _nameWindow == null ? Vector2.zero : _nameWindow.GetComponent<RectTransform>().anchoredPosition;

            _elementUpdater.Add(
                (UpdaterTypes.Quake, new Func<float, bool>((float deltaTime) =>
                {
                    if ((time += deltaTime) < seconds)
                    {
                        var ns = 10.0f;
                        var nc = 2.0f;

                        var t = Time.time * ns;
                        var nx = Mathf.PerlinNoise(t, t + 5.0f) * (powerH * nc);
                        var ny = Mathf.PerlinNoise(t + 10.0f, t + 15.0f) * (powerV * nc);

                        nx = nx * 0.5f;
                        ny = ny * 0.5f;

                        if (UnityEngine.Random.Range(0, 2) > 0) nx = nx * -1;
                        if (UnityEngine.Random.Range(0, 2) > 0) ny = ny * -1;

                        _textComponent.rectTransform.anchoredPosition = new Vector2(defaultTextPosition.x + nx, defaultTextPosition.y + ny);

                        if (_messageWindow != null) _messageWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultTextWindowPosition.x + nx, defaultTextWindowPosition.y + ny);
                        if (_nameTextComponent != null) _nameTextComponent.GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultNamePosition.x + nx, defaultNamePosition.y + ny);
                        if (_nameWindow != null) _nameWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultNameWindowPosition.x + nx, defaultNameWindowPosition.y + ny);

                        return true;
                    }

                    _textComponent.rectTransform.anchoredPosition = defaultTextPosition;
                    if (_messageWindow != null) _messageWindow.GetComponent<RectTransform>().anchoredPosition = defaultTextWindowPosition;
                    if (_nameTextComponent != null) _nameTextComponent.GetComponent<RectTransform>().anchoredPosition = defaultNamePosition;
                    if (_nameWindow != null) _nameWindow.GetComponent<RectTransform>().anchoredPosition = defaultNameWindowPosition;

                    return false;
                }))
            );
        }

        public void WaitQuake(bool canSkip)
        {
            _enableCancel = canSkip;
            _stateMachine.SendEvent((int)StateEventId.WaitQuake);
        }

        public void Delay(int speed)
        {
            _delayTime = speed / 1000.0f;
        }

        public void SetName(string text)
        {
            if (_nameTextComponent != null)
            {
                _nameTextComponent.text = text;
            }
            _nameWindow?.SetActive(!string.IsNullOrEmpty(text));
        }

        public void SetImage(Image image)
        {
            if (_imageLayer.Count <= image.Layer) return;

            var color = Color.white;
            ColorUtility.TryParseHtmlString(image.Color, out color);

            _imageLayer[image.Layer].sprite = Resources.Load<Sprite>(image.Storage);
            Debug.Log(_imageLayer[image.Layer].sprite);
            _imageLayer[image.Layer].color = color;

            //if (!_allAssetBundles.ContainsKey(image.Storage))
            //{
            //    _allAssetBundles.Add(image.Storage, asset);
            //}

        }

        public void Update(float deltaTime)
        {
            foreach (var updater in _elementUpdater.ToArray())
            {
                if (!updater.Item2(deltaTime))
                {
                    _elementUpdater.Remove(updater);
                }
            }

            _stateMachine.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        public class SetupParameter
        {
            /// <summary>
            /// �X�N���v�g���e��\������e�L�X�g�R���|�[�l���g�i�K�{�j
            /// </summary>
            public UnityEngine.UI.Text TextComponent { get; set; }

            /// <summary>
            /// ���O��\������e�L�X�g�R���|�[�l���g�i�C�Ӂj
            /// </summary>
            public UnityEngine.UI.Text NameTextComponent { get; set; }

            /// <summary>
            /// ���b�Z�[�W�E�B���h�E�I�u�W�F�N�g�i�C�Ӂj
            /// </summary>
            public GameObject MessageWindow { get; set; }

            /// <summary>
            /// ���O�E�B���h�E�I�u�W�F�N�g�i�C�Ӂj
            /// </summary>
            public GameObject NameWindow { get; set; }

            /// <summary>
            /// ���̃y�[�W�������摜/�I�u�W�F�N�g�i�C�Ӂj
            /// </summary>
            public GameObject NextPageSymbol { get; set; }

            /// <summary>
            /// �摜���C���[�i�C�Ӂj
            /// </summary>
            public List<UnityEngine.UI.Image> ImageLayer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public CommandExecutor(SetupParameter param)
        {
            _textComponent = param.TextComponent;
            _nameTextComponent = param.NameTextComponent;
            _messageWindow = param.MessageWindow;
            _nameWindow = param.NameWindow;
            _nextPageSymbol = param.NextPageSymbol;
            _imageLayer = param.ImageLayer;

            _textComponent.text = string.Empty;
            if (_nameTextComponent != null) _nameTextComponent.text = string.Empty;

            _stateMachine = new StateMachine<CommandExecutor>(this);
            _stateMachine.AddAnyTransition<PrintingState>((int)StateEventId.Printing);
            _stateMachine.AddAnyTransition<WaitKayState>((int)StateEventId.WaitKay);
            _stateMachine.AddAnyTransition<WaitTimeState>((int)StateEventId.WaitTime);
            _stateMachine.AddAnyTransition<WaitQuakeState>((int)StateEventId.WaitQuake);
            _stateMachine.AddAnyTransition<NewlineState>((int)StateEventId.Newline);
            _stateMachine.AddAnyTransition<ProcessedState>((int)StateEventId.Processed);
            _stateMachine.SetStartState<PrintingState>();
            _stateMachine.Update();

            _delayTime = DefaultDelaySpeed;
            _nameWindow?.SetActive(false);
            _nextPageSymbol?.SetActive(false);

            _elementUpdater = new List<(UpdaterTypes, Func<float, bool>)>();
            _allAssetBundles = new Dictionary<string, AssetBundle>();
        }

        /// <summary>
        /// �������������Ă��邩�ǂ���
        /// </summary>
        public bool IsProcessed()
        {
            return _stateMachine.IsCurrentState<ProcessedState>();
        }

        /// <summary>
        /// �L�[���͑҂��̏�Ԃ��ǂ���
        /// </summary>
        /// <returns></returns>
        public bool IsKeyWait()
        {
            return _stateMachine.IsCurrentState<WaitKayState>();
        }

        /// <summary>
        /// �y�[�W����A�܂��̓y�[�W���̕����\��
        /// </summary>
        /// <returns></returns>
        private bool InputAnyKeys()
        {
            return Input.anyKeyDown;
        }

        /// <summary>
        /// Printing �X�e�[�g
        /// </summary>
        private class PrintingState : StateMachine<CommandExecutor>.State
        {
            protected internal override void Enter()
            {
                _index = 1;
                _text = Context._textComponent.text;
            }

            protected internal override void Update()
            {
                // delayTime �� 0 �̏ꍇ�͑҂����ԂȂ��ŕ\��������̂Ƃ��Ă���
                bool noWait = Context._delayTime == 0;

                if (!noWait && _time > Context._delayTime)
                {
                    _time = 0;

                    if (++_index > Context._activeText.Length)
                    {
                        Context._activeText = string.Empty;
                        Context._stateMachine.SendEvent((int)StateEventId.Processed);
                        return;
                    }
                }
                _time += Time.deltaTime;

                if (Context.InputAnyKeys() || noWait)
                {
                    int pass = _index;
                    while(!TextExtension.IsOverflowingVerticle(Context._textComponent))
                    {
                        Context._textComponent.text = _text + Context._activeText.Substring(0, pass);

                        if (++pass >= Context._activeText.Length)
                            break;
                    }
                    var nextIndex = pass - 1;
                    Context._activeText = Context._activeText.Substring(nextIndex, Context._activeText.Length - nextIndex);
                    Context._stateMachine.SendEvent((int)StateEventId.WaitKay);
                }
                else
                {
                    Context._textComponent.text = _text + Context._activeText.Substring(0, _index);
                    if (TextExtension.IsOverflowingVerticle(Context._textComponent))
                    {
                        var nextIndex = _index - 1;
                        Context._activeText = Context._activeText.Substring(nextIndex, Context._activeText.Length - nextIndex);
                        Context._stateMachine.SendEvent((int)StateEventId.WaitKay);
                    }
                }
            }

            /// <summary>
            /// ���̕�����
            /// </summary>
            private string _text;

            /// <summary>
            /// �����̕\���C���f�b�N�X
            /// </summary>
            private int _index;

            /// <summary>
            /// ����
            /// </summary>
            private float _time;
        }

        /// <summary>
        /// WaitKay �X�e�[�g
        /// </summary>
        private class WaitKayState : StateMachine<CommandExecutor>.State
        {
            protected internal override void Enter()
            {
                Context._nextPageSymbol?.SetActive(true);
            }

            protected internal override void Update()
            {
                if (Context.InputAnyKeys())
                {
                    Context._stateMachine.SendEvent(Context._activeText.Length == 0 ?
                        (int)StateEventId.Processed : (int)StateEventId.Printing);

                    Context._textComponent.text = string.Empty;
                }
            }

            protected internal override void Exit()
            {
                Context._nextPageSymbol?.SetActive(false);
            }
        }

        /// <summary>
        /// WaitTime �X�e�[�g
        /// </summary>
        private class WaitTimeState : StateMachine<CommandExecutor>.State
        {
            protected internal override void Enter()
            {
                _time = 0;
            }

            protected internal override void Update()
            {
                if ((_time += Time.deltaTime) > Context._waitTime)
                {
                    Context._stateMachine.SendEvent((int)StateEventId.Processed);
                }
            }

            /// <summary>
            /// ����
            /// </summary>
            private float _time;
        }

        /// <summary>
        /// WaitQuake �X�e�[�g
        /// </summary>
        private class WaitQuakeState : StateMachine<CommandExecutor>.State
        {
            protected internal override void Update()
            {
                // �X�L�b�v�\�̎��́A�X�L�b�v������s��
                if (Context._enableCancel && Context.InputAnyKeys())
                {
                    Context._elementUpdater.RemoveAll((updater) =>
                    {
                        return updater.Item1 == UpdaterTypes.Quake;
                    });
                }
                else
                {
                    foreach (var (name, updater) in Context._elementUpdater)
                    {
                        if (name == UpdaterTypes.Quake) return;
                    }
                }

                Context._stateMachine.SendEvent((int)StateEventId.Processed);
            }
        }

        /// <summary>
        /// Newline �X�e�[�g
        /// </summary>
        private class NewlineState : StateMachine<CommandExecutor>.State
        {
            protected internal override void Enter()
            {
                Context._textComponent.text += System.Environment.NewLine;
                Context._stateMachine.SendEvent((int)StateEventId.Processed);
            }
        }

        /// <summary>
        /// Processed �X�e�[�g
        /// </summary>
        private class ProcessedState : StateMachine<CommandExecutor>.State
        {
            protected internal override void Enter()
            {
                //Context._textComponent.text = string.Empty;
            }
        }

        /// <summary>
        /// �X�e�[�g ID
        /// </summary>
        private enum StateEventId : int
        {
            Printing,
            WaitKay,
            WaitTime,
            WaitQuake,
            Newline,
            Processed,
            Max
        }

        /// <summary>
        /// �v�f�X�V�f���Q�[�^�[
        /// </summary>
        delegate void ElementUpdater();

        /// <summary>
        /// �\���������̕�����
        /// </summary>
        private string _activeText;

        /// <summary>
        /// WaitTime �X�e�[�g�̑ҋ@���Ԃ��i�[
        /// </summary>
        private float _waitTime;

        /// <summary>
        /// �������莞��
        /// </summary>
        private float _delayTime;

        /// <summary>
        /// �L�����Z���\��
        /// </summary>
        private bool _enableCancel;

        /// <summary>
        /// �e�L�X�g��\������R���|�[�l���g
        /// </summary>
        private UnityEngine.UI.Text _textComponent;

        /// <summary>
        /// ���O��\������e�L�X�g�R���|�[�l���g
        /// </summary>
        public UnityEngine.UI.Text _nameTextComponent;

        /// <summary>
        /// ���b�Z�[�W�E�B���h�E�I�u�W�F�N�g
        /// </summary>
        public GameObject _messageWindow;

        /// <summary>
        /// ���O�E�B���h�E�I�u�W�F�N�g
        /// </summary>
        public GameObject _nameWindow;

        /// <summary>
        /// ���̃y�[�W�ɐi�߂邱�Ƃ������摜/�I�u�W�F�N�g
        /// </summary>
        private GameObject _nextPageSymbol;

        /// <summary>
        /// �C���[�W���C���[
        /// </summary>
        private List<UnityEngine.UI.Image> _imageLayer;

        /// <summary>
        /// �X�e�[�g�}�V��
        /// </summary>
        private StateMachine<CommandExecutor> _stateMachine;

        /// <summary>
        /// ���[�h�������ׂẴA�Z�b�g�o���h��
        /// </summary>
        private Dictionary<string, AssetBundle> _allAssetBundles;

        /// <summary>
        /// �X�V�����̃^�C�v
        /// </summary>
        private enum UpdaterTypes : int
        {
            Quake,
        }

        /// <summary>
        /// �X�V����
        /// </summary>
        private List<(UpdaterTypes, Func<float, bool>)> _elementUpdater;
    }

    /// <summary>
    /// �e�L�X�g�g��
    /// </summary>
    public class TextExtension
    {
        /// <summary>
        /// Returns true when the Text object contains more lines of text than will fit in the text container vertically
        /// </summary>
        /// <returns></returns>
        public static bool IsOverflowingVerticle(UnityEngine.UI.Text text)
        {
            return UnityEngine.UI.LayoutUtility.GetPreferredHeight(text.rectTransform) > GetCalculatedPermissibleHeight(text);
        }

        private static float GetCalculatedPermissibleHeight(UnityEngine.UI.Text text)
        {
            if (cachedCalculatedPermissibleHeight != -1) return cachedCalculatedPermissibleHeight;

            cachedCalculatedPermissibleHeight = text.gameObject.GetComponent<RectTransform>().rect.height;
            return cachedCalculatedPermissibleHeight;
        }
        private static float cachedCalculatedPermissibleHeight = -1;

        /// <summary>
        /// Returns true when the Text object contains more character than will fit in the text container horizontally
        /// </summary>
        /// <returns></returns>
        public static bool IsOverflowingHorizontal(UnityEngine.UI.Text text)
        {
            return UnityEngine.UI.LayoutUtility.GetPreferredWidth(text.rectTransform) > GetCalculatedPermissibleHeight(text);
        }

        private static float GetCalculatedPermissibleWidth(UnityEngine.UI.Text text)
        {
            if (cachedCalculatedPermissiblWidth != -1) return cachedCalculatedPermissiblWidth;

            cachedCalculatedPermissiblWidth = text.gameObject.GetComponent<RectTransform>().rect.width;
            return cachedCalculatedPermissiblWidth;
        }

        private static float cachedCalculatedPermissiblWidth = -1;

    }
}
