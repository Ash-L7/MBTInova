using System.Security.Authentication;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private GameObject mainUI = default;

    [SerializeField] private GameObject registerDisplay = default;
    [SerializeField] private TMP_InputField registerEmailInput = default;
    [SerializeField] private TMP_InputField registerPasswordInput = default;

    [SerializeField] private GameObject loginDisplay = default;
    [SerializeField] private TMP_InputField loginEmailInput = default;
    [SerializeField] private TMP_InputField loginPasswordInput = default;

    [SerializeField] private TextMeshProUGUI errorMessageText = default;

    public float displayErrorDuration = 5f;

    public void ChangeScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
    async void Start()
    {
        await UnityServices.InitializeAsync();
        bool isLoggedIn = AuthenticationService.Instance.IsSignedIn;

        if (isLoggedIn)
        {
            ChangeScene(1);
            registerDisplay.SetActive(false);
            loginDisplay.SetActive(false);
        }
    }

    public void OpenLoginPanel()
    {
        loginDisplay.SetActive(true);
        registerDisplay.SetActive(false);
    }

    public void OpenRegisterPanel()
    {
        loginDisplay.SetActive(false);
        registerDisplay.SetActive(true);
    }
    public async void Register()
    {
        string emailText = registerEmailInput.text;
        string passwordText = registerPasswordInput.text;

        await RegisterWithEmailPassword(emailText, passwordText);
    }

    public async void Login()
    {
        string emailText = loginEmailInput.text;
        string passwordText = loginPasswordInput.text;

        await LoginWithEmailPassword(emailText, passwordText);
    }

    async Task RegisterWithEmailPassword(string email, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(email, password);
            Debug.Log("Registration successful");
            ChangeScene(1);
            registerDisplay.SetActive(false);
            loginDisplay.SetActive(false);
        }
        catch (Unity.Services.Authentication.AuthenticationException e)
        {
            ShowErrorMessage(e.Message);
        }
        catch (RequestFailedException e)
        {
            ShowErrorMessage(e.Message);
        }
    }

    async Task LoginWithEmailPassword(string email, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(email, password);
            Debug.Log("Login successful");
            ChangeScene(1);
            registerDisplay.SetActive(false);
            loginDisplay.SetActive(false);
        }
        catch (Unity.Services.Authentication.AuthenticationException e)
        {
            ShowErrorMessage(e.Message);
        }
        catch (RequestFailedException e)
        {
            ShowErrorMessage(e.Message);
        }
    }

    public void ShowErrorMessage(string message)
    {
        errorMessageText.text = message;
        errorMessageText.gameObject.SetActive(true);
        Invoke(nameof(HideErrorMessage), displayErrorDuration);
    }

    private void HideErrorMessage()
    {
        errorMessageText.gameObject.SetActive(false);
    }
}