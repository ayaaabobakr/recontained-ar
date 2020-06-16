using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{

    public GameObject button;
    public GameObject thisCanvas;
    public GameObject panel;
    private RectTransform panelDimensions;
    Rect buttonDimensions;
    FirebaseFirestore db;
    private Vector2 newScale;
    GameObject item;
    public GameObject ObjectGenerator;
    public GameObject closePanel;
    panelManager panelManager;
    public GameObject DetailMenu;

    void Start()
    {
        Debug.Log("Start SelectItem");
        panelManager = GameObject.Find("PanelManager").GetComponent<panelManager>();
        panelDimensions = panel.GetComponent<RectTransform>();
        buttonDimensions = button.GetComponent<RectTransform>().rect;
        db = FirebaseFirestore.DefaultInstance;
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
        Query products = db.Collection("Products");
        products.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log("loading Products");
            QuerySnapshot allcategoriesQuerySnapshot = task.Result;
            Debug.Log(allcategoriesQuerySnapshot.Count);
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
        var imgURL = product["imgURL"].ToString();
        var prefabURL = product["prefabURL"].ToString();

        GameObject icon = Instantiate(button) as GameObject;
        icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("img1") as Sprite;
        icon.transform.SetParent(thisCanvas.transform, false);
        icon.transform.SetParent(panel.transform);
        icon.name = name;
        icon.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = name;

        Product p = icon.AddComponent<Product>() as Product;

        icon.GetComponent<Button>().onClick.AddListener(() =>
          {
              panelManager.currPanel = DetailMenu;
              panelManager.setData(p);
              panelManager.openPanel();
          });

        yield return p.createProduct(name, 1, imgURL, prefabURL);
        // yield return p.createProduct(product);


        icon.GetComponent<Image>().sprite = p.getImage();
        item = p.getPrefab();



        AddObject addObj = icon.AddComponent<AddObject>();
        addObj.closePanel = closePanel;
        addObj.ObjectGenerator = ObjectGenerator;
        addObj.prefab = item;

    }

}