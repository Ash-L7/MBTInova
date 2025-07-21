using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EventManager : MonoBehaviour
{
    public List<Event> loadedEvents = new List<Event>();
    private Dictionary<string, int> cooldowns = new Dictionary<string, int>();
    public EventUI eventUI;

    void Start()
    {
        LoadEvents();
        StartCoroutine(EventLoop());
    }

    void LoadEvents()
    {
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("Events");
        foreach (var file in jsonFiles)
        {
            Event e = JsonUtility.FromJson<Event>(file.text);
            loadedEvents.Add(e);
            Debug.Log($"Loaded event: {e.id}");
        }
    }

    IEnumerator EventLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // 模拟 5 Tick
            TryTriggerEvent();
            UpdateCooldowns();
        }
    }

    void TryTriggerEvent()
    {
        foreach (Event e in loadedEvents)
        {
            if (ConditionsMet(e) && !IsOnCooldown(e))
            {
                if (Random.value <= e.rarity)
                {
                    TriggerEvent(e);
                    cooldowns[e.id] = e.cooldownTicks;
                    break;
                }
            }
        }
    }

    void TriggerEvent(Event e)
    {
        Debug.Log($"[Event Triggered] {e.title}");

        // 用 UI 弹出来！
        eventUI.ShowEvent(e, this);
    }


    public void ApplyChoice(Event e, int choiceIndex)
    {
        var choice = e.choices[choiceIndex];

        Debug.Log($"[Choice Picked] {choice.text}");
        foreach (var effect in choice.effects)
        {
            ApplyEffect(effect);
        }
    }

    void ApplyEffect(EventEffect effect)
    {
        Debug.Log("Would apply: " + effect.type);
        // TODO: Call ResourceManager.Instance.Add() here later
    }

    bool ConditionsMet(Event e)
    {
        // 这里先简单写死 true，表示条件都满足
        return true;
    }

    bool IsOnCooldown(Event e)
    {
        return cooldowns.ContainsKey(e.id);
    }

    void UpdateCooldowns()
    {
        var keys = cooldowns.Keys.ToList();
        foreach (var key in keys)
        {
            cooldowns[key] -= 5; // 每 Tick 5
            if (cooldowns[key] <= 0) cooldowns.Remove(key);
        }
    }

    bool CheckVisibility(EventChoice choice)
    {
        // 这里先简单写死 true，表示选项都可见
        return true;
    }
}
