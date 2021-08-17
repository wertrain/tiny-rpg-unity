using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var statusWindow = GameObject.Find("StatusWindow");
        {
            var position = statusWindow.gameObject.transform.position;
            var defaultY = position.y;
            position.y = 800.0f;
            statusWindow.gameObject.transform.position = position;

            var rectTransform = statusWindow.GetComponent<RectTransform>();
            var localScale = rectTransform.localScale;
            var defaultScale = localScale;
            rectTransform.localScale = new Vector3(0.01f, defaultScale.y, defaultScale.z);
            LeanTween.moveY(statusWindow, defaultY, 0.35f).setOnComplete(() =>
            {
                LeanTween.scale(rectTransform, defaultScale, 0.25f).setOnComplete(() =>
            {

               });
            });
        }

        var buttons = new List<string> { "ItemButton", "SkillButton", "EquipmentButton", "StatusButton" };
        for (var i = 0; i < buttons.Count; ++i)
        {
            var button = GameObject.Find(buttons[i]);
            var position = button.gameObject.transform.position;
            var defaultX = position.x;
            position.x = 1600.0f;
            button.gameObject.transform.position = position;
            LeanTween.moveX(button, defaultX, 0.3f).setEase(LeanTweenType.easeInQuad).setDelay(0.15f * i).setOnComplete(() =>
            {

            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
