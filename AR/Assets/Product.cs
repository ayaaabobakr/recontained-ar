using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Product : MonoBehaviour
{
    public string pName { get; set; }
    public int categoryID { get; set; }
    public GameObject prefab;
    public Sprite image;

    // public Product(string pName)
    // {
    //     this.pName = pName;
    //     this.image = Resources.Load<Sprite>("img1") as Sprite;
    // }

    // public Product(string pName, int categoryID)
    // {
    //     this.pName = pName;
    //     this.categoryID = categoryID;
    //     this.image = Resources.Load<Sprite>("img1") as Sprite;
    // }

    public IEnumerator createProduct(string pName, int categoryID, string imgUrl, string prefabUrl)
    {
        this.pName = pName;
        this.categoryID = categoryID;
        this.image = Resources.Load<Sprite>("img1") as Sprite;
        yield return StartCoroutine(setImage(imgUrl));
        yield return StartCoroutine(setPrefab(prefabUrl));
    }
    public IEnumerator setImage(string url)
    {
        Debug.Log("trying to get the image");
        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        yield return wr.SendWebRequest();
        if (!(wr.isNetworkError || wr.isHttpError))
        {
            Texture2D t = texDl.texture;
            this.image = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 1f);
            Debug.Log("image is Loaded");
        }
        else
        {
            Debug.Log("www.error");
        }

    }
    public Sprite getImage()
    {
        return image;
    }

    public IEnumerator setPrefab(string url)
    {
        Debug.Log("trying to get the prefab");
        WWW www = new WWW(url);
        Debug.Log("I'm here");
        yield return www;
        AssetBundle bundle = www.assetBundle;
        if (www.error == null)
        {
            Debug.Log("Prefab is loaded");
            this.prefab = (GameObject)bundle.LoadAsset(this.pName);
            if (this.prefab.GetComponent<BoxCollider>() == null)
            {
                this.prefab.AddComponent<BoxCollider>();
            }

            // addObj.prefab = prefab;
        }
        else
        {
            Debug.Log("www.error");
        }

    }
    public GameObject getPrefab()
    {
        return this.prefab;
    }



}
