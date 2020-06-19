using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.ObjectManipulation;

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
        manipulator.chosenPrefab = true;
        // yield return StartCoroutine(this.product.setPrefabByColor(this.product.color));
        manipulator.PawnPrefab = this.product.getPrefabByColor(this.product.color);
        panelManager.closeAll();
    }
}
