﻿using System.Collections;
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
    public void setProduct()
    {
        clearProducts();
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
        GameObject card = Instantiate(productBtn) as GameObject;
        Sprite cardImage = card.GetComponentsInChildren<Image>()[1].sprite;
        cardImage = Resources.Load<Sprite>("img1") as Sprite;
        card.transform.SetParent(canvas.transform, false);
        card.transform.SetParent(panel.transform);
        card.name = name;
        card.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = name;

        GameObject emptyProduct = new GameObject();
        Product p = emptyProduct.AddComponent<Product>() as Product;
        if (panelManager == null)
        {
            Debug.Log("panelManager is null");
            panelManager = GameObject.Find("PanelManager").GetComponent<panelManager>();
        }
        p.transform.SetParent(panelManager.currPanel.transform);
        p.createProduct(product, card);

        card.GetComponent<Button>().onClick.AddListener(() =>
           {
               panelManager.currPanel = DetailMenu;
               GameObject selectProduct = new GameObject();
               Product pp = selectProduct.AddComponent<Product>();
               pp.createProduct(product);
               pp.transform.SetParent(panelManager.currPanel.transform);
               panelManager.openPanel();
               panelManager.setData(pp);
           });

        yield return StartCoroutine(p.setImageByColor(p.color));
    }

    public void clearProducts()
    {
        var products = panel.GetComponentsInChildren<Button>();
        foreach (var product in products)
        {
            Destroy(product.gameObject);
        }

    }
}
