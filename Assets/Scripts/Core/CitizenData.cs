using System;
using System.Collections.Generic;
using UnityEngine;
using GameDefs;

[System.Serializable]
public class CitizenData
{
    public string citizenID;
    public string mbtiType;
    public string temperament;
    public string archetype;

    public string dominantFunction;
    public string auxiliaryFunction;
    public Dictionary<string, int> functionStats = new Dictionary<string, int>();

    public string currentBuildingID;
    public int jobSatisfaction;
    public bool isPromoted;

    public CitizenState currentState = CitizenState.Idle;
    public int highSatisfactionDays = 0;
    public string titleName = "";

    public CitizenData(string mbti)
    {
        citizenID = Guid.NewGuid().ToString();
        mbtiType = mbti;
        temperament = ResolveTemperament(mbti);
        archetype = ResolveArchetype(temperament);

        (dominantFunction, auxiliaryFunction) = GetFunctions(mbti);
        functionStats[dominantFunction] = 3;
        functionStats[auxiliaryFunction] = 2;

        // for future reference
        string[] allFunctions = { "Ti", "Te", "Fi", "Fe", "Ni", "Ne", "Si", "Se" };
        foreach (string func in allFunctions)
        {
            if (!functionStats.ContainsKey(func))
                functionStats[func] = 0;
        }

        currentBuildingID = "";
        jobSatisfaction = 100;
        isPromoted = false;
    }

    private string ResolveTemperament(string mbti)
    {
        if (mbti == "INTP" || mbti == "ENTP"  || mbti == "INTJ" || mbti == "ENTJ") return "NT";
        if (mbti == "INFP" || mbti == "ENFP"  || mbti == "INFJ" || mbti == "ENFJ") return "NF";
        if (mbti == "ISTP" || mbti == "ESTP"  || mbti == "ISFP" || mbti == "ESFP") return "SP";
        if (mbti == "ISTJ" || mbti == "ESTJ"  || mbti == "ISFJ" || mbti == "ESFJ") return "SJ";
        return "NT";
    }

    private string ResolveArchetype(string temperament)
    {
        switch (temperament)
        {
            case "NT": return "Analyst";
            case "NF": return "Diplomat";
            case "SP": return "Explorer";
            case "SJ": return "Sentinel";
            default: return "Analyst";
        }
    }

    private (string, string) GetFunctions(string mbti)
    {
        Dictionary<string, (string dom, string aux)> functionMap = new Dictionary<string, (string, string)>
        {
            { "INTP", ("Ti", "Ne") }, { "ENTP", ("Ne", "Ti") },
            { "INFJ", ("Ni", "Fe") }, { "ENFJ", ("Fe", "Ni") },
            { "INFP", ("Fi", "Ne") }, { "ENFP", ("Ne", "Fi") },
            { "ISTJ", ("Si", "Te") }, { "ESTJ", ("Te", "Si") },
            { "ISFJ", ("Si", "Fe") }, { "ESFJ", ("Fe", "Si") },
            { "ISTP", ("Ti", "Se") }, { "ESTP", ("Se", "Ti") },
            { "ISFP", ("Fi", "Se") }, { "ESFP", ("Se", "Fi") },
            { "INTJ", ("Ni", "Te") }, { "ENTJ", ("Te", "Ni") }
        };

        return functionMap.ContainsKey(mbti) ? functionMap[mbti] : ("Ti", "Ne");
    }
}