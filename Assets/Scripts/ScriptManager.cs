using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour
{
    /// <summary>
    /// テキストコンポーネント
    /// </summary>
    public Text Text;

    // Start is called before the first frame update
    void Start()
    {
        var script = "ああaaaaaaaaaああaaaaaaaaaああaaaaaaaaaああaaaaaaaaaああaaaaaaaaaああaaaaaaaaaああaaaaaaaaa";

        var interpreter = new Tsumugi.Interpreter();
        ///interpreter.OnPrintError += Interpreter_OnPrintError;
        interpreter.Executor = new Tsumugi.Unity.CommandExecutor(Text);
        interpreter.Execute(script);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
