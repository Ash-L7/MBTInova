using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateCharacterUIController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInput;
    public TextMeshProUGUI mbtiText;
    public TextMeshProUGUI temperamentText;
    public TextMeshProUGUI functionText;
    public Button recruitButton;

    private void Start()
    {
        // 添加按钮点击事件监听
        if (recruitButton != null)
        {
            recruitButton.onClick.AddListener(OnRecruitButtonClicked);
        }
    }

    private void OnRecruitButtonClicked()
    {
        // 拿玩家输入的名字
        string characterName = nameInput != null ? nameInput.text : "Unnamed";

        // 生成新市民（调用你的管理器）
        MBTICharacter newCitizen = CitizenManager.Instance.CreateRandomCitizen(characterName);

        // 显示市民信息到 UI
        if (mbtiText != null)
            mbtiText.text = $"MBTI: {newCitizen.mbtiType}";

        if (temperamentText != null)
            temperamentText.text = $"Temperament: {newCitizen.temperament.ToString()}";

        if (functionText != null)
            functionText.text = $"Function: {newCitizen.dominantFunction}";
    }
}
