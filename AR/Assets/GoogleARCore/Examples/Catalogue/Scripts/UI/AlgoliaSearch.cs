// using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
// using Algolia.Search.Clients;
// using TMPro;
// using System.Linq;
// using Algolia.Search.Models.Search;
// using System.Collections;
// using System.Threading.Tasks;
public class AlgoliaSearch : MonoBehaviour
{
    // public GameObject TextField;
    // public GameObject dataWriten;
    // public GameObject WentToAsyncFun;
    // public GameObject GotData;
    // SearchIndex index;
    // SearchClient client;
    // TMP_InputField SearchInput;
    // private readonly List<GameObject> _products = new List<GameObject>();
    // public GameObject productBtn;
    // public GameObject canvas;
    // public GameObject panel;
    // private panelManager panelManager;
    // public GameObject DetailMenu;
    // private int numOfProduct;

    // void Start()
    // {

    //     var appId = System.Environment.GetEnvironmentVariable("H3UVYZKAXY");
    //     var apiKey = System.Environment.GetEnvironmentVariable("d4e0d0a9c5ee96b60dd093681381579c");
    //     client = new SearchClient("H3UVYZKAXY", "d4e0d0a9c5ee96b60dd093681381579c");
    //     index = client.InitIndex("Products");
    //     SearchInput = TextField.GetComponent<TMP_InputField>();
    //     panelManager = GameObject.Find("PanelManager").GetComponent<panelManager>();
    //     dataWriten.SetActive(true);
    // }

    // public void ValueChangeCheck()
    // {
    //     Debug.Log(SearchInput.text);
    //     var search = SearchInput.text;
    //     dataWriten.GetComponent<Text>().text = search;
    //     Debug.Log("searchKey is " + search);
    //     if (string.IsNullOrWhiteSpace(search))
    //     {
    //         _products?.ForEach(Destroy);
    //         return;
    //     }

    //     WentToAsyncFun.SetActive(true);

    //     SearchAsynchronuously(search);

    //     // RED
    //     GameObject a = new GameObject();
    //     a.AddComponent<Image>().color = new Color(1f, 0f, 0f);
    //     a.transform.SetParent(canvas.transform, false);
    //     a.transform.SetParent(panel.transform);



    // }

    // async Task SearchAsynchronuously(string searchkey)
    // {
    //     Debug.Log("SearchAsynchronuously started");

    //     var results = await index.SearchAsync<Dictionary<string, object>>(new Query("b"));

    //     // Green
    //     GameObject a = new GameObject();
    //     a.transform.SetParent(canvas.transform, false);
    //     a.transform.SetParent(panel.transform);
    //     a.AddComponent<Image>().color = new Color(0f, 1f, 0f);


    //     var resultsHits = results.Hits;
    //     GameObject b = new GameObject();
    //     b.AddComponent<Image>().color = new Color(0f, 0f, 1f);
    //     b.transform.SetParent(canvas.transform, false);
    //     b.transform.SetParent(panel.transform);

    //     GotData.SetActive(true);
    //     if (resultsHits != null)
    //     {
    //         foreach (Dictionary<string, object> result in resultsHits)
    //         {
    //         }

    //     }
    // }
    // IEnumerator LoadResult(Dictionary<string, object> product)
    // {
    //     var name = product["name"].ToString();

    //     GameObject card = Instantiate(productBtn) as GameObject;
    //     Sprite cardImage = card.GetComponentsInChildren<Image>()[1].sprite;
    //     cardImage = Resources.Load<Sprite>("img1") as Sprite;
    //     card.transform.SetParent(canvas.transform, false);
    //     card.transform.SetParent(panel.transform);
    //     card.name = name;
    //     card.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = name;

    //     GameObject emptyProduct = new GameObject();
    //     Product p = emptyProduct.AddComponent<Product>() as Product;


    //     p.transform.SetParent(canvas.transform);

    //     card.GetComponent<Button>().onClick.AddListener(() =>
    //        {
    //            panelManager.currPanel = DetailMenu;
    //            panelManager.openPanel();
    //            panelManager.setData(p);
    //        });
    //     yield return null;
    // }

    // void LoadResults(List<Product> products)
    // {
    //     // _products?.ForEach(Destroy);

    //     for (int i = 0; i < products.Count(); i++)
    //     {

    //         var name = products[i].name;

    //         GameObject card = Instantiate(productBtn) as GameObject;
    //         Sprite cardImage = card.GetComponentsInChildren<Image>()[1].sprite;
    //         cardImage = Resources.Load<Sprite>("img1") as Sprite;
    //         card.transform.SetParent(canvas.transform, false);
    //         card.transform.SetParent(panel.transform);
    //         card.name = name;
    //         card.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = name;

    //         GameObject emptyProduct = new GameObject();
    //         Product p = emptyProduct.AddComponent<Product>() as Product;


    //         p.transform.SetParent(canvas.transform);
    //         p.createProduct(products[i], card);
    //         p.toggle.group = panelManager.currPanel.GetComponent<ToggleGroup>();

    //         card.GetComponent<Button>().onClick.AddListener(() =>
    //            {
    //                panelManager.currPanel = DetailMenu;
    //                panelManager.openPanel();
    //                panelManager.setData(p);
    //            });


    //     }
    //     GotData.SetActive(true);

    // }
}
