using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.ObjectManipulation;
using UnityEngine.UI;

public class AddObject : MonoBehaviour
{
    public GameObject ObjectGenerator;
    public GameObject closePanel;
    public Product product;
    PawnManipulator manipulator;
    panelManager panelManager;

    void Start()
    {
        manipulator = ObjectGenerator.GetComponent<PawnManipulator>();
        panelManager = GameObject.Find("PanelManager").GetComponent<panelManager>();
    }

    public void viewInSpace()
    {
        // Debug.Log("I'am in ViewInSpace");
        // gameObject.GetComponent<Button>().interactable = false;
        // yield return StartCoroutine(this.product.setPrefabByColor(this.product.color));
        // manipulator.PawnPrefab = this.product.getPrefabByColor(this.product.color);
        // manipulator.chosenPrefab = true;
        // // gameObject.GetComponent<Button>().interactable = true;
        // panelManager.closeAll();
        StartCoroutine(test());
    }
    public IEnumerator test()
    {
        Debug.Log("I'am in ViewInSpace");
        gameObject.GetComponent<Button>().interactable = false;
        yield return StartCoroutine(this.product.setPrefabByColor(this.product.color));
        manipulator.chosenPrefab = true;
        manipulator.PawnPrefab = this.product.getPrefabByColor(this.product.color);
        gameObject.GetComponent<Button>().interactable = true;
        panelManager.closeAll();
    }
}
