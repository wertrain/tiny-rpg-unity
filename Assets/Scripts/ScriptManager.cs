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

        var script = "手も足も出ないような 悩みに縛られて" +
            "ひとりきりむりやり酔っぱらって アルバムを抱いて寝た" +
            "目が覚めれば気分が悪い それだけで 何も変わってないね やっぱり" +
            "" +
            "逃がさないで 逃げないで 胸の痛みと手をつないで" +
            "明日を迎えよう" +
            "イヤな問題 大損害 避けて通る人生なら論外" +
            "生きてるからしょうがない" +
            "シンパイナイモンダイナイナイナイザッツライフイッツオーライ" +
            "" +
            "窓の外すがすがしく 晴れてれば晴れてるほど" +
            "哀しくなるのはとても寂しいことだと思います" +
            "僅かな月日で積み上げた 幻を後生大事に拝むのはもうやめた" +
            "" +
            "逃がさないで 逃げないで 胸の痛みと手をつないで" +
            "明日を迎えよう" +
            "イヤな問題 大損害 避けて通る人生なら論外" +
            "生きてるからしょうがない" +
            "シンパイナイモンダイナイナイナイザッツライフ" +
            "" +
            "甘えたいね ぬぎたいね ツラいおもいしないのはダサイね" +
            "素通りしたいけど" +
            "イカす男女 ナレるでしょう 切り抜ければ待ってる次のショウ" +
            "トラブルは素晴らしいチャンス" +
            "シンパイナイモンダイナイナイナイザッツライフイッツオーライ" +
            "" +
            "逃がさないで 逃げないで 胸の痛みと手をつないで" +
            "明日を迎えよう" +
            "イヤな問題 大損害 避けて通る人生なら論外" +
            "生きてるからしょうがない" +
            "シンパイナイモンダイナイナイナイザッツライフ" +
            "" +
            "楽になりたい 泣き出したい いつか今を軽く笑い飛ばしたい" +
            "なんとかなるよ" +
            "イカす男女 ナレるでしょう 切り抜ければ待ってる次のショウ" +
            "トラブルは素晴らしいチャンス" +
            "シンパイナイモンダイナイナイナイザッツライフイッツオーライ";

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
