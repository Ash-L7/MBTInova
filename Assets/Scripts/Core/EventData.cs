using System;
using System.Collections.Generic;

[System.Serializable]
public class Event
{
    public string id;
    public string title;
    public string description;
    public string category;
    public string[] conditions;
    public int cooldownTicks;
    public float rarity;
    public List<EventChoice> choices;
}

[System.Serializable]
public class EventChoice
{
    public string text;
    public string visibilityCondition; // 以后可以写成条件，这里先写 ""
    public List<EventEffect> effects;
    public string nextEventId; // 可以留空
}

[System.Serializable]
public class EventEffect
{
    public string type;   // resource_change, buff, unlock
    public string target; // Science, Stability, etc.
    public float value;
}
