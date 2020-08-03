using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour {
    public GameObject button;
    public GameObject cardBtn;
    public GameObject thisCanvas;
    public GameObject panel;
    public GameObject DetailMenu;
    private panelManager panelManager;
    private FirebaseFirestore db;
    private Toggle toggle;

    private void OnEnable () {
        toggle = panel.GetComponent<Toggle> ();

        if (toggle.isOn) {
            panelManager = GameObject.Find ("PanelManager").GetComponent<panelManager> ();
            db = FirebaseFirestore.DefaultInstance;
            loadButton ();
        }
    }

    public void loadButton () {
        Query products = db.Collection ("Products").Limit (3);

        products.GetSnapshotAsync ().ContinueWithOnMainThread (task => {
            QuerySnapshot allcategoriesQuerySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in allcategoriesQuerySnapshot.Documents) {
                Dictionary<string, object> product = documentSnapshot.ToDictionary ();
                createButton (product);
            }
            panel.GetComponent<Toggle> ().isOn = false;
        });

    }

    public void createButton (Dictionary<string, object> product) {
        var name = product["name"].ToString ();

        var x = decimal.Parse (product["price"].ToString ());
        var price = "EGP " + Math.Round ((decimal) x, 2);

        GameObject card = Instantiate (button) as GameObject;
        Sprite cardImage = card.GetComponentsInChildren<Image> () [1].sprite;
        cardImage = Resources.Load<Sprite> ("img1") as Sprite;
        card.transform.SetParent (thisCanvas.transform, false);
        card.transform.SetParent (panel.transform);
        card.name = name;
        card.GetComponentsInChildren<TMPro.TextMeshProUGUI> () [0].text = name;
        card.GetComponentsInChildren<TMPro.TextMeshProUGUI> () [1].text = price;

        GameObject emptyProduct = new GameObject ();
        Product p = emptyProduct.AddComponent<Product> () as Product;

        p.transform.SetParent (thisCanvas.transform);
        p.createProduct (product, card);
        p.toggle.group = panelManager.currPanel.GetComponent<ToggleGroup> ();

        // Toggle toggle = card.GetComponentInChildren<Toggle>();
        // AddToFavourite addToFavourite = toggle.gameObject.GetComponent<AddToFavourite>();
        // addToFavourite.product = p;
        // addToFavourite.toggle = toggle;
        // addToFavourite.isLiked();

        card.GetComponent<Button> ().onClick.AddListener (() => {
            panelManager.currPanel = DetailMenu;
            panelManager.openPanel ();
            panelManager.setData (p);
        });

        StartCoroutine (p.setCardImage ());
    }

}