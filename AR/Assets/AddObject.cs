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
    
    void Start()
    {
        manipulator = ObjectGenerator.GetComponent<PawnManipulator>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void viewInSpace()
    {
        manipulator.chosenPrefab = true;
        manipulator.PawnPrefab = prefab;
        closePanel.GetComponent<openPanel>().OpenPanel();
    }
}
