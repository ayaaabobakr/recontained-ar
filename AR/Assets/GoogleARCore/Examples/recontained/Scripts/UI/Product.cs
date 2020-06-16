﻿using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;


public class Product : MonoBehaviour
{
    public string pName { get; set; }
    public int categoryID { get; set; }
    public GameObject prefab;
    public Sprite image;
    public string color;
    public Sprite[] colorImages;
    private string[] colors;
    private Dictionary<string, string> imageMap;
    private Dictionary<string, string> prefabMap;
    private Dictionary<string, string> colorMap;

    private void Start()
    {
        colorMap = new Dictionary<string, string>();
        colors = new string[1];
        colors[0] = "silver";
        colorMap.Add("silver", "https://firebasestorage.googleapis.com/v0/b/ar-project-3d092.appspot.com/o/images%2Fsalon%20table.png?alt=media&token=97686c12-b9f8-4225-8741-612936c21651");
    }

    public IEnumerator createProduct(string pName, int categoryID, string imgUrl, string prefabUrl)
    {
        this.pName = pName;
        this.categoryID = categoryID;
        this.image = Resources.Load<Sprite>("img1") as Sprite;

        yield return StartCoroutine(setImage(imgUrl));
        yield return StartCoroutine(setPrefab(prefabUrl));
    }

    public IEnumerator createProduct(Dictionary<string, object> product)
    {
        this.pName = product["name"].ToString();
        this.categoryID = (int)product["categoryID"];
        this.colors = (string[])product["colors"];
        this.colorMap = (Dictionary<string, string>)product["colorMap"];
        this.imageMap = (Dictionary<string, string>)product["imageMap"];
        this.prefabMap = (Dictionary<string, string>)product["prefabMap"];
        this.color = this.colors[0];
        this.image = Resources.Load<Sprite>("img1") as Sprite;

        yield return changeColor(this.color);
    }

    public IEnumerator changeColor(string color)
    {
        string imageUrl = imageMap[color];
        string prefabUrl = prefabMap[color];

        yield return StartCoroutine(setImage(imageUrl));
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
        WWW www = new WWW(url);
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

    public IEnumerator getColorImages()
    {
        int numOfColors = this.colors.Length;
        colorImages = new Sprite[numOfColors];
        for (int i = 0; i < numOfColors; ++i)
        {
            string url = this.colorMap[this.colors[i]];
            Debug.Log("trying to get the color image");
            UnityWebRequest wr = new UnityWebRequest(url);
            DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
            wr.downloadHandler = texDl;
            yield return wr.SendWebRequest();
            if (!(wr.isNetworkError || wr.isHttpError))
            {
                Texture2D t = texDl.texture;
                colorImages[i] = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 1f);

                Debug.Log("color image is Loaded");
            }
            else
            {
                Debug.Log("www.error");
            }

        }
    }

}
