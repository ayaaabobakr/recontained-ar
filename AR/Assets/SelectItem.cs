using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour {
    // Start is called before the first frame update

    public GameObject button;
    public GameObject thisCanvas;
    public GameObject panel;
    public int item_num = 7;
    private RectTransform panelDimensions;
    public Sprite img;
    Rect buttonDimensions;
    FirebaseFirestore db;
    private Vector2 newScale;
    GameObject icon;
    GameObject item;
    public GameObject ObjectGenerator;
    public GameObject closePanel;

    void Start () {
        panelDimensions = panel.GetComponent<RectTransform> ();
        buttonDimensions = button.GetComponent<RectTransform> ().rect;
        db = FirebaseFirestore.DefaultInstance;
        // storage = FirebaseStorage.DefaultInstance;
        // storage_ref = storage.GetReferenceFromUrl("gs://ar-project-3d092.appspot.com");
        SetUpGrid (panel);
        // loadButton();
    }

    // Update is called once per frame

    void SetUpGrid (GameObject panel) {
        GridLayoutGroup grid = panel.AddComponent<GridLayoutGroup> ();

        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = 1;
        grid.spacing = new Vector2 (160, 180);
        grid.padding = new RectOffset (50, 0, 50, 0);

        grid.cellSize = new Vector2(100, 180);
        grid.childAlignment = TextAnchor.UpperLeft;
        newScale = panelDimensions.sizeDelta;
        newScale.x = (grid.spacing.x + grid.cellSize.x) * item_num + grid.padding.left + grid.padding.right;
        panelDimensions.sizeDelta = newScale;
        // panelDimensions.anchoredPosition;
    }

    public void loadButton () {
        StartCoroutine (GetAssetBundle ());
        Query categories = db.Collection ("categories");
        categories.GetSnapshotAsync ().ContinueWithOnMainThread (task => {
            QuerySnapshot allcategoriesQuerySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in allcategoriesQuerySnapshot.Documents) {

                Dictionary<string, object> collection = documentSnapshot.ToDictionary ();
                StartCoroutine (LoadFromWeb (collection));
            }
        });
    }

    IEnumerator LoadFromWeb (Dictionary<string, object> collection) {
        string url = collection["img"].ToString ();
        UnityWebRequest wr = new UnityWebRequest (url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture (true);
        wr.downloadHandler = texDl;
        GameObject icon = Instantiate (button) as GameObject;
        icon.transform.SetParent (thisCanvas.transform, false);
        icon.transform.SetParent (panel.transform);

        var name = collection["name"];
        icon.name = name.ToString ();
        icon.GetComponentInChildren<TMPro.TextMeshProUGUI> ().text = name.ToString ();
        Sprite img = Resources.Load<Sprite> ("img1") as Sprite;
        icon.GetComponent<Image> ().sprite = img;
        yield return wr.SendWebRequest ();
        if (!(wr.isNetworkError || wr.isHttpError)) {
            Texture2D t = texDl.texture;
            Sprite s = Sprite.Create (t, new Rect (0, 0, t.width, t.height), Vector2.zero, 1f);

            icon.GetComponent<Image> ().sprite = s;
        }
    }

    IEnumerator GetAssetBundle () {
        string url = "https://firebasestorage.googleapis.com/v0/b/ar-project-3d092.appspot.com/o/woodtable?alt=media&token=837fd36d-860f-4a99-a574-c7e1eb56c453";
        WWW www = new WWW (url);
        yield return www;
        AssetBundle bundle = www.assetBundle;
        if (www.error == null) {
            Debug.Log ("Prefab is loaded");
            item = (GameObject) bundle.LoadAsset ("wood table");
            GameObject icon = Instantiate (button) as GameObject;
            icon.transform.SetParent (thisCanvas.transform, false);
            icon.transform.SetParent (panel.transform);

            var name = "Test Prefab";
            icon.name = name.ToString ();
            icon.GetComponentInChildren<TMPro.TextMeshProUGUI> ().text = name.ToString ();
            Sprite img = Resources.Load<Sprite> ("img1") as Sprite;
            icon.GetComponent<Image> ().sprite = img;
            item.AddComponent<BoxCollider>();
            icon.AddComponent<AddObject> ().prefab = item;
            icon.GetComponent<AddObject> ().closePanel = closePanel;
            icon.GetComponent<AddObject> ().ObjectGenerator = ObjectGenerator;
            icon.GetComponent<Button> ().onClick.AddListener (icon.GetComponent<AddObject> ().viewInSpace);

        } else {
            Debug.Log ("www.error");
        }
    }
}