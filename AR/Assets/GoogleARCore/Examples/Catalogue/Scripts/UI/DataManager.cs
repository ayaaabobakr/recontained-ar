using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine.UI;
using Firebase.Auth;
using System;

public class DataManager : MonoBehaviour
{
    public GameObject categoryPanel;

    public GameObject FavouritePanel;
    public GameObject loadingPanel;
    public GameObject FavouriteButton;
    private FirebaseFirestore db;
    private panelManager panelManager;
    private FirebaseAuth auth;


    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        panelManager = GameObject.Find("PanelManager").GetComponent<panelManager>();
        auth = FirebaseAuth.DefaultInstance;
    }

    public void getDatabyCategory(Button btn)
    {
        string categoryName = btn.name;
        Query products = db.Collection("Products").WhereEqualTo("name", categoryName);

        products.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("task.Result.Count" + task.Result.Count);
                Debug.Log("is completed");
                categoryPanel.GetComponent<ProductLayout>().data = task.Result;
                panelManager.setPanel(categoryPanel);
                panelManager.openPanel();
                categoryPanel.GetComponent<ProductLayout>().setProduct();
                btn.interactable = true;
                loadingPanel.SetActive(false);


            }
            else if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("is canceled or is fault");
                // Error Panel Required;
                loadingPanel.SetActive(false);
            }

        });

    }

    public void getFavouriteProducts()
    {

        Query products = db.Collection("Favourite").WhereEqualTo("userID", auth.CurrentUser.UserId);

        products.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {

                QuerySnapshot allcategoriesQuerySnapshot = task.Result;
                if (allcategoriesQuerySnapshot.Count == 0)
                {
                    panelManager.setPanel(FavouritePanel);
                    panelManager.openPanel();
                    FavouriteButton.GetComponent<Button>().interactable = true;
                    return;
                }
                FavouriteButton.GetComponent<Button>().interactable = true;
                foreach (DocumentSnapshot documentSnapshot in allcategoriesQuerySnapshot.Documents)
                {

                    Dictionary<string, object> product = documentSnapshot.ToDictionary();
                    var productID = Int32.Parse(product["productID"].ToString());
                    Debug.Log(productID);
                    getProductByID(productID);
                }



            }
            else if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("is canceled or is fault");
                // Error Panel Required;
            }

        });

    }

    public void getProductByID(int productID)
    {

        Query products = db.Collection("Products").WhereEqualTo("ID", productID);

        products.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("task.Result.Count" + task.Result.Count);
                FavouritePanel.GetComponent<ProductLayout>().data = task.Result;
                panelManager.setPanel(FavouritePanel);

                panelManager.openPanel();
                FavouritePanel.GetComponent<ProductLayout>().setProduct();
                Debug.Log("is completed");

            }
            else if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("is canceled or is fault");
                // Error Panel Required;
            }

        });

    }


}
