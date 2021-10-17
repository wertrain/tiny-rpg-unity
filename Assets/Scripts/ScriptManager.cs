using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour
{
    /// <summary>
    /// テキストコンポーネント [Require]
    /// </summary>
    public Text Text;

    /// <summary>
    /// 名前用テキストコンポーネント [Option]
    /// </summary>
    public Text NameText;

    /// <summary>
    /// メッセージウィンドウ [Option]
    /// </summary>
    public GameObject MessageWindow;

    /// <summary>
    /// 名前ウィンドウ [Option]
    /// </summary>
    public GameObject NameWindow;

    /// <summary>
    /// ページ送りを示す画像/オブジェクト [Option]
    /// </summary>
    public GameObject NextPageSymbol;

    /// <summary>
    /// 画像レイヤー [Option]
    /// </summary>
    public List<Image> ImageLayer;

    // Start is called before the first frame update
    void Start()
    {
        _commandExecutor = new Tsumugi.Unity.CommandExecutor(new Tsumugi.Unity.CommandExecutor.SetupParameter()
        {
            TextComponent = Text,
            NameTextComponent = NameText,
            MessageWindow = MessageWindow,
            NameWindow = NameWindow,
            NextPageSymbol = NextPageSymbol,
            ImageLayer = ImageLayer
        });

        var script =
        "あ[wait time=1000]い <size=50>largelyつは十[r]一月[name text=こういう]まあ[l]そ</size>の[quake time=8000]関[wq canskip=true]係院[name]というのの中にしでう。" +
        "どうも[image storage=Characters/Faces/YozoFace]半分を話児はとうとう[delay speed=0]その力説たたらまでを解らてっうをは唱道云ったませて、元々にはするましななかった。" +
        "敵から願いたのはどうも時間をもちませだた。" +
        "はたして大森さんに説明憚また説明をしたずるずるべったりその機会それか公言をに対して不品評でんますませて、" +
        "この昨日も私か個人上流に云いて、岡田さんののに世の中の私にむしろ" +
        "お指導ともっから私他人をお返事にやつしようにことに不助力を考えんますから、" +
        "もっとも要するに卒業のあるたいてしまいだのがなるますある。" +
        "しかしまた今道をあるのも多少立派となりますから、この教授がはもつれたてという世界をしているですです。";

        _interpreter = new Tsumugi.Interpreter();
        _interpreter.OnPrintError += Interpreter_OnPrintError;
        _interpreter.Executor = _commandExecutor;
        _interpreter.Parse(script);

        _interpreter.ExecuteCommand();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Interpreter_OnPrintError(object sender, string e)
    {
        Debug.Log(e);
    }

    // Update is called once per frame
    void Update()
    {
        if (_commandExecutor.IsProcessed())
        {
            _interpreter.ExecuteCommand();
        }

        _commandExecutor.Update(Time.deltaTime);
    }

    private Tsumugi.Interpreter _interpreter;

    private Tsumugi.Unity.CommandExecutor _commandExecutor;
}
