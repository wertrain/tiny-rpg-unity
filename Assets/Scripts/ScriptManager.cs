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

        var script = "��������o�Ȃ��悤�� �Y�݂ɔ�����" +
            "�ЂƂ肫��ނ��萌���ς���� �A���o��������ĐQ��" +
            "�ڂ��o�߂�΋C�������� ���ꂾ���� �����ς���ĂȂ��� ����ς�" +
            "" +
            "�������Ȃ��� �����Ȃ��� ���̒ɂ݂Ǝ���Ȃ���" +
            "�������}���悤" +
            "�C���Ȗ�� �呹�Q �����Ēʂ�l���Ȃ�_�O" +
            "�����Ă邩�炵�傤���Ȃ�" +
            "�V���p�C�i�C�����_�C�i�C�i�C�i�C�U�b�c���C�t�C�b�c�I�[���C" +
            "" +
            "���̊O������������ ����Ă�ΐ���Ă�ق�" +
            "�������Ȃ�̂͂ƂĂ��₵�����Ƃ��Ǝv���܂�" +
            "�͂��Ȍ����Őςݏグ�� �����㐶�厖�ɔq�ނ̂͂�����߂�" +
            "" +
            "�������Ȃ��� �����Ȃ��� ���̒ɂ݂Ǝ���Ȃ���" +
            "�������}���悤" +
            "�C���Ȗ�� �呹�Q �����Ēʂ�l���Ȃ�_�O" +
            "�����Ă邩�炵�傤���Ȃ�" +
            "�V���p�C�i�C�����_�C�i�C�i�C�i�C�U�b�c���C�t" +
            "" +
            "�Â������� �ʂ������� �c�������������Ȃ��̂̓_�T�C��" +
            "�f�ʂ肵��������" +
            "�C�J���j�� �i����ł��傤 �؂蔲����Α҂��Ă鎟�̃V���E" +
            "�g���u���͑f���炵���`�����X" +
            "�V���p�C�i�C�����_�C�i�C�i�C�i�C�U�b�c���C�t�C�b�c�I�[���C" +
            "" +
            "�������Ȃ��� �����Ȃ��� ���̒ɂ݂Ǝ���Ȃ���" +
            "�������}���悤" +
            "�C���Ȗ�� �呹�Q �����Ēʂ�l���Ȃ�_�O" +
            "�����Ă邩�炵�傤���Ȃ�" +
            "�V���p�C�i�C�����_�C�i�C�i�C�i�C�U�b�c���C�t" +
            "" +
            "�y�ɂȂ肽�� �����o������ ���������y���΂���΂�����" +
            "�Ȃ�Ƃ��Ȃ��" +
            "�C�J���j�� �i����ł��傤 �؂蔲����Α҂��Ă鎟�̃V���E" +
            "�g���u���͑f���炵���`�����X" +
            "�V���p�C�i�C�����_�C�i�C�i�C�i�C�U�b�c���C�t�C�b�c�I�[���C";

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
