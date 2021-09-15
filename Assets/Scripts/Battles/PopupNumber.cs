using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupNumber : MonoBehaviour
{
    public int Number;

    public Font Font;

    public int FontSize = 16;

    // Start is called before the first frame update
    void Start()
    {
        var textNumber = Number.ToString();
        for (int index = 0; index < textNumber.Length; ++index)
        {
            var container = new GameObject("PopupNumber" + index);
            container.transform.SetParent(transform);

            var text = container.AddComponent<Text>();
            text.name = "PopupNumber" + index;
            text.text = textNumber[index].ToString();
            text.fontSize = FontSize;
            text.color = Color.white;
            text.font = Font;
            text.rectTransform.anchoredPosition = new Vector3(index * FontSize, 0, 0);

            var outline = container.AddComponent<Outline>();
            outline.effectDistance = new Vector2(1, -1);
            outline.effectColor = Color.black;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
