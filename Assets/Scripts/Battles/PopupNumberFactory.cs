using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupNumberFactory : MonoBehaviour
{
    /// <summary>
    /// テキストのフォント
    /// </summary>
    public Font Font;

    /// <summary>
    /// フォントのサイズ
    /// </summary>
    public int FontSize = 18;

    /// <summary>
    /// 生存期間
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
