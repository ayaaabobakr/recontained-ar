using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Product : MonoBehaviour
{
    public GameObject DetailsMenu;
    public string name { get; set; }
    public int categoryID { get; set; }
    public int productID { get; set; }
    public GameObject prefab;
    public Sprite image;
    public string color { get; set; }
    public string mainColor;
    public GameObject colorPanel;
    public string lowQualityImageUrl { get; set; }
    public string highQualityImageUrl { get; set; }
    public string prefabUrl { get; set; }
    public Sprite[] colorImages;
    public GameObject card;
    public Toggle toggle;
    private Dictionary<string, Sprite> imageMap;
    private Dictionary<string, GameObject> prefabMap;
    private FirebaseFirestore db;
    private string[] colors;
    private Dictionary<string, string> imageUrlMap;
    private Dictionary<string, string> prefabUrlMap;
    private Dictionary<string, string> colorUrlMap;
    private Dictionary<string, string> colorHex;

    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public void createProduct(Dictionary<string, object> product)
    {
        this.name = product["name"].ToString();
        this.color = product["color"].ToString();
        this.lowQualityImageUrl = product["lowQualityImageUrl"].ToString();
        this.highQualityImageUrl = product["highQualityImageUrl"].ToString();
        this.prefabUrl = product["prefabUrl"].ToString();
        this.categoryID = int.Parse(product["categoryID"].ToString());
        this.productID = int.Parse(product["ID"].ToString());
        this.colorHex = new Dictionary<string, string>();
        this.colorUrlMap = new Dictionary<string, string>();
        this.imageUrlMap = new Dictionary<string, string>();
        this.prefabUrlMap = new Dictionary<string, string>();
        this.imageMap = new Dictionary<string, Sprite>();
        this.prefabMap = new Dictionary<string, GameObject>();
        this.imageUrlMap.Add(this.color, this.highQualityImageUrl);
        this.prefabUrlMap.Add(this.color, this.prefabUrl);

    }

    public void createProduct(Dictionary<string, object> product, GameObject card)
    {
        this.name = product["name"].ToString();
        this.color = product["color"].ToString();
        this.lowQualityImageUrl = product["lowQualityImageUrl"].ToString();
        this.highQualityImageUrl = product["highQualityImageUrl"].ToString();
        this.prefabUrl = product["prefabUrl"].ToString();
        this.categoryID = int.Parse(product["categoryID"].ToString());
        this.productID = int.Parse(product["ID"].ToString());
        this.colorHex = new Dictionary<string, string>();
        this.colorUrlMap = new Dictionary<string, string>();
        this.imageUrlMap = new Dictionary<string, string>();
        this.prefabUrlMap = new Dictionary<string, string>();
        this.imageMap = new Dictionary<string, Sprite>();
        this.prefabMap = new Dictionary<string, GameObject>();
        this.imageUrlMap.Add(this.color, this.highQualityImageUrl);
        this.prefabUrlMap.Add(this.color, this.prefabUrl);
        this.mainColor = this.color;
        this.card = card;
        toggle = gameObject.AddComponent<Toggle>();
        toggle.isOn = true;
        toggle.interactable = false;
    }

    public void createProduct(Product product)
    {
        this.name = product.name;
        this.color = product.color;
        // this.lowQualityImageUrl = product.lowQualityImageUrl;
        // this.highQualityImageUrl = product.highQualityImageUrl;
        this.prefabUrl = product.prefabUrl;
        this.categoryID = product.categoryID;
        this.productID = product.productID;
        this.colorHex = new Dictionary<string, string>();
        this.colorUrlMap = new Dictionary<string, string>();
        this.imageUrlMap = new Dictionary<string, string>();
        this.prefabUrlMap = new Dictionary<string, string>();
        this.imageMap = new Dictionary<string, Sprite>();
        this.prefabMap = new Dictionary<string, GameObject>();
        // this.imageUrlMap.Add(this.color, this.imageUrl);
        // this.prefabUrlMap.Add(this.color, this.prefabUrl);
        this.mainColor = this.color;
        toggle = gameObject.AddComponent<Toggle>();
        toggle.isOn = true;
        toggle.interactable = false;
    }

    public void createProduct(Product product, GameObject card)
    {
        this.name = product.name;
        this.color = product.color;
        // this.lowQualityImageUrl = product.lowQualityImageUrl;
        // this.highQualityImageUrl = product.highQualityImageUrl;
        this.prefabUrl = product.prefabUrl;
        this.categoryID = product.categoryID;
        this.productID = product.productID;
        this.colorHex = new Dictionary<string, string>();
        this.colorUrlMap = new Dictionary<string, string>();
        this.imageUrlMap = new Dictionary<string, string>();
        this.prefabUrlMap = new Dictionary<string, string>();
        this.imageMap = new Dictionary<string, Sprite>();
        this.prefabMap = new Dictionary<string, GameObject>();
        // this.imageUrlMap.Add(this.color, this.imageUrl);
        // this.prefabUrlMap.Add(this.color, this.prefabUrl);
        this.mainColor = this.color;
        this.card = card;
        toggle = gameObject.AddComponent<Toggle>();
        toggle.isOn = true;
        toggle.interactable = false;
    }

    public void getColors()
    {
        Query products = db.Collection("ProductColor").WhereEqualTo("ID", this.productID);
        int cnt = 0;
        products.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allcategoriesQuerySnapshot = task.Result;
            this.colors = new string[allcategoriesQuerySnapshot.Count];
            colorImages = new Sprite[allcategoriesQuerySnapshot.Count];

            Debug.Log("number of colors are " + allcategoriesQuerySnapshot.Count);

            foreach (DocumentSnapshot documentSnapshot in allcategoriesQuerySnapshot.Documents)
            {
                Dictionary<string, object> product = documentSnapshot.ToDictionary();
                this.colors[cnt] = product["color"].ToString();
                this.colorHex.Add(colors[cnt], product["colorHex"].ToString());
                if (this.color != colors[cnt])
                {
                    this.imageUrlMap.Add(colors[cnt], product["highQualityImageUrl"].ToString());
                    this.prefabUrlMap.Add(colors[cnt], product["prefabUrl"].ToString());
                }
                else
                {
                    setImageByColor(colors[cnt]);
                }
                Debug.Log("Getting color by Hex");
                createColorImages(this.colorHex[this.colors[cnt]], cnt++);
            }
        });
    }

    public IEnumerator setCardImage()
    {

        string url = this.lowQualityImageUrl;
        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        yield return wr.SendWebRequest();
        if (!(wr.isNetworkError || wr.isHttpError))
        {
            Texture2D t = texDl.texture;
            Sprite sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 1f);
            this.image = sprite;
            this.card.GetComponentsInChildren<Image>()[1].sprite = sprite;
        }
        else
        {
            Debug.Log("www.error");
        }
    }

    public void createColorImages(string colorHex, int cnt)
    {
        GameObject colorCard = new GameObject();
        Color colorRBG;
        Image colorImage = colorCard.AddComponent<Image>();
        colorImage.rectTransform.sizeDelta = new Vector2(50, 50);
        if (ColorUtility.TryParseHtmlString(colorHex, out colorRBG))
        {
            colorImage.color = colorRBG;
        }

        colorImages[cnt] = colorImage.sprite;
        colorImage.GetComponent<RectTransform>().SetParent(colorPanel.transform);

        Toggle colorToggle = colorCard.AddComponent<Toggle>();
        Outline colorOutline = colorCard.AddComponent<Outline>();
        colorOutline.effectDistance = new Vector2(2, 2);
        colorOutline.effectColor = new Color(0, 0, 0, 1);
        colorToggle.group = colorPanel.GetComponent<ToggleGroup>();
        colorToggle.name = this.colors[cnt];
        if (this.color == this.colors[cnt])
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

    public IEnumerator setImageByColor(string colorName)
    {
        Debug.Log("I'am setting image by color");
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
            Sprite sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 1f);
            if (this.mainColor == colorName)
            {
                card.GetComponentsInChildren<Image>()[1].sprite = sprite;
                toggle.isOn = false;
            }
            if (!imageMap.ContainsKey(colorName))
            {
                this.imageMap.Add(colorName, sprite);
            }
            Debug.Log("Color is loaded" + colorName);
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
                string prefabName = this.name + this.productID + " " + colorName;
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

}