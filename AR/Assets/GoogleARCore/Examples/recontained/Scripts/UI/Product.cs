using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Threading.Tasks;

public class Product : MonoBehaviour
{
    public GameObject DetailsMenu;
    public string pName { get; set; }
    public int categoryID { get; set; }
    public int productID { get; set; }
    public GameObject prefab;
    public Sprite image;
    public string color;
    public GameObject colorPanel;
    public string imageUrl;
    public string prefabUrl;
    public Sprite[] colorImages;
    private Dictionary<string, Sprite> imageMap;
    private Dictionary<string, GameObject> prefabMap;
    private FirebaseFirestore db;
    private string[] colors;
    private Dictionary<string, string> imageUrlMap;
    private Dictionary<string, string> prefabUrlMap;
    private Dictionary<string, string> colorUrlMap;


    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public void createProduct(Dictionary<string, object> product)
    {
        this.pName = product["name"].ToString();
        this.color = product["color"].ToString();
        this.imageUrl = product["imageUrl"].ToString();
        this.prefabUrl = product["prefabUrl"].ToString();
        this.categoryID = int.Parse(product["categoryID"].ToString());
        this.productID = int.Parse(product["ID"].ToString());
        this.colorUrlMap = new Dictionary<string, string>();
        this.imageUrlMap = new Dictionary<string, string>();
        this.prefabUrlMap = new Dictionary<string, string>();
        this.imageMap = new Dictionary<string, Sprite>();
        this.prefabMap = new Dictionary<string, GameObject>();
        this.imageUrlMap.Add(this.color, this.imageUrl);
        this.prefabUrlMap.Add(this.color, this.prefabUrl);

    }

    // public IEnumerator setImage(string url)
    // {
    //     UnityWebRequest wr = new UnityWebRequest(url);
    //     DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
    //     wr.downloadHandler = texDl;
    //     yield return wr.SendWebRequest();

    //     if (!(wr.isNetworkError || wr.isHttpError))
    //     {
    //         Texture2D t = texDl.texture;
    //         this.image = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 1f);
    //         Debug.Log("image is Loaded");
    //     }
    //     else
    //     {
    //         Debug.Log("www.error");
    //     }
    // }

    // public Sprite getImage()
    // {
    //     return image;
    // }

    public IEnumerator setPrefabByColor(string colorName)
    {

        if (this.prefabMap.ContainsKey(colorName)) { yield break; }

        if (!prefabUrlMap.ContainsKey(colorName))
        {
            Debug.Log("the prefab url is not here");
            yield break;
        }

        string url = this.prefabUrlMap[colorName];
        WWW www = new WWW(url);
        yield return www;
        AssetBundle bundle = www.assetBundle;

        if (www.error == null)
        {
            if (!prefabMap.ContainsKey(colorName))
            {
                Debug.Log("Prefab is loaded");
                string prefabName = this.pName + this.productID + " " + colorName;
                Debug.Log(prefabName);
                this.prefab = (GameObject)bundle.LoadAsset(prefabName);
                this.prefabMap.Add(colorName, this.prefab);
            }

            if (this.prefab.GetComponent<BoxCollider>() == null)
            {
                this.prefab.AddComponent<BoxCollider>();
            }
        }
        else
        {
            Debug.Log("www.error");
        }

    }

    public GameObject getPrefabByColor(string colorName)
    {
        if (this.prefabMap.ContainsKey(colorName))
        {
            Debug.Log("The prefab is currently in the map");
            return this.prefabMap[colorName];
        }
        return null;
    }

    public IEnumerator getColors()
    {
        Query products = db.Collection("ProductColor").WhereEqualTo("ID", this.productID);
        int cnt = 0;
        yield return null;
        products.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allcategoriesQuerySnapshot = task.Result;
            this.colors = new string[allcategoriesQuerySnapshot.Count];
            colorImages = new Sprite[allcategoriesQuerySnapshot.Count];
            foreach (DocumentSnapshot documentSnapshot in allcategoriesQuerySnapshot.Documents)
            {
                Dictionary<string, object> product = documentSnapshot.ToDictionary();
                this.colors[cnt] = product["color"].ToString();
                this.colorUrlMap.Add(colors[cnt], product["colorUrl"].ToString());
                if (this.color != colors[cnt])
                {
                    this.imageUrlMap.Add(colors[cnt], product["imageUrl"].ToString());
                    this.prefabUrlMap.Add(colors[cnt], product["prefabUrl"].ToString());

                }
                StartCoroutine(getColorImages(this.colors[cnt], cnt++));
            }
        });
    }

    public IEnumerator getColorImages(string color, int cnt)
    {
        string url = this.colorUrlMap[color];

        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        yield return wr.SendWebRequest();
        if (!(wr.isNetworkError || wr.isHttpError))
        {
            Texture2D t = texDl.texture;
            colorImages[cnt] = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 1f);

            GameObject c = new GameObject();
            Image colorImage = c.AddComponent<Image>();
            colorImage.sprite = colorImages[cnt];
            colorImage.GetComponent<RectTransform>().SetParent(colorPanel.transform);

            Toggle colorToggle = c.AddComponent<Toggle>();
            Outline colorOutline = c.AddComponent<Outline>();
            colorOutline.effectDistance = new Vector2(2, 2);
            colorOutline.effectColor = new Color(0, 0, 0, 1);
            colorToggle.group = colorPanel.GetComponent<ToggleGroup>();
            colorToggle.name = color;
            if (this.color == color)
            {
                colorOutline.enabled = true;
                colorToggle.isOn = true;
            }
            else
            {
                colorOutline.enabled = false;
                colorToggle.isOn = false;
            }

            colorToggle.onValueChanged.AddListener(delegate
            {
                if (colorToggle.isOn)
                {
                    colorOutline.enabled = true;
                    StartCoroutine(DetailsMenu.GetComponent<DetailsMenu>().changeColor(colorToggle.name));
                }
                else
                {
                    colorOutline.enabled = false;
                }
            });


            Debug.Log("color image is Loaded");
        }
        else
        {
            Debug.Log("www.error");
        }
    }

    public IEnumerator setImageByColor(string colorName)
    {
        if (this.imageMap.ContainsKey(colorName)) { yield break; }

        if (!imageUrlMap.ContainsKey(colorName)) { yield break; }

        string url = this.imageUrlMap[colorName];
        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        yield return wr.SendWebRequest();
        if (!(wr.isNetworkError || wr.isHttpError))
        {
            Texture2D t = texDl.texture;
            if (!imageMap.ContainsKey(colorName))
            {
                this.imageMap.Add(colorName, Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 1f));
                yield return StartCoroutine(setPrefabByColor(colorName));
            }
        }
        else
        {
            Debug.Log("www.error");
        }
    }
    public Sprite getImageByColor(string colorName)
    {
        this.color = colorName;
        if (this.imageMap.ContainsKey(colorName))
        {
            Debug.Log("The image is currently in the map");
            return this.imageMap[colorName];
        }
        return image;
    }
}






