using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    /// <summary>
    /// �V���O���g���C���X�^���X
    /// </summary>
    public static MyGameManager Instance { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �ő�v���C���[�l��
    /// </summary>
    public readonly int MaxPartyCharacterNum = 4;

    /// <summary>
    /// ���݂̃p�[�e�B
    /// </summary>
    public CharacterData [] PartyMembers { get; set; }

    /// <summary>
    /// 
    /// </summary>
    private void Initialize()
    {
        PartyMembers = new CharacterData[MaxPartyCharacterNum];

        {
            var yozo = new CharacterData();
            yozo.Name = "���[�]�[";
            yozo.Face = Resources.Load<Sprite>("Characters/Faces/YozoFace");
            yozo.BaseStatus = new CharacterData.Status();
            yozo.CurrentStatus = new CharacterData.Status();
            PartyMembers[0] = yozo;

            var gregor = new CharacterData();
            gregor.Name = "�O���S�[��";
            gregor.Face = Resources.Load<Sprite>("Characters/Faces/YozoFace");
            gregor.BaseStatus = new CharacterData.Status();
            gregor.CurrentStatus = new CharacterData.Status();
            PartyMembers[1] = gregor;
        }
    }

}
