using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public GameObject categoryPanel;
    private FirebaseFirestore db;
    private panelManager panelManager;



    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        panelManager = GameObject.Find("PanelManager").GetComponent<panelManager>();
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


            }
            else if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("is canceled or is fault");
                // Error Panel Required;
            }

        });

    }


}
