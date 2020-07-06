using UnityEngine;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;


public class Auth : MonoBehaviour
{

    public GameObject UserNameField;
    public GameObject emailInputField;
    public GameObject passwordInputField;
    private FirebaseAuth auth;
    private string userEmail;
    private string userPassword;
    private string userName;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    // public void SignIn()
    // {
    //     userEmail = emailInputField.GetComponent<TMP_InputField>().text;
    //     userPassword = passwordInputField.GetComponent<TMP_InputField>().text;
    //     onClickSignInUser(userEmail, userPassword);

    // }

    // public void CreateUser()
    // {
    //     userName = UserNameField.GetComponent<TMP_InputField>().text;
    //     userEmail = emailInputField.GetComponent<TMP_InputField>().text;
    //     userPassword = passwordInputField.GetComponent<TMP_InputField>().text;
    //     Debug.Log("userName : " + userName);
    //     Debug.Log("userEmail : " + userEmail);
    //     Debug.Log("userPassword : " + userPassword);
    //     onClickCreateUser(userName, userEmail, userPassword);
    // }

    public void onClickSignInAnonymously()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            loadScene(1);
        });

    }

    // public void onClickSignInUser(string email, string password)
    // {
    //     auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
    //     {
    //         if (task.IsCanceled)
    //         {
    //             Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
    //             return;
    //         }
    //         if (task.IsFaulted)
    //         {
    //             Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
    //             return;
    //         }

    //         Firebase.Auth.FirebaseUser newUser = task.Result;
    //         Debug.LogFormat("User signed in successfully: {0} ({1})",
    //             newUser.DisplayName, newUser.UserId);
    //     });

    // }

    // public void onClickCreateUser(string name, string email, string password)
    // {
    //     auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
    //     {
    //         if (task.IsCanceled)
    //         {
    //             Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
    //             return;
    //         }
    //         if (task.IsFaulted)
    //         {
    //             Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
    //             return;
    //         }

    //         // Firebase user has been created.
    //         Firebase.Auth.FirebaseUser newUser = task.Result;
    //         // firebase.auth().CurrentUser.DisplayName = name;
    //         Debug.LogFormat("Firebase user created successfully: {0} ({1})",
    //             newUser.DisplayName, newUser.UserId);
    //     });

    // }

    public void loadScene(int sceneIndex)
    {
        Debug.Log("Load Sence");
        
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex){
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while(!operation.isDone) {
            Debug.Log(operation.progress);
            yield return null;
        }
    }

    // public void onClickSignOutUser()
    // {
    //     auth.SignOut();
    // }
}