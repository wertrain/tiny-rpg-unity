using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupNumber : MonoBehaviour
{
    /// <summary>
    /// 表示する番号
    /// </summary>
    public int Number;

    /// <summary>
    /// テキストのフォント
    /// </summary>
    public Font Font;

    /// <summary>
    /// フォントのサイズ
    /// </summary>
    public int FontSize = 16;

    // Start is called before the first frame update
    void Start()
    {
        _popupChars = new List<PopupChar>();

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

            var charWidth = (FontSize * 0.65f);
            var half = (textNumber.Length * charWidth) * 0.5f;
            text.rectTransform.anchoredPosition = new Vector3((index * charWidth) + half, 0, 0);

            var outline = container.AddComponent<Outline>();
            outline.effectDistance = new Vector2(1, -1);
            outline.effectColor = Color.black;

            _popupChars.Add(new PopupChar(container));
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (_sequence)
        {
            case 0:
                if (_time > 0.08f)
                {
                    _time = 0;

                    _popupChars[_activeIndex].Start();

                    if (++_activeIndex >= _popupChars.Count)
                    {
                        _sequence = 1;
                    }
                }
                _time += Time.deltaTime;
                break;
        }

        foreach (var p in _popupChars)
        {
            p.Update(Time.deltaTime);
        }
    }

    private class PopupChar
    {
        /// <summary>
        /// 
        /// </summary>
        public GameObject GameObject { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gameObject"></param>
        public PopupChar(GameObject gameObject)
        {
            GameObject = gameObject;
            _time = -1;
            GameObject.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            _time = 0;
            GameObject.SetActive(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            if (_time < 0) return;

            _time += deltaTime;

            if (_time < 0.1f)
            {
                _y += 1.2f;
            }
            else
            {
                _y -= 1.3f;

                if (_y <= 0.0f)
                {
                    _y = 0;
                    _time = -1.0f;
                }
            }

            var text = GameObject.GetComponent<Text>();
            text.rectTransform.anchoredPosition = new Vector3(text.rectTransform.anchoredPosition.x, _y, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        private float _time;

        /// <summary>
        /// 
        /// </summary>
        private float _y;
    }

    /// <summary>
    /// シーケンス
    /// </summary>
    private int _sequence;

    /// <summary>
    /// 時間
    /// </summary>
    private float _time;

    /// <summary>
    /// インデックス
    /// </summary>
    private int _activeIndex;

    /// <summary>
    /// 
    /// </summary>
    private List<PopupChar> _popupChars;
}
