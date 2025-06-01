using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private UIDocument uiDoc;
    private VisualElement root;

    void Start()
    {
        uiDoc = GetComponent<UIDocument>();
        root = uiDoc.rootVisualElement;
        LoadLoginUI();
    }

    void ClearUI()
    {
        root.Clear();
    }

    public void LoadLoginUI()
    {
        ClearUI();

        var loginUI = Resources.Load<VisualTreeAsset>("UI/LoginUI");
        var loginInstance = loginUI.Instantiate();
        root.Add(loginInstance);

        var loginBtn = root.Q<Button>("loginBtn");
        var toRegisterBtn = root.Q<Button>("toRegisterBtn");

        loginBtn.clicked += () =>
        {
            string email = root.Q<TextField>("emailField").value;
            string password = root.Q<TextField>("passwordField").value;

            Debug.Log($"Logging in with {email}, {password}");
        };

        toRegisterBtn.clicked += () =>
        {
            LoadRegisterUI();
        };
    }

    public void LoadRegisterUI()
    {
        ClearUI();

        var registerUI = Resources.Load<VisualTreeAsset>("UI/RegisterUI");
        var registerInstance = registerUI.Instantiate();
        root.Add(registerInstance);

        var registerBtn = root.Q<Button>("registerBtn");
        var toLoginBtn = root.Q<Button>("toLoginBtn");

        registerBtn.clicked += () =>
        {
            string email = root.Q<TextField>("emailField").value;
            string password = root.Q<TextField>("passwordField").value;
            string city = root.Q<TextField>("cityField").value;

            Debug.Log($"Registering {email}, {password}, City: {city}");
        };

        toLoginBtn.clicked += () =>
        {
            LoadLoginUI();
        };
    }
}
