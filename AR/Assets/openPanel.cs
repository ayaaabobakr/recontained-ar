using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class openPanel : MonoBehaviour
{
    public GameObject panel;
    Vector3 panelLocation;
    public GameObject loadData;

    private void Start()
    {

        panelLocation = panel.transform.position;
        panelLocation.y = panelLocation.y + Screen.height;
        LeanTween.move(panel, panelLocation, 0f);

    }


    public void OpenPanel()
    {

        float delta = Screen.height + (4 * Screen.height) / 100;
        if (panel != null)
        {

            bool isOpen = panel.activeSelf;
            Animator animator = panel.GetComponent<Animator>();
            if (isOpen)
            {

                LeanTween.move(panel, panelLocation, 1f).setEase(LeanTweenType.easeOutBack);
                panel.SetActive(!isOpen);

            }
            else
            {
                panel.SetActive(true);
                if (loadData.GetComponent<SelectItem>() != null)
                {
                    loadData.GetComponent<SelectItem>().loadButton();
                }
                if (animator != null)
                {
                    // isOpen = animator.GetBool("open");
                    animator.SetBool("open", true);
                }
                LeanTween.move(panel, new Vector3(panelLocation.x, panelLocation.y - delta, panelLocation.z), 1f).setEase(LeanTweenType.easeOutBack);
            }

        }
    }
}
