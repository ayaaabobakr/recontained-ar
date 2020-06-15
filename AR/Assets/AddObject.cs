using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.ObjectManipulation;

public class AddObject : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject prefab;
    public GameObject ObjectGenerator;
    public GameObject closePanel;
    PawnManipulator manipulator;

    panelManager panelManager;

    void Start()
    {
        manipulator = ObjectGenerator.GetComponent<PawnManipulator>();
        panelManager = GameObject.Find("PanelManager").GetComponent<panelManager>();

    }

    public void viewInSpace()
    {
        panelManager.closeAll();
        manipulator.chosenPrefab = true;
        manipulator.PawnPrefab = prefab;
        // closePanel.GetComponent<openPanel>().OpenPanel();
    }
}
