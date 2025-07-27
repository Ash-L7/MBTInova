using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [Header("City Resources")]
    public ResourceData resourceData = new ResourceData();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool SpendMoney(int amount)
    {
        if (resourceData.money >= amount)
        {
            resourceData.money -= amount;
            Debug.Log($"[ResourceManager] Spent {amount} money. Remaining: {resourceData.money}"); return true;
        }
        Debug.Log("[ResourceManager] Not enough money!");
        return false;
    }

    public void AddMoney(int amount)
    {
        resourceData.money += amount;
        Debug.Log($"Added {amount} money. Total: {resourceData.money}");
    }

    public int GetMoney()
    {
        return resourceData.money;
    }

    // future：
    public void AddScience(int amount)
    {
        resourceData.sciencePoints += amount;
    }

    public void AddFood(int amount)
    {
        resourceData.food += amount;
    }

    public void AddEnergy(int amount)
    {
        resourceData.energy += amount;
    }

    // for other resource
}