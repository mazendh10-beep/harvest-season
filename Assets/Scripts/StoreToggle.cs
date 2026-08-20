using UnityEngine;

public class StoreToggle : MonoBehaviour
{
    public GameObject storePanel; // Assign StorePanel
    private bool isVisible = false;

    void Start()
    {
        storePanel.SetActive(isVisible);
      
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // change key if you like
        {
            isVisible = !isVisible;
            storePanel.SetActive(isVisible);
            if (isVisible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true; // Initially visible
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
