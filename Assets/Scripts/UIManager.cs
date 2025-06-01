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

        root.Q<Button>("loginBtn").clicked += () =>
        {
            string email = root.Q<TextField>("emailField").value;
            string password = root.Q<TextField>("passwordField").value;
            Debug.Log($"Logging in with {email}, {password}");
            LoadGameUI();
        };

        root.Q<Button>("toRegisterBtn").clicked += LoadRegisterUI;
    }
    public void LoadRegisterUI()
    {
        ClearUI();
        var registerUI = Resources.Load<VisualTreeAsset>("UI/RegisterUI");
        var registerInstance = registerUI.Instantiate();
        root.Add(registerInstance);

        root.Q<Button>("registerBtn").clicked += () =>
        {
            string email = root.Q<TextField>("emailField").value;
            string password = root.Q<TextField>("passwordField").value;
            string city = root.Q<TextField>("cityField").value;
            Debug.Log($"Registering {email}, {password}, City: {city}");
            LoadGameUI();
        };

        root.Q<Button>("toLoginBtn").clicked += LoadLoginUI;
    }
    void LoadGameUI()
    {
        ClearUI();

        // Resource Bar
        var topBar = new VisualElement();
        topBar.style.flexDirection = FlexDirection.Row;
        topBar.style.justifyContent = Justify.SpaceAround;
        topBar.style.height = 50;
        topBar.style.backgroundColor = new Color(0.15f, 0.15f, 0.15f);

        topBar.Add(CreateResourceItem("Materials", "100"));
        topBar.Add(CreateResourceItem("Tech Points", "50"));
        topBar.Add(CreateResourceItem("Culture Points", "75"));
        topBar.Add(CreateResourceItem("Entertainment Points", "20"));

        root.Add(topBar);

        // Building 
        var mainContainer = new VisualElement();
        mainContainer.style.flexDirection = FlexDirection.Row;
        mainContainer.style.flexGrow = 1;
        mainContainer.style.marginTop = 10;

        var buildingPanel = new VisualElement();
        buildingPanel.style.flexGrow = 1;
        buildingPanel.style.flexDirection = FlexDirection.Column;
        buildingPanel.style.justifyContent = Justify.SpaceEvenly;
        buildingPanel.style.paddingTop = 20;
        buildingPanel.style.paddingBottom = 20;
        buildingPanel.style.paddingLeft = 20;
        buildingPanel.style.paddingRight = 20;
        buildingPanel.style.backgroundColor = new Color(0.15f, 0.15f, 0.2f);

        var topRow = new VisualElement();
        topRow.style.flexDirection = FlexDirection.Row;
        topRow.style.justifyContent = Justify.SpaceEvenly;
        topRow.style.flexGrow = 1;
        topRow.style.marginBottom = 10;

        topRow.Add(new BuildingCard("Factory", () => ShowBuildingDetail("Factory")));
        topRow.Add(new BuildingCard("Research Lab", () => ShowBuildingDetail("Research Lab")));

        var bottomRow = new VisualElement();
        bottomRow.style.flexDirection = FlexDirection.Row;
        bottomRow.style.justifyContent = Justify.SpaceEvenly;
        bottomRow.style.flexGrow = 1;
        bottomRow.style.marginTop = 10;

        bottomRow.Add(new BuildingCard("Art Studio", () => ShowBuildingDetail("Art Studio")));
        bottomRow.Add(new BuildingCard("Entertainment Hub", () => ShowBuildingDetail("Entertainment Hub")));

        buildingPanel.Add(topRow);
        buildingPanel.Add(bottomRow);
        mainContainer.Add(buildingPanel);

        root.Add(mainContainer);

        // bottom BAR
        var bottomBar = new VisualElement();
        bottomBar.style.flexDirection = FlexDirection.Row;
        bottomBar.style.justifyContent = Justify.SpaceAround;
        bottomBar.style.height = 60;
        bottomBar.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f);
        bottomBar.style.marginTop = 10;

        bottomBar.Add(new Button(() => { /* Resources Conversion */ }) { text = "Convert Resources" });
        bottomBar.Add(new Button(() => { /* Upgrade Buildings */ }) { text = "Upgrade Buildings" });
        bottomBar.Add(new Button(() => { /* Elect Leaders */ }) { text = "Elect Leaders" });
        bottomBar.Add(new Button(() => { /* Research Policies */ }) { text = "Research Policies" });
        bottomBar.Add(new Button(() => { ShowCitizenPopup(); }) { text = "Citizens" });
        bottomBar.Add(new Button(() => { ShowCrisisPopup(); }) { text = "Test Crisis" });

        root.Add(bottomBar);

    }
    VisualElement CreateResourceItem(string name, string amount)
    {
        var container = new VisualElement();
        container.style.flexDirection = FlexDirection.Column;
        container.style.alignItems = Align.Center;
        container.style.marginLeft = 10;
        container.style.marginRight = 10;

        var labelName = new Label(name);
        StyleTitle(labelName);

        var labelAmount = new Label(amount);
        labelAmount.style.color = Color.yellow;
        labelAmount.style.fontSize = 18;
        labelAmount.style.unityFontStyleAndWeight = FontStyle.Bold;

        container.Add(labelName);
        container.Add(labelAmount);
        return container;
    }
    VisualElement CreatePopup(float width, float height)
    {
        var popup = new VisualElement();
        popup.style.position = Position.Absolute;
        popup.style.width = width;
        popup.style.height = height;
        popup.style.backgroundColor = new Color(0.15f, 0.15f, 0.25f);
        popup.style.borderTopLeftRadius = 10;
        popup.style.borderTopRightRadius = 10;
        popup.style.paddingTop = 15;
        popup.style.paddingLeft = 20;
        popup.style.paddingRight = 20;
        popup.style.paddingBottom = 20;
        popup.style.flexDirection = FlexDirection.Column;
        return popup;
    }
    Button CreateCloseButton(VisualElement parent)
    {
        var closeBtn = new Button(() => root.Remove(parent)) { text = "X" };
        closeBtn.style.width = 30;
        closeBtn.style.height = 30;
        closeBtn.style.backgroundColor = new Color(0.4f, 0.2f, 0.2f);
        closeBtn.style.color = Color.white;
        closeBtn.style.alignSelf = Align.FlexEnd;
        closeBtn.style.marginTop = 10;
        closeBtn.style.marginRight = 10;
        return closeBtn;
    }
    void ShowCrisisPopup()
    {
        var popup = CreatePopup(450, 300);
        popup.style.position = Position.Absolute;
        popup.style.top = 120;
        popup.style.left = 120;

        var title = new Label("🔥 Crisis: NF vs ST");
        StyleTitle(title);
        title.style.marginBottom = 10;
        popup.Add(title);

        var body = new Label("The Diplomats (NF) propose a grand visionary project to inspire the city. The Sentinels (SJ) reject it as impractical. Who will you support?");
        StyleBody(body);
        body.style.marginBottom = 20;
        popup.Add(body);

        var nfButton = new Button(() =>
        {
            Debug.Log("Supported NF (gain Culture, lose Tech)");
            root.Remove(popup);
        }) { text = "Support NF (Gain Culture, ↓ Tech)" };
        SetMargin(nfButton, top: 5);

        var sjButton = new Button(() =>
        {
            Debug.Log("Supported SJ (gain Tech, lose Culture)");
            root.Remove(popup);
        }) { text = "Support SJ (Gain Tech, ↓ Culture)" };
        SetMargin(sjButton, top: 5);

        var compromiseButton = new Button(() =>
        {
            Debug.Log("Compromise chosen (no resource loss, morale drop)");
            root.Remove(popup);
        }) { text = "Compromise (Morale drops)" };
        SetMargin(compromiseButton, top: 5);

        popup.Add(nfButton);
        popup.Add(sjButton);
        popup.Add(compromiseButton);

        popup.Add(CreateCloseButton(popup));

        root.Add(popup);
    }
    void ShowBuildingDetail(string buildingName)
    {
        var popup = CreatePopup(400, 300);
        popup.style.top = 100;
        popup.style.left = 100;

        var titleBar = new VisualElement();
        titleBar.style.flexDirection = FlexDirection.Row;
        titleBar.style.justifyContent = Justify.SpaceBetween;
        titleBar.style.alignItems = Align.Center;

        var title = new Label($"🏗️ {buildingName}");
        StyleTitle(title);

        var closeBtn = CreateCloseButton(popup);
        closeBtn.style.alignSelf = Align.Auto;
        closeBtn.style.marginTop = 0;
        closeBtn.style.marginRight = 0;

        titleBar.Add(title);
        titleBar.Add(closeBtn);
        popup.Add(titleBar);

        var assignmentLabel = new Label("Assigned: Bob (ISTJ) - Efficiency: +30%");
        StyleBody(assignmentLabel);
        SetMargin(assignmentLabel, top: 10);
        popup.Add(assignmentLabel);

        var assignStatusLabel = new Label("No citizen assigned yet");
        StyleBody(assignStatusLabel);
        SetMargin(assignStatusLabel, top: 10);
        popup.Add(assignStatusLabel);

        var assignBtn = new Button(() =>
        {
            assignStatusLabel.text = "Assigned: Alice (ISFJ) - Efficiency: +30%";

            var bonusLabel = new Label("Bonus: Group Bonus Detected (+10%)");
            bonusLabel.style.color = Color.green;
            SetMargin(bonusLabel, top: 5);
            popup.Add(bonusLabel);

            Debug.Log($"Assigned Alice to {buildingName}");
        })
        {
            text = "Assign Alice"
        };
        SetMargin(assignBtn, top: 10);
        popup.Add(assignBtn);

        root.Add(popup);
    }
    void ShowCitizenPopup()
    {
        var popup = CreatePopup(420, 500);
        popup.style.position = Position.Absolute;
        popup.style.top = 100;
        popup.style.left = 100;

        var titleBar = new VisualElement();
        titleBar.style.flexDirection = FlexDirection.Row;
        titleBar.style.justifyContent = Justify.SpaceBetween;
        titleBar.style.alignItems = Align.Center;

        var title = new Label("All Citizens");
        StyleTitle(title);

        var closeBtn = CreateCloseButton(popup);
        closeBtn.style.alignSelf = Align.Auto;  // 在titleBar里水平对齐
        closeBtn.style.marginTop = 0;
        closeBtn.style.marginRight = 0;

        titleBar.Add(title);
        titleBar.Add(closeBtn);
        popup.Add(titleBar);

        var scrollView = new ScrollView();
        scrollView.style.flexGrow = 1;
        scrollView.style.marginTop = 10;

        for (int i = 0; i < 12; i++)
        {
            Temperament temp = (Temperament)Random.Range(0, 4);
            scrollView.Add(new CitizenCard(temp));
        }
        popup.Add(scrollView);

        root.Add(popup);
    }
    void StyleBody(Label label)
    {
        label.style.fontSize = 14;
        label.style.color = Color.white;
        label.style.whiteSpace = WhiteSpace.Normal;
        label.style.flexWrap = Wrap.Wrap;
        label.style.maxWidth = 400;
    }
    void StyleTitle(Label label)
    {
        label.style.fontSize = 18;
        label.style.color = Color.white;
        label.style.unityFontStyleAndWeight = FontStyle.Bold;
    }
    void SetMargin(VisualElement element, float top = 0, float right = 0, float bottom = 0, float left = 0)
    {
        element.style.marginTop = top;
        element.style.marginRight = right;
        element.style.marginBottom = bottom;
        element.style.marginLeft = left;
    }
}