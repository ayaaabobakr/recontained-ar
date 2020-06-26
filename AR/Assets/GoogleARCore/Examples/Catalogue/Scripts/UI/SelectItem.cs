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

    public void loadButton()
    {
        Query products = db.Collection("Products").Limit(3);

        products.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allcategoriesQuerySnapshot = task.Result;
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

        GameObject card = Instantiate(button) as GameObject;
        Sprite cardImage = card.GetComponentsInChildren<Image>()[1].sprite;
        cardImage = Resources.Load<Sprite>("img1") as Sprite;
        card.transform.SetParent(thisCanvas.transform, false);
        card.transform.SetParent(panel.transform);
        card.name = name;
        card.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = name;

        GameObject emptyProduct = new GameObject();
        Product p = emptyProduct.AddComponent<Product>() as Product;
        p.transform.SetParent(thisCanvas.transform);
        p.createProduct(product, card);
        p.toggle.group = panelManager.currPanel.GetComponent<ToggleGroup>();

        card.GetComponent<Button>().onClick.AddListener(() =>
           {
               panelManager.currPanel = DetailMenu;
               //    GameObject selectProduct = new GameObject();
               //    Product pp = selectProduct.AddComponent<Product>();
               //    pp.createProduct(product);
               //    pp.transform.SetParent(panelManager.currPanel.transform);
               panelManager.openPanel();
               panelManager.setData(p);
           });

        yield return StartCoroutine(p.setImageByColor(p.color));
    }
    public void changeImage()
    {


    }

}