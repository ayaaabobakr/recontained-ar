using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Loading : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject image;
    void Start()
    {
        StartCoroutine(loadingAnimation());
    }
    IEnumerator loadingAnimation()
    {

        LeanTween.scale(image, new Vector3(1, 1, 1), 3);
        yield return new WaitForSeconds(6);
        LeanTween.scale(image, new Vector3(0, 0, 0), 3);
        
    }

}
