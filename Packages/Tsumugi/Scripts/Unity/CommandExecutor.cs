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
            _activeText = text.Replace(" ", "\u00A0"); // ノーブレークスペース
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
            throw new System.NotImplementedException();
        }

        public void StartNewPage()
        {
            throw new System.NotImplementedException();
        }

        public void WaitAnyKey()
        {
            throw new System.NotImplementedException();
        }

        public void WaitTime(int millisec)
        {
            throw new System.NotImplementedException();
        }

        public void Update(float deltaTime)
        {
            _stateMachine.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public CommandExecutor(UnityEngine.UI.Text textComponent)
        {
            _textComponent = textComponent;

            _stateMachine = new StateMachine<CommandExecutor>(this);
            _stateMachine.AddAnyTransition<PrintingState>((int)StateEventId.Printing);
            _stateMachine.AddAnyTransition<WaitKayState>((int)StateEventId.WaitKay);
            _stateMachine.AddAnyTransition<ProcessedState>((int)StateEventId.Processed);
            _stateMachine.SetStartState<PrintingState>();
        }

        /// <summary>
        /// Printing ステート
        /// </summary>
        private class PrintingState : StateMachine<CommandExecutor>.State
        {
            protected internal override void Enter()
            {
                _index = 1;
            }

            protected internal override void Update()
            {
                if (_time > 0.01f)
                {
                    _time = 0;

                    if (++_index >= Context._activeText.Length)
                    {
                        Context._activeText = string.Empty;
                        Context._stateMachine.SendEvent((int)StateEventId.WaitKay);
                        return;
                    }
                }
                _time += Time.deltaTime;

                Context._textComponent.text = Context._activeText.Substring(0, _index);
                if (TextExtension.IsOverflowingVerticle(Context._textComponent))
                {
                    var nextIndex = _index - 1;
                    Context._activeText = Context._activeText.Substring(nextIndex, Context._activeText.Length - nextIndex);
                    Context._stateMachine.SendEvent((int)StateEventId.WaitKay);
                }
            }

            private int _index;

            private float _time;
        }

        /// <summary>
        /// WaitKay ステート
        /// </summary>
        private class WaitKayState : StateMachine<CommandExecutor>.State
        {
            protected internal override void Update()
            {
                if (Input.anyKey)
                {
                    Context._stateMachine.SendEvent(Context._activeText.Length == 0 ?
                        (int)StateEventId.Processed : (int)StateEventId.Printing);
                }
            }
        }

        /// <summary>
        /// Processed ステート
        /// </summary>
        private class ProcessedState : StateMachine<CommandExecutor>.State
        {
            protected internal override void Enter()
            {
                Context._textComponent.text = string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private enum StateEventId : int
        {
            Printing,
            WaitKay,
            Processed,
            Max
        }

        /// <summary>
        /// 
        /// </summary>
        private string _activeText;

        /// <summary>
        /// テキストを表示するコンポーネント
        /// </summary>
        private UnityEngine.UI.Text _textComponent;

        /// <summary>
        /// 
        /// </summary>
        private StateMachine<CommandExecutor> _stateMachine;
    }

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
