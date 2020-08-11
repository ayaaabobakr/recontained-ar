using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class panelManager : MonoBehaviour
{
    public GameObject currPanel { get; set; }
    public GameObject mainPanel;
    public GameObject dataManager;
    private Vector3 panelLocation;
    private Vector3 upLocation;
    private Stack<GameObject> panelStack;
    private float delta;


    private void Start()
    {
        Debug.Log("Start panelManager");
        panelStack = new Stack<GameObject>();
        if (currPanel == null)
        {
            currPanel = mainPanel;
        }

        panelLocation = currPanel.transform.position;
        panelLocation.y = panelLocation.y + Screen.height;

        LeanTween.move(currPanel, panelLocation, 0f);
        upLocation = panelLocation;

        delta = Screen.height + (4 * Screen.height) / 100;
        currPanel.SetActive(false);

    }

    public void panelState()
    {

        if (currPanel != null)
        {
            if (currPanel.activeSelf)
            {
                Debug.Log("Close");
                StartCoroutine(closeAnimation());
            }
            else
            {
                Debug.Log("Open");
                openPanel();

            }
        }

    }

    public void openPanel()
    {
        if (panelStack.Count != 0)
        {
            Debug.Log("PanelStack is not empty");
            panelStack.Peek().SetActive(false);
        }
        panelStack.Push(currPanel);
        currPanel.SetActive(true);
        Load();
        StartCoroutine(openAnimation());

    }

    public void closePanel()
    {
        deleteData();
        currPanel.SetActive(false);
        panelStack.Pop();
    }

    public void backPanel()
    {
        closePanel();
        currPanel = panelStack.Pop();
        openPanel();
        Reload();

    }

    IEnumerator openAnimation()
    {
        Vector3 viewLoc = new Vector3(panelLocation.x, panelLocation.y - delta, panelLocation.z);
        yield return LeanTween.move(currPanel, viewLoc, 1f)
            .setEase(LeanTweenType.easeOutBack);
    }

    IEnumerator closeAnimation()
    {
        yield return LeanTween.move(currPanel, upLocation, 1f)
            .setEase(LeanTweenType.easeOutBack);
        closePanel();
    }

    public void setPanel(GameObject newPanel)
    {
        currPanel = newPanel;
    }

    public void closeAll()
    {
        currPanel.SetActive(false);
        panelStack.Clear();

    }

    public void setData(Product p)
    {
        Debug.Log("setData is activated");
        switch (currPanel.tag)
        {
            case "mainMenu":
                break;
            case "FavouritePanel":
            case "CategoriesPanel":
                currPanel.GetComponent<ProductLayout>().clearProducts();
                break;
            case "DetailsMenu":
                Debug.Log("case Details menu");
                StartCoroutine(currPanel.GetComponent<DetailsMenu>().Loadpage(p));
                break;
        }
    }

    public void deleteData()
    {
        Debug.Log("deleteData is activated");
        switch (currPanel.tag)
        {
            case "mainMenu":
                break;

            case "FavouritePanel":
            case "CategoriesPanel":
                currPanel.GetComponent<ProductLayout>().clearProducts();
                break;
            case "DetailsMenu":
                currPanel.GetComponent<DetailsMenu>().clearPanel();
                break;

        }

    }

    public void Load()
    {
        ToggleGroup toggleGroup = currPanel.GetComponent<ToggleGroup>();
        if (toggleGroup == null)
        {
            Debug.Log("No Toggle Group here");
            return;
        }
        var toggles = toggleGroup.ActiveToggles();
        Debug.Log("the lenght of the active toggle in this group is " + toggles);
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                Product product = toggle.GetComponent<Product>();
                StartCoroutine(product.setCardImage());
            }

        }
    }

    public void Reload()
    {
        Debug.Log("deleteData is activated");
        switch (currPanel.tag)
        {
            case "mainMenu":
            case "CategoriesPanel":
            case "DetailsMenu":
                break;

            case "FavouritePanel":
                Debug.Log("Reload FavouritePanel");
                currPanel.GetComponent<ProductLayout>().clearProductsLike();
                dataManager.GetComponent<DataManager>().getFavouriteProducts();
                break;
        }
    }
}