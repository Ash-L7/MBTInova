using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class LoginManage : MonoBehaviour
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

    async void Start()
    {
        await UnityServices.InitializeAsync();
        bool isLoggedIn = AuthenticationService.Instance.IsSignedIn;

        if (isLoggedIn)
        {
            registerDisplay.SetActive(false);
            loginDisplay.SetActive(false);
        }
    }

    public void OpenLoginPanel()
    {
        loginDisplay.SetActive(true);
        registerDisplay.SetActive(false);
        mainUI.SetActive(false);
    }

    public void OpenRegisterPanel()
    {
        loginDisplay.SetActive(false);
        registerDisplay.SetActive(true);
        mainUI.SetActive(false);
    }
    public async void Register()
    {
        string emailText = registerEmailInput.text;
        string passwordText = registerPasswordInput.text;
        
        await RegisterWithEmailPassword(emailText, passwordText);
        mainUI.SetActive(true);
    }

    public async void Login()
    {
        string emailText = loginEmailInput.text;
        string passwordText = loginPasswordInput.text;

        await LoginWithEmailPassword(emailText, passwordText);
        mainUI.SetActive(true);
    }

    async Task RegisterWithEmailPassword(string email, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(email, password);
            Debug.Log("Registration successful");
        }
        catch (AuthenticationException e)
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
        }
        catch (AuthenticationException e)
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