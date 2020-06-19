using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailsMenu : MonoBehaviour
{
    public Product product;
    public GameObject panel;
    public GameObject image;
    public GameObject colorPanel;
    public GameObject ViewInSpace;

    public IEnumerator Loadpage(Product product)
    {
        this.product = product;
        this.product.colorPanel = colorPanel;
        this.product.DetailsMenu = panel;

        AddObject addObj = ViewInSpace.GetComponent<AddObject>();
        Debug.Log("i will put the product in the addobject component");
        addObj.product = product;
       
        panel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = this.product.pName;
        string color = this.product.color;

        if (this.product.image == null)
        {
            yield return StartCoroutine(this.product.setImageByColor(color));
        }

        this.product.image = this.product.getImageByColor(color);
        image.GetComponent<Image>().sprite = this.product.image;

        if (product.colorImages == null || product.colorImages.Length == 0)
        {
            Debug.Log("getColors");
            yield return StartCoroutine(this.product.getColors());
        }
        else
        {
            setColorPanel();
        }

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
}
