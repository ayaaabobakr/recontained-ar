using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using System;

public class ProductLayout : MonoBehaviour
{
    public QuerySnapshot data;
    public GameObject productBtn;
    public GameObject canvas;
    public GameObject panel;
    public bool Horizontal;
    public bool Vertical;
    private panelManager panelManager;
    public GameObject DetailMenu;
    private int numOfProduct;

    void Start()
    {
        panelManager = GameObject.Find("PanelManager").GetComponent<panelManager>();
    }

    void setUpGrid()
    {
        GridLayoutGroup grid = panel.GetComponent<GridLayoutGroup>();
        RectTransform panelDimensions = panel.GetComponent<RectTransform>();
        Vector2 newScale = panelDimensions.sizeDelta;
        Vector2 sizeOfCell;
        sizeOfCell.x = grid.cellSize.x + grid.padding.left + grid.padding.right + grid.spacing.x;
        sizeOfCell.y = grid.cellSize.y + grid.padding.top + grid.padding.bottom + grid.spacing.y;
        int numInRow = Convert.ToInt32(newScale.x / sizeOfCell.x);
        int numInColoum = (numInRow == 0) ? 0 : (numOfProduct + numInRow - 1) / numInRow;

        if (Horizontal && Vertical)
        {
            newScale.y = numInColoum * sizeOfCell.y;
            newScale.x = numInRow * sizeOfCell.x;
        }
        else if (Horizontal)
        {

            newScale.y = numOfProduct * sizeOfCell.y;
            newScale.x = sizeOfCell.x;
        }
        else if (Vertical)
        {
            newScale.x = numOfProduct * sizeOfCell.x;
            newScale.y = sizeOfCell.y;
        }

        panelDimensions.sizeDelta = newScale;

    }
    public void setProduct()
    {
        numOfProduct = data.Count;
        setUpGrid();
        Debug.Log("I am in set Product");

        foreach (DocumentSnapshot documentSnapshot in data.Documents)
        {
            Dictionary<string, object> product = documentSnapshot.ToDictionary();
            Debug.Log("I am in set Product with product name is" + product["name"].ToString());
            StartCoroutine(productCard(product));
        }
    }

    public IEnumerator productCard(Dictionary<string, object> product)
    {

        var name = product["name"].ToString();
        Debug.Log("Product Card" + name);
        GameObject icon = Instantiate(productBtn) as GameObject;
        Debug.Log("Product Card" + name);
        icon.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("img1") as Sprite;
        icon.transform.SetParent(canvas.transform, false);
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
