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
            LoadGameUI();
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
            LoadGameUI();
        };

        toLoginBtn.clicked += () =>
        {
            LoadLoginUI();
        };
    }

    void LoadGameUI()
    {
        root.Clear();

        // 顶部资源栏
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

        // 主体横向分栏，左中右三块
        var mainContainer = new VisualElement();
        mainContainer.style.flexDirection = FlexDirection.Row;
        mainContainer.style.flexGrow = 1;
        mainContainer.style.marginTop = 10;

        // 左侧公民栏
        var citizenPanel = new VisualElement();
        citizenPanel.style.width = 250;
        citizenPanel.style.backgroundColor = new Color(0.2f, 0.2f, 0.25f);
        citizenPanel.style.paddingLeft = 10;
        citizenPanel.style.paddingTop = 10;
        citizenPanel.Add(new Label("Citizens") { style = { fontSize = 20, color = Color.white } });
        // 添加公民列表，支持拖拽
        for (int i = 0; i < 12; i++)
        {
            Temperament temp = (Temperament)Random.Range(0, 4);
            citizenPanel.Add(new CitizenCard(temp));
        }
        mainContainer.Add(citizenPanel);

        // 中央建筑栏
        var buildingPanel = new VisualElement();
        buildingPanel.style.flexGrow = 1;
        buildingPanel.style.backgroundColor = new Color(0.15f, 0.15f, 0.2f);
        buildingPanel.style.marginLeft = 10;
        buildingPanel.style.marginRight = 10;
        buildingPanel.style.paddingLeft = 10;
        buildingPanel.style.paddingTop = 10;
        buildingPanel.Add(new Label("Buildings") { style = { fontSize = 20, color = Color.white } });

        var buildingScroll = new ScrollView();
        buildingScroll.style.flexDirection = FlexDirection.Row;
        buildingScroll.style.justifyContent = Justify.SpaceEvenly;
        buildingScroll.style.flexGrow = 1;

        buildingScroll.Add(new BuildingCard("Factory"));
        buildingScroll.Add(new BuildingCard("Entertainment Hub"));
        buildingScroll.Add(new BuildingCard("Art Studio"));
        buildingScroll.Add(new BuildingCard("Research Lab"));

        buildingPanel.Add(buildingScroll);
        mainContainer.Add(buildingPanel);

        // 右侧事件面板
        var eventPanel = new VisualElement();
        eventPanel.style.width = 300;
        eventPanel.style.backgroundColor = new Color(0.25f, 0.15f, 0.15f);
        eventPanel.style.paddingRight = 10;
        eventPanel.style.paddingTop = 10;
        eventPanel.Add(new Label("Current Events") { style = { fontSize = 20, color = Color.white } });

        // 这里动态显示危机信息和解决按钮
        eventPanel.Add(CreateCrisisPanel());
        mainContainer.Add(eventPanel);

        root.Add(mainContainer);

        // 底部操作栏
        var bottomBar = new VisualElement();
        bottomBar.style.flexDirection = FlexDirection.Row;
        bottomBar.style.justifyContent = Justify.SpaceAround;
        bottomBar.style.height = 60;
        bottomBar.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f);
        bottomBar.style.marginTop = 10;

        bottomBar.Add(new Button(() => { /* 资源兑换逻辑 */ }) { text = "Convert Resources" });
        bottomBar.Add(new Button(() => { /* 升级建筑 */ }) { text = "Upgrade Buildings" });
        bottomBar.Add(new Button(() => { /* 选举领导者 */ }) { text = "Elect Leaders" });
        bottomBar.Add(new Button(() => { /* 研究政策 */ }) { text = "Research Policies" });

        root.Add(bottomBar);

        EnableDragging();
    }

    // 资源项创建器：名称 + 数值
VisualElement CreateResourceItem(string resourceName, string amount)
{
    var container = new VisualElement();
    container.style.flexDirection = FlexDirection.Column;
    container.style.alignItems = Align.Center;
    container.style.marginLeft = 10;
    container.style.marginRight = 10;

    var labelName = new Label(resourceName);
    labelName.style.color = Color.white;
    labelName.style.unityFontStyleAndWeight = FontStyle.Bold;
    labelName.style.fontSize = 14;

    var labelAmount = new Label(amount);
    labelAmount.style.color = Color.yellow;
    labelAmount.style.fontSize = 18;
    labelAmount.style.unityFontStyleAndWeight = FontStyle.Bold;

    container.Add(labelName);
    container.Add(labelAmount);
    return container;
}

// 简单的危机面板示例，显示当前危机和“解决”按钮
    VisualElement CreateCrisisPanel()
    {
        var crisisPanel = new VisualElement();
        crisisPanel.style.flexDirection = FlexDirection.Column;
        crisisPanel.style.marginTop = 10;

        // 示例：当前一个危机
        var crisisLabel = new Label("Data Storm: Tech Points -50% for 3 turns");
        crisisLabel.style.color = Color.red;
        crisisLabel.style.fontSize = 16;
        crisisLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        crisisPanel.Add(crisisLabel);

        var fixBtn = new Button(() =>
        {
            Debug.Log("Crisis resolved!");
            // 这里写处理逻辑
        })
        { text = "Resolve Crisis" };
        fixBtn.style.marginTop = 10;

        crisisPanel.Add(fixBtn);

        return crisisPanel;
    }

    // 让公民卡片可拖拽的示范实现（伪代码，具体实现根据你CitizenCard定义调整）
    void EnableDragging()
    {
        var citizens = root.Query<CitizenCard>().ToList();
        foreach (var citizen in citizens)
        {
            citizen.RegisterCallback<PointerDownEvent>(evt =>
            {
                citizen.CaptureMouse();
                // 开始拖拽
            });

            citizen.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (citizen.HasMouseCapture())
                {
                    // 跟随鼠标移动位置
                    citizen.style.left = evt.position.x;
                    citizen.style.top = evt.position.y;
                }
            });

            citizen.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (citizen.HasMouseCapture())
                {
                    citizen.ReleaseMouse();
                    // 放下时逻辑，比如判断是否放到某个建筑上
                    Debug.Log("Dropped citizen");
                }
            });
        }

    }



}
