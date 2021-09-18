using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupNumberFactory : MonoBehaviour
{
    /// <summary>
    /// �e�L�X�g�̃t�H���g
    /// </summary>
    public Font Font;

    /// <summary>
    /// �t�H���g�̃T�C�Y
    /// </summary>
    public int FontSize = 18;

    /// <summary>
    /// ��������
    /// </summary>
    public float LifeTime = 0.5f;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="number"></param>
    public void Print(Vector2 position, int number)
    {
        var gameObject = new GameObject();
        gameObject.transform.SetParent(transform);
        gameObject.transform.position = position;

        var popupNumber = gameObject.AddComponent<PopupNumber>();
        popupNumber.Number = number;
        popupNumber.Font = Font;
        popupNumber.FontSize = FontSize;

        Destroy(gameObject, LifeTime);
    }
}
