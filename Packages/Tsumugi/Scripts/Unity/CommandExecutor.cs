using System;
using System.Collections;
using System.Collections.Generic;
using Tsumugi.Text.Executing;
using UnityEngine;

namespace Tsumugi.Unity
{
    public class CommandExecutor : Tsumugi.Text.Executing.ICommandExecutor
    {
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
            var defaultPosition = new Vector2(_textComponent.rectTransform.anchoredPosition.x, _textComponent.rectTransform.anchoredPosition.y);
            _elementUpdater.Add(
                new Func<float, bool>((float deltaTime) =>
                {
                    if ((time += deltaTime) < millisec)
                    {
                        _textComponent.rectTransform.anchoredPosition = new Vector2(
                            defaultPosition.x + 10, defaultPosition.y + 10);
                        return true;
                    }
                    _textComponent.rectTransform.anchoredPosition = defaultPosition;
                    return false;
                })
            );
        }

        public void Update(float deltaTime)
        {
            foreach (var updater in _elementUpdater.ToArray())
            {
                if (!updater(deltaTime))
                {
                    _elementUpdater.Remove(updater);
                }
            }

            _stateMachine.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public CommandExecutor(UnityEngine.UI.Text textComponent)
        {
            _textComponent = textComponent;
            _textComponent.text = string.Empty;

            _stateMachine = new StateMachine<CommandExecutor>(this);
            _stateMachine.AddAnyTransition<PrintingState>((int)StateEventId.Printing);
            _stateMachine.AddAnyTransition<WaitKayState>((int)StateEventId.WaitKay);
            _stateMachine.AddAnyTransition<WaitTimeState>((int)StateEventId.WaitTime);
            _stateMachine.AddAnyTransition<NewlineState>((int)StateEventId.Newline);
            _stateMachine.AddAnyTransition<ProcessedState>((int)StateEventId.Processed);
            _stateMachine.SetStartState<PrintingState>();
            _stateMachine.Update();

            _elementUpdater = new List<Func<float, bool>>();
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
                if (_time > 0.01f)
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

                if (Context.InputAnyKeys())
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
            protected internal override void Update()
            {
                if (Context.InputAnyKeys())
                {
                    Context._stateMachine.SendEvent(Context._activeText.Length == 0 ?
                        (int)StateEventId.Processed : (int)StateEventId.Printing);

                    Context._textComponent.text = string.Empty;
                }
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
        /// �e�L�X�g��\������R���|�[�l���g
        /// </summary>
        private UnityEngine.UI.Text _textComponent;

        /// <summary>
        /// �X�e�[�g�}�V��
        /// </summary>
        private StateMachine<CommandExecutor> _stateMachine;

        /// <summary>
        /// �X�V����
        /// </summary>
        private List<Func<float, bool>> _elementUpdater;
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
