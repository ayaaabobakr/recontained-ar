using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class panelManager : MonoBehaviour
{

    public GameObject currPanel { get; set; }
    public GameObject mainPanel;
    public GameObject colorPanel;
    public GameObject productImage;
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
        Load();
        panelStack.Push(currPanel);
        currPanel.SetActive(true);
        StartCoroutine(openAnimation());
        

    }
    public void closePanel()
    {
        currPanel.SetActive(false);
        panelStack.Pop();
    }

    public void backPanel()
    {
        closePanel();
        currPanel = panelStack.Peek();
        openPanel();

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

    public void getData()
    {
        switch (currPanel.tag)
        {
            case "mainMenu":
                break;
            case "detailsMenu":

                break;
        }
    }

    public void setData(Product p)
    {
        Debug.Log("setData is activated");
        switch (currPanel.tag)
        {
            case "mainMenu":
                break;
            case "DetailsMenu":
                Debug.Log("case Details menu");

                StartCoroutine(currPanel.GetComponent<DetailsMenu>().Loadpage(p));

                // currPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = p.pName;
                // currPanel.AddComponent<Product>();
                // productImage.GetComponent<Image>().sprite = p.image;
                // currPanel.GetComponentInChildren<AddObject>().prefab = p.prefab;
                // p.getColors();
                // Debug.Log("I'm back from getColors awiat");
                // setColor(p);
                break;
        }
    }

    public void Load()
    {
        Toggle[] toggles = currPanel.GetComponentsInChildren<Toggle>();
        if (currPanel == mainPanel)
        {
            Debug.Log("the number of toggles are " + toggles.Length + " and the current panel is mainPanel");
        }
        else
        {
            Debug.Log("the number of toggles are " + toggles.Length + " and the current panel is categoryPanel");
        }

        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                Product product = toggle.GetComponent<Product>();
                StartCoroutine(product.setImageByColor(product.mainColor));
            }

        }
    }
}
