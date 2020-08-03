using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsMenu : MonoBehaviour {
    public GameObject panel;
    public GameObject panel2;
    public Product product;
    public GameObject image;
    public GameObject colorPanel;
    public GameObject ViewInSpace;
    public TextMeshProUGUI decription;
    public TextMeshProUGUI dimension;
    public TextMeshProUGUI materials;
    public List<GameObject> garbage;
    private FirebaseFirestore db;
    private FirebaseAuth auth;
    private string userID;

    private void Start () {
        Debug.Log ("Detailsmenu Start");
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        userID = auth.CurrentUser.UserId;

    }

    public IEnumerator Loadpage (Product product) {
        clearPanel ();

        this.product = product;
        this.product.colorPanel = colorPanel;
        this.product.DetailsMenu = panel;
        var addToFavourite = panel.GetComponentInChildren<AddToFavourite> ();
        addToFavourite.GetComponent<AddToFavourite> ().product = this.product;
        addToFavourite.toggle = addToFavourite.gameObject.GetComponent<Toggle> ();
        addToFavourite.isLiked ();

        ViewInSpace.GetComponent<Button> ().interactable = true;
        AddObject addObj = ViewInSpace.GetComponent<AddObject> ();
        addObj.product = product;

        panel.GetComponentsInChildren<TMPro.TextMeshProUGUI> () [0].text = this.product.name;
        panel.GetComponentsInChildren<TMPro.TextMeshProUGUI> () [1].text = "EGP " + this.product.price;
        string color = this.product.color;

        if (product.colorImages == null || product.colorImages.Length == 0) {
            Debug.Log ("getColors");
            this.product.getColors ();
        } else {
            setColorPanel ();
        }
        decription.GetComponent<TMPro.TextMeshProUGUI> ().text = this.product.description + " ";
        decription.GetComponent<TMPro.TextMeshProUGUI> ().text += " ";
        decription.gameObject.SetActive (true);

        dimension.GetComponentInChildren<TMPro.TextMeshProUGUI> ().text = this.product.dimensions + " ";
        dimension.GetComponentInChildren<TMPro.TextMeshProUGUI> ().text += " ";
        dimension.gameObject.SetActive (true);

        materials.GetComponentInChildren<TMPro.TextMeshProUGUI> ().text = this.product.material + " ";
        materials.GetComponentInChildren<TMPro.TextMeshProUGUI> ().text += " ";
        materials.gameObject.SetActive (true);

        if (this.product.image != null) {
            image.GetComponent<Image> ().sprite = this.product.image;
        }

        yield return StartCoroutine (this.product.setImageByColor (color));
        image.GetComponent<Image> ().sprite = this.product.getImageByColor (color);

    }
    public IEnumerator changeColor (string colorName) {

        if (product.color == colorName) {
            Debug.Log ("I'am the same color, no change");
            yield return null;
        }
        Debug.Log ("I'am the different color, trying to change image");

        decription.GetComponent<TMPro.TextMeshProUGUI> ().text = this.product.colorDescription[colorName];
        dimension.GetComponentInChildren<TMPro.TextMeshProUGUI> ().text = this.product.colorDimensions[colorName];
        materials.GetComponentInChildren<TMPro.TextMeshProUGUI> ().text = this.product.colorMaterial[colorName];
        yield return StartCoroutine (product.setImageByColor (colorName));
        image.GetComponent<Image> ().sprite = product.getImageByColor (colorName);;

    }
    public void setColorPanel () {
        string[] colorsHex = this.product.colorsHex;
        for (int i = 0; i < colorsHex.Length; ++i) {
            this.product.createColorImages (colorsHex[i], i);
        }
    }
    public void clearPanel () {
        product = null;
        garbage?.ForEach (Destroy);
        RectTransform myRectTransform = panel2.GetComponent<RectTransform> ();
        myRectTransform.localPosition = Vector3.zero;
        myRectTransform.anchoredPosition = Vector3.zero;
        var colors = colorPanel.GetComponentsInChildren<Image> ();
        for (int i = 0; i < colors.Length; ++i) {
            Destroy (colors[i].gameObject);
        }

    }
}