namespace GoogleARCore.Examples.ObjectManipulation
{
    using UnityEngine;
    using UnityEngine.UI;
    public class delete : MonoBehaviour
    {

        // Update is called once per frame
        public ManipulationSystem system;
        public Button button;
        void Update()
        {
            if (system.SelectedObject != null)
            {
                Debug.Log("not null");
                button.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("null");
                button.gameObject.SetActive(false);
            }
        }
    }
}
