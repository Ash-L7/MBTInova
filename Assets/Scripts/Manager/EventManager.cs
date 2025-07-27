using UnityEngine;

public class EventManager : MonoBehaviour
{
    private CityData cityData;

    public void Init(CityData data)
    {
        cityData = data;
    }

    public void CheckTriggers()
    {
        // TODO: Check Criteria
    }

    public void ForceTrigger(string eventID)
    {
        // TODO: testing
        Debug.Log($"Force triggering event: {eventID}");
    }
}