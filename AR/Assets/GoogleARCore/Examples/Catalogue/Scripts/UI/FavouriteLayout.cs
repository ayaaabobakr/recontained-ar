﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;

public class FavouriteLayout : MonoBehaviour
{
    public QuerySnapshot data;
    public GameObject productBtn;
    public GameObject cardBtn;
    public GameObject canvas;
    public GameObject panel;
    private panelManager panelManager;
    public GameObject DetailMenu;
    public List<GameObject> garbage;
    private List<AddToFavourite> likes = new List<AddToFavourite>();
    private List<GameObject> favouriteProductsCards = new List<GameObject>();


    void Start()
    {
        Debug.Log("ProductLayout start");
        panelManager = GameObject.Find("PanelManager").GetComponent<panelManager>();
    }

    void OnEnable()
    {
        if (likes.Count != 0)
        {
            foreach (var like in likes)
            {
                like.isLiked();
            }
        }

    }

    public void setProduct()
    {
        Debug.Log("I am in set Product");

        foreach (DocumentSnapshot documentSnapshot in data.Documents)
        {
            Dictionary<string, object> product = documentSnapshot.ToDictionary();
            Debug.Log("I am in set Product with product name is" + product["name"].ToString());
            productCard(product);
        }
    }

    public void productCard(Dictionary<string, object> product)
    {

        var name = product["name"].ToString();
        GameObject card = Instantiate(cardBtn) as GameObject;
        Sprite cardImage = card.GetComponentsInChildren<Image>()[1].sprite;
        cardImage = Resources.Load<Sprite>("img1") as Sprite;
        card.transform.SetParent(canvas.transform, false);
        card.transform.SetParent(panel.transform);
        card.name = name;
        card.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = name;

        garbage.Add(card);

        GameObject emptyProduct = new GameObject();
        Product p = emptyProduct.AddComponent<Product>() as Product;

        garbage.Add(emptyProduct);

        if (panelManager == null)
        {
            Debug.Log("panelManager is null");
            panelManager = GameObject.Find("PanelManager").GetComponent<panelManager>();
        }
        p.transform.SetParent(canvas.transform);
        p.createProduct(product, card);
        p.toggle.group = panelManager.currPanel.GetComponent<ToggleGroup>();

        Toggle toggle = card.GetComponentInChildren<Toggle>();
        AddToFavourite addToFavourite = toggle.gameObject.GetComponent<AddToFavourite>();
        addToFavourite.product = p;
        addToFavourite.toggle = toggle;
        addToFavourite.isLiked();
        likes.Add(addToFavourite);

        card.GetComponentInChildren<Button>().onClick.AddListener(() =>
           {
               panelManager.currPanel = DetailMenu;
               panelManager.openPanel();
               panelManager.setData(p);
           });

        StartCoroutine(p.setCardImage());
        favouriteProductsCards.Add(card);
    }

    public void Reload()
    {
        for (int i = 0; i < favouriteProductsCards.Count; ++i)
        {
            GameObject card = Instantiate(favouriteProductsCards[i]) as GameObject;
            card.transform.SetParent(canvas.transform, false);
            card.transform.SetParent(panel.transform);
            card.SetActive(true);
        }
    }

    public void clearProducts()
    {
        garbage?.ForEach(Destroy);
        var products = panel.GetComponentsInChildren<Button>();
        likes.Clear();
        foreach (var product in products)
        {
            Destroy(product.gameObject);
        }

    }
}
