using System.Collections.Generic;
using UnityEngine;

public class CitizenManager : MonoBehaviour
{
    public static CitizenManager Instance;

    private List<MBTICharacter> citizenList = new List<MBTICharacter>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public MBTICharacter CreateRandomCitizen(string name)
    {
        // 所有 MBTI 类型（你可以删减或自定义）
        string[] mbtiTypes = { "INTJ", "INFP", "ESFJ", "ESTP" };
        TemperamentType[] temperamentPool = {
            TemperamentType.Analyst,
            TemperamentType.Diplomat,
            TemperamentType.Sentinel,
            TemperamentType.Explorer
        };
        string[] dominantFunctions = { "Ni", "Fi", "Si", "Se", "Ne", "Fe", "Ti", "Te" };

        // 随机选择
        string randomMBTI = mbtiTypes[Random.Range(0, mbtiTypes.Length)];
        TemperamentType randomTemperament = temperamentPool[Random.Range(0, temperamentPool.Length)];
        string randomFunction = dominantFunctions[Random.Range(0, dominantFunctions.Length)];

        // 创建角色
        MBTICharacter newCharacter = new MBTICharacter(name, randomTemperament, randomMBTI, randomFunction);

        // 添加到列表
        citizenList.Add(newCharacter);

        return newCharacter;
    }

    // （可选）获取当前市民列表
    public List<MBTICharacter> GetAllCitizens()
    {
        return citizenList;
    }
}
