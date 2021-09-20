using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour
{
    /// <summary>
    /// �e�L�X�g�R���|�[�l���g
    /// </summary>
    public Text Text;

    // Start is called before the first frame update
    void Start()
    {
        var script = "����aaaaaaaaa����aaaaaaaaa����aaaaaaaaa����aaaaaaaaa����aaaaaaaaa����aaaaaaaaa����aaaaaaaaa";

        var interpreter = new Tsumugi.Interpreter();
        ///interpreter.OnPrintError += Interpreter_OnPrintError;
        _commandExecutor = new Tsumugi.Unity.CommandExecutor(Text);
        interpreter.Executor = _commandExecutor;
        interpreter.Execute(script);
    }

    // Update is called once per frame
    void Update()
    {
        _commandExecutor.Update(Time.deltaTime);
    }

    private Tsumugi.Unity.CommandExecutor _commandExecutor;
}
