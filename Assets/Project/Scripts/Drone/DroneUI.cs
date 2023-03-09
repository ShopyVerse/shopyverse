using UnityEngine;

public class DroneUI : MonoBehaviour
{
    public GameObject objectToToggle;

    void Start()
    {
        objectToToggle.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
        objectToToggle.SetActive(!objectToToggle.activeSelf);
        }
    }  
}
