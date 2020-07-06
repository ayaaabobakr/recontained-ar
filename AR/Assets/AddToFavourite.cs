using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine.UI;
using System.Collections;


public class AddToFavourite : MonoBehaviour
{

    public bool isLike = false;
    public Toggle toggle;
    public Product product = null;
    private FirebaseFirestore db;
    private FirebaseAuth auth;
    private string userID;
    public List<GameObject> favouriteProductsCards;


    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        userID = auth.CurrentUser.UserId;

    }
    public bool isLiked()
    {
        Debug.Log("isliked wooooooooohoooooooooooooooooo");
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }
        if (auth == null)
        {
            auth = FirebaseAuth.DefaultInstance;
            userID = auth.CurrentUser.UserId;
        }

        string DocName = product.productID + ":" + userID;
        Debug.Log(DocName);
        DocumentReference docRef = db.Collection("Favourite").Document(DocName);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log("You Like product no " + product.productID);
                toggle.isOn = true;
                isLike = true;

            }
            else
            {
                Debug.Log("this product is not likedS");
                toggle.isOn = false;
                isLike = false;

            }
        });
        return true;
    }

    public void onClickToggleLike()
    {
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }
        if (auth == null)
        {
            auth = FirebaseAuth.DefaultInstance;
            userID = auth.CurrentUser.UserId;
        }

        Debug.Log("onClickToggleLike");
        string DocName = product.productID + ":" + userID;
        Debug.Log(DocName);
        DocumentReference docRef = db.Collection("Favourite").Document(DocName);
        if (!isLike)
        {
            Debug.Log("like");
            isLike = true;
            Dictionary<string, object> like = new Dictionary<string, object>{
                { "productID", product.productID },
                { "userID",  userID},
            };

            docRef.SetAsync(like).ContinueWithOnMainThread(task =>
            {
                Debug.Log("Added Like to Favourite");
            });
        }
        else
        {
            docRef.DeleteAsync();
            // if (gameObject.tag == "FavouritePanel")
            // {
            //     GameObject card = gameObject.GetComponentsInParent<RectTransform>()[1].gameObject;
            //     Destroy(card);
            //     Debug.Log("one is not liked");
            // }
            Debug.Log("dislike");
            isLike = false;
        }
    }

}
