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
        _commandExecutor = new Tsumugi.Unity.CommandExecutor(Text);

        var script = 
        "�����͏\�ꌎ�܂����̊֌W�@�Ƃ����̂̒��ɂ��ł��B" +
        "�ǂ���������b���͂Ƃ��Ƃ����̗͐�������܂ł�����Ă������͏����]�����܂��āA���X�ɂ͂���܂��ȂȂ������B" +
        "�G����肢���̂͂ǂ������Ԃ������܂������B" +
        "�͂����đ�X����ɐ����݂܂��������������邸��ׂ����肻�̋@��ꂩ�������ɑ΂��ĕs�i�]�ł�܂��܂��āA" +
        "���̍���������l�㗬�ɉ]���āA���c����̂̂ɐ��̒��̎��ɂނ���" +
        "���w���Ƃ������玄���l�����Ԏ��ɂ���悤�ɂ��Ƃɕs���͂��l����܂�����A" +
        "�����Ƃ��v����ɑ��Ƃ̂��邽���Ă��܂����̂��Ȃ�܂�����B" +
        "�������܂�����������̂��������h�ƂȂ�܂�����A���̋������͂��ꂽ�ĂƂ������E�����Ă���ł��ł��B";

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
