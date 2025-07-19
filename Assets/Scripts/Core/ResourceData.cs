using UnityEngine;

[System.Serializable]
public class ResourceData
{
    public int money;
    public int sciencePoints;
    public int food;
    public int energy;

    public ResourceData()
    {
        money = 1000;
        sciencePoints = 0;
        food = 500;
        energy = 100;
    }
}