using UnityEngine;

public enum TemperamentType { Analyst, Diplomat, Sentinel, Explorer }

[System.Serializable]
public class MBTICharacter
{
    public string name;
    public TemperamentType temperament;
    public string mbtiType;
    public string dominantFunction;

    public int productivity;
    public int creativity;
    public int stability;

    public MBTICharacter(string name, TemperamentType temperament, string mbtiType, string dominantFunction)
    {
        this.name = name;
        this.temperament = temperament;
        this.mbtiType = mbtiType;
        this.dominantFunction = dominantFunction;

        // 简单初始值（可之后做公式）
        this.productivity = 10;
        this.creativity = 10;
        this.stability = 10;
    }
}