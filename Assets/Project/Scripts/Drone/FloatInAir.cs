using UnityEngine;
using DG.Tweening;

public class FloatInAir : MonoBehaviour
{
    public float floatHeight = 0.2f; // The maximum height the object will float
    public float floatSpeed = 2f; // The speed at which the object will float

    void Start()
    {
        transform.DOMoveY(transform.position.y + floatHeight, floatSpeed)
            .SetEase(Ease.InOutSine) // Sets the easing function to create a smooth animation
            .SetLoops(-1, LoopType.Yoyo); // Loops the animation indefinitely
    }
}