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
        _commandExecutor = new Tsumugi.Unity.CommandExecutor(Text);

        var script = 
        "あいつは十一月まあその関係院というのの中にしでう。" +
        "どうも半分を話児はとうとうその力説たたらまでを解らてっうをは唱道云ったませて、元々にはするましななかった。" +
        "敵から願いたのはどうも時間をもちませだた。" +
        "はたして大森さんに説明憚また説明をしたずるずるべったりその機会それか公言をに対して不品評でんますませて、" +
        "この昨日も私か個人上流に云いて、岡田さんののに世の中の私にむしろ" +
        "お指導ともっから私他人をお返事にやつしようにことに不助力を考えんますから、" +
        "もっとも要するに卒業のあるたいてしまいだのがなるますある。" +
        "しかしまた今道をあるのも多少立派となりますから、この教授がはもつれたてという世界をしているですです。";

        var interpreter = new Tsumugi.Interpreter();
        ///interpreter.OnPrintError += Interpreter_OnPrintError;
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
