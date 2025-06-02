using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class LoginManager : MonoBehaviour
{
    public GameObject loginPanel, registerPanel, notificationPanel;

    public TMP_InputField loginEmail, loginPassword, registerEmail, registerPassword;

    public TMP_Text notifTitleText, notifMessageText;
    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        notificationPanel.SetActive(false);
    }

    public void OpenRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        notificationPanel.SetActive(false);
    }

    public void LoginUser()
    {
        // Checks if the email or password field for login page is empty
        if (string.IsNullOrEmpty(loginEmail.text) || string.IsNullOrEmpty(loginPassword.text))
        {
            Debug.LogError("Email or Password cannot be empty.");
            showNotificationMessage("Error", "Email or Password cannot be empty.");
            return;
        }

        // Do login
    }

    public void RegisterUser()
    {
        // Checks if the email or password field for login page is empty
        if (string.IsNullOrEmpty(registerEmail.text) || string.IsNullOrEmpty(registerPassword.text))
        {
            Debug.LogError("Email or Password cannot be empty.");
            showNotificationMessage("Error", "Email or Password cannot be empty.");
            return;
        }

        // Do registration
    }

    private void showNotificationMessage(string title, string message)
    {
        notifTitleText.text = "" + title;
        notifMessageText.text = "" + message;

        notificationPanel.SetActive(true);
    }
}
