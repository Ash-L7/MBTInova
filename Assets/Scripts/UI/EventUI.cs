using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventUI : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Transform choiceContainer;
    public Button choiceButtonPrefab;

    private EventManager eventManager; // 引用谁来执行 ApplyChoice

    public void ShowEvent(Event e, EventManager manager)
    {
        eventManager = manager;

        titleText.text = e.title;
        descriptionText.text = e.description;

        // 清空旧按钮
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < e.choices.Count; i++)
        {
            var choice = e.choices[i];
            var btn = Instantiate(choiceButtonPrefab, choiceContainer);
            btn.GetComponentInChildren<TMP_Text>().text = choice.text;

            int index = i; // 避免闭包坑
            btn.onClick.AddListener(() =>
            {
                eventManager.ApplyChoice(e, index);
                Hide();
            });
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
