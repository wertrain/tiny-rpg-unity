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
            _textComponent.text = text;
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

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public CommandExecutor(UnityEngine.UI.Text text)
        {
            _textComponent = text;
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
        /// テキストを表示するコンポーネント
        /// </summary>
        private UnityEngine.UI.Text _textComponent;

        /// <summary>
        /// 
        /// </summary>
        private StateMachine<CommandExecutor> _stateMachine;
    }
}
