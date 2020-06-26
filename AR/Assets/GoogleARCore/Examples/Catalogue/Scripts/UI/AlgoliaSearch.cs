using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Algolia.Search.Clients;
using TMPro;
using System.Linq;
using Algolia.Search.Models.Search;
using System.Collections;



namespace Assets.Scripts
{
    public class AlgoliaSearch : MonoBehaviour
    {
        public GameObject TextField;
        SearchIndex index;
        SearchClient client;
        TMP_InputField SearchInput;
        private readonly List<GameObject> _products = new List<GameObject>();
        public GameObject productBtn;
        public GameObject canvas;
        public GameObject panel;
        private panelManager panelManager;
        public GameObject DetailMenu;
        private int numOfProduct;

        void Start()
        {

            var appId = System.Environment.GetEnvironmentVariable("H3UVYZKAXY");
            var apiKey = System.Environment.GetEnvironmentVariable("d4e0d0a9c5ee96b60dd093681381579c");
            client = new SearchClient("H3UVYZKAXY", "d4e0d0a9c5ee96b60dd093681381579c");
            index = client.InitIndex("Products");
            SearchInput = TextField.GetComponent<TMP_InputField>();
            panelManager = GameObject.Find("PanelManager").GetComponent<panelManager>();
        }


        public async void ValueChangeCheck()
        {
            Debug.Log(SearchInput.text);
            var search = SearchInput.text;

            if (string.IsNullOrWhiteSpace(search))
            {
                _products?.ForEach(Destroy);
                return;
            }

            var results = await index.SearchAsync<Product>(new Query(SearchInput.text));

            if (results.Hits != null)
            {
                Debug.Log(results.Hits.Count);
                StartCoroutine(LoadResults(results.Hits));
            }
        }

        IEnumerator LoadResults(List<Product> products)
        {
            _products?.ForEach(Destroy);

            for (int i = 0; i < products.Count(); i++)
            {
                // var planetObject = new GameObject($"{products[i].name}");
                var name = products[i].name;
                GameObject card = Instantiate(productBtn) as GameObject;
                Sprite cardImage = card.GetComponentsInChildren<Image>()[1].sprite;
                cardImage = Resources.Load<Sprite>("img1") as Sprite;
                card.transform.SetParent(canvas.transform, false);
                card.transform.SetParent(panel.transform);
                card.name = name;
                card.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = name;
                _products?.Add(card);

                GameObject emptyProduct = new GameObject();
                Product p = emptyProduct.AddComponent<Product>() as Product;
                if (panelManager == null)
                {
                    Debug.Log("panelManager is null");
                    panelManager = GameObject.Find("PanelManager").GetComponent<panelManager>();
                }
                p.transform.SetParent(panelManager.currPanel.transform);
                p.createProduct(products[i], card);
                _products?.Add(emptyProduct);

                card.GetComponent<Button>().onClick.AddListener(() =>
                   {
                       panelManager.currPanel = DetailMenu;
                       GameObject selectProduct = new GameObject();
                       Product pp = selectProduct.AddComponent<Product>();
                       pp.createProduct(products[i]);
                       pp.transform.SetParent(panelManager.currPanel.transform);
                       panelManager.openPanel();
                       panelManager.setData(pp);
                   });

                yield return StartCoroutine(p.setImageByColor(p.color));
            }

        }
    }
}
