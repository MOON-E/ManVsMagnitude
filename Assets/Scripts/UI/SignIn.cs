using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SignIn : MonoBehaviour {
    public InputField usernameText;
    public InputField passwordText;
    public Button SignInButton;

    private string username;
    private string password;

    void Start()
    {
        SignInButton = gameObject.GetComponent<Button>();
        SignInButton.onClick.AddListener(() => { LogIn(); });

    }

    public void LogIn() {
        username = usernameText.text;
        password = passwordText.text;

        Debug.Log(username + " is the username");
        Debug.Log(password + " is the password");
    }
}
