using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using Firebase.Extensions;


public class DetailsMenu : MonoBehaviour
{
    public GameObject panel;
    public Product product;
    public GameObject image;
    public GameObject colorPanel;
    public GameObject ViewInSpace;
    public List<GameObject> garbage;
    private FirebaseFirestore db;
    private FirebaseAuth auth;
    private string userID;

    private void Start()
    {
        Debug.Log("Detailsmenu Start");
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        userID = auth.CurrentUser.UserId;

    }
    public IEnumerator Loadpage(Product product)
    {
        // clearPanel();

        this.product = product;
        this.product.colorPanel = colorPanel;
        this.product.DetailsMenu = panel;
        var addToFavourite = panel.GetComponentInChildren<AddToFavourite>();
        addToFavourite.GetComponent<AddToFavourite>().product = this.product;
        addToFavourite.toggle = addToFavourite.gameObject.GetComponent<Toggle>();
        addToFavourite.isLiked();

        AddObject addObj = ViewInSpace.GetComponent<AddObject>();
        addObj.product = product;


        panel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = this.product.name;
        string color = this.product.color;

        if (product.colorImages == null || product.colorImages.Length == 0)
        {
            Debug.Log("getColors");
            this.product.getColors();
        }
        else
        {
            setColorPanel();
        }

        if (this.product.image != null)
        {
            image.GetComponent<Image>().sprite = this.product.image;
        }

        yield return StartCoroutine(this.product.setImageByColor(color));
        image.GetComponent<Image>().sprite = this.product.getImageByColor(color);

    }

    public IEnumerator changeColor(string colorName)
    {

        if (product.color == colorName)
        {
            Debug.Log("I'am the same color, no change");
            yield return null;
        }
        Debug.Log("I'am the different color, trying to change image");
        yield return StartCoroutine(product.setImageByColor(colorName));
        image.GetComponent<Image>().sprite = product.getImageByColor(colorName); ;

    }

    public void setColorPanel()
    {
        Sprite[] images = this.product.colorImages;
        for (int i = 0; i < images.Length; ++i)
        {
            GameObject color = new GameObject();
            Image colorImage = color.AddComponent<Image>();
            colorImage.sprite = images[i];
            colorImage.GetComponent<RectTransform>().SetParent(colorPanel.transform);
            Button colorBtn = color.AddComponent<Button>();
            color.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log(colorBtn.name);
                });

        }
    }

    public void clearPanel()
    {
        product = null;
        garbage?.ForEach(Destroy);

        var colors = colorPanel.GetComponentsInChildren<Image>();
        for (int i = 1; i < colors.Length; ++i)
        {
            Destroy(colors[i].gameObject);
        }
    }
}
