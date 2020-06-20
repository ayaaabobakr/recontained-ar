using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class SelectItem : MonoBehaviour
{
    public GameObject button;
    public GameObject thisCanvas;
    public GameObject panel;
    private RectTransform panelDimensions;
    public GameObject ObjectGenerator;
    public GameObject closePanel;
    public GameObject DetailMenu;
    private panelManager panelManager;
    private Vector2 newScale;
    Rect buttonDimensions;
    FirebaseFirestore db;
    GameObject item;

    bool isloading = true;

    void Start()
    {
        panelManager = GameObject.Find("PanelManager").GetComponent<panelManager>();
        panelDimensions = panel.GetComponent<RectTransform>();
        buttonDimensions = button.GetComponent<RectTransform>().rect;
        db = FirebaseFirestore.DefaultInstance;
        // StartCoroutine(loading());
        loadButton();
    }

    void SetUpGrid(GameObject panel, int item_num)
    {
        GridLayoutGroup grid = panel.GetComponent<GridLayoutGroup>();

        newScale = panelDimensions.sizeDelta;
        newScale.x = (grid.spacing.x + grid.cellSize.x) * item_num + grid.padding.left + grid.padding.right;
        panelDimensions.sizeDelta = newScale;
    }

    // public IEnumerator loading()
    // {

    //     while (isloading)
    //     {
    //         Debug.Log("I'm loading");
    //     }
    //     Debug.Log("I'm Done loading");
    //     yield return 1;
    // }

    public void loadButton()
    {
        Query products = db.Collection("Products").Limit(3);

        products.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allcategoriesQuerySnapshot = task.Result;
            // if (task.IsCompleted)
            // {
            //     isloading = false;
            // }
            SetUpGrid(panel, allcategoriesQuerySnapshot.Count);
            foreach (DocumentSnapshot documentSnapshot in allcategoriesQuerySnapshot.Documents)
            {
                Dictionary<string, object> product = documentSnapshot.ToDictionary();
                StartCoroutine(createButton(product));
            }
        });

    }

    public IEnumerator createButton(Dictionary<string, object> product)
    {
        var name = product["name"].ToString();

        GameObject icon = Instantiate(button) as GameObject;
        icon.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("img1") as Sprite;
        icon.transform.SetParent(thisCanvas.transform, false);
        icon.transform.SetParent(panel.transform);
        icon.name = name;
        icon.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = name;

        GameObject emptyProduct = new GameObject();
        Product p = emptyProduct.AddComponent<Product>() as Product;
        p.createProduct(product);

        icon.GetComponent<Button>().onClick.AddListener(() =>
           {
               panelManager.currPanel = DetailMenu;
               panelManager.openPanel();
               panelManager.setData(p);
           });

        yield return StartCoroutine(p.setImageByColor(p.color));

        icon.GetComponentsInChildren<Image>()[1].sprite = p.getImageByColor(p.color);

    }

}