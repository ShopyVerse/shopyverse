using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class DroneUI : MonoBehaviour
{
    public GameObject objectToToggle;

    public GameObject drone;

    public float durationVal;

    private List<Transform> locationPoints = new List<Transform>();

    public List<Button> buttons = new List<Button>();

    void Start()
    {
        var meetSalon = GameObject.Find("MeetSalon");
        var BowlingPlayground = GameObject.Find("BowlingPlayground");
        var ToggAraba = GameObject.Find("ToggAraba");
        var ShoeStore = GameObject.Find("ShoeStore");
        
        locationPoints.Add(meetSalon.transform);
        locationPoints.Add(BowlingPlayground.transform);
        locationPoints.Add(ToggAraba.transform);
        locationPoints.Add(ShoeStore.transform);

        buttons[0].onClick.AddListener(() => GoToLocation(locationPoints[0].position));
        buttons[1].onClick.AddListener(() => GoToLocation(locationPoints[1].position));
        buttons[2].onClick.AddListener(() => GoToLocation(locationPoints[2].position));
        buttons[3].onClick.AddListener(() => GoToLocation(locationPoints[3].position));
        
        objectToToggle.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
        objectToToggle.SetActive(!objectToToggle.activeSelf);
        }    
    }  

    public void GoToLocation(Vector3 targetPosition)
    {
    float distance = Vector3.Distance(targetPosition, drone.transform.position);
    float duration = distance / durationVal; // adjust the speed by changing the value of the denominator

    // unparent the drone object
    drone.transform.parent = null;
    Debug.Log("gitti");
    // move the drone object to the target position using DoTween
    drone.transform.DOMove(new Vector3(targetPosition.x,drone.transform.position.y,targetPosition.z), duration).OnComplete(() => {
        // set the drone object as a child of the player's upper shoulder
        var Parent = GameObject.Find("DroneParent");
        drone.transform.SetParent(Parent.transform);
        Debug.Log("geldi");    
        // reset the position and rotation of the drone object
        drone.transform.localPosition = Parent.transform.localPosition;
        drone.transform.localRotation = Quaternion.identity;        
        
        if (distance > 0) {
            drone.transform.LookAt(new Vector3(targetPosition.x,drone.transform.position.y,targetPosition.z));
        }
    });
}
}
