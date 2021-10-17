using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour
{
    /// <summary>
    /// �e�L�X�g�R���|�[�l���g [Require]
    /// </summary>
    public Text Text;

    /// <summary>
    /// ���O�p�e�L�X�g�R���|�[�l���g [Option]
    /// </summary>
    public Text NameText;

    /// <summary>
    /// ���b�Z�[�W�E�B���h�E [Option]
    /// </summary>
    public GameObject MessageWindow;

    /// <summary>
    /// ���O�E�B���h�E [Option]
    /// </summary>
    public GameObject NameWindow;

    /// <summary>
    /// �y�[�W����������摜/�I�u�W�F�N�g [Option]
    /// </summary>
    public GameObject NextPageSymbol;

    /// <summary>
    /// �摜���C���[ [Option]
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
        "��[wait time=1000]�� <size=50>largely�͏\[r]�ꌎ[name text=��������]�܂�[l]��</size>��[quake time=8000]��[wq canskip=true]�W�@[name]�Ƃ����̂̒��ɂ��ł��B" +
        "�ǂ���[image storage=Characters/Faces/YozoFace]������b���͂Ƃ��Ƃ�[delay speed=0]���̗͐�������܂ł�����Ă������͏����]�����܂��āA���X�ɂ͂���܂��ȂȂ������B" +
        "�G����肢���̂͂ǂ������Ԃ������܂������B" +
        "�͂����đ�X����ɐ����݂܂��������������邸��ׂ����肻�̋@��ꂩ�������ɑ΂��ĕs�i�]�ł�܂��܂��āA" +
        "���̍���������l�㗬�ɉ]���āA���c����̂̂ɐ��̒��̎��ɂނ���" +
        "���w���Ƃ������玄���l�����Ԏ��ɂ���悤�ɂ��Ƃɕs���͂��l����܂�����A" +
        "�����Ƃ��v����ɑ��Ƃ̂��邽���Ă��܂����̂��Ȃ�܂�����B" +
        "�������܂�����������̂��������h�ƂȂ�܂�����A���̋������͂��ꂽ�ĂƂ������E�����Ă���ł��ł��B";

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
