using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupNumber : MonoBehaviour
{
    /// <summary>
    /// �\������ԍ�
    /// </summary>
    public int Number;

    /// <summary>
    /// �e�L�X�g�̃t�H���g
    /// </summary>
    public Font Font;

    /// <summary>
    /// �t�H���g�̃T�C�Y
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
            text.rectTransform.anchoredPosition = new Vector3(index * FontSize, 0, 0);

            var outline = container.AddComponent<Outline>();
            outline.effectDistance = new Vector2(1, -1);
            outline.effectColor = Color.black;

            _popupChars.Add(new PopupChar(container));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private class PopupChar
    {
        /// <summary>
        /// 
        /// </summary>
        public GameObject GameObject { get; set; }

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="gameObject"></param>
        public PopupChar(GameObject gameObject)
        {
            GameObject = gameObject;
            _time = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            _time = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            if (_time < 0) return;

            _time += deltaTime;

            if (_time < 0.5f)
            {
                _y += 1.0f;
            }
            else if (_time < 1.0f)
            {
                _y -= 1.0f;

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
    /// 
    /// </summary>
    private List<PopupChar> _popupChars;
}
