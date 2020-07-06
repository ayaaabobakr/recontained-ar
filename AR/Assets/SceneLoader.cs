using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class SceneLoader : MonoBehaviour
{

    public GameObject loadingScreen;
    public Sprite[] frames;
    public Slider slider;
    private FirebaseAuth auth;
    private bool loggedIn = false;
    public Image explosion;
    public float frameRate = 0.0f;
    private int currentImage;


    private void Start()
    {
        Debug.Log("start Scene Loader");
        auth = FirebaseAuth.DefaultInstance;
        currentImage = 0;
        InvokeRepeating("ChangeImage", 0.0f, frameRate);
    }

    private void Update()
    {
        Debug.Log("Update");
        Firebase.Auth.FirebaseUser newUser = auth.CurrentUser;
        if (newUser != null && !loggedIn)
        {
            Debug.Log("Update");
            Debug.Log("There exist an anonymously user");
            loadScene(1);
            loggedIn = true;
        }
    }
    void ChangeImage()
    {
        if (currentImage == frames.Length - 1)
        {
            currentImage = -1;

            // CancelInvoke("ChangeImage");
            // Destroy(explosion);
        }
        currentImage += 1;
        explosion.sprite = frames[currentImage];
    }
    public void onClickSignInAnonymously()
    {
        Debug.Log("onClickSignInAnonymously");
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

            Debug.Log("SignInAnonymouslyAsync was completed.");
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

        });

    }

    public void loadScene(int sceneIndex)
    {
        Debug.Log("Load Scene");
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        Debug.Log("Load Asynchronously");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            Debug.Log(progress);
            yield return null;
        }
    }

}

