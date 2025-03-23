using UnityEngine;

public class DoubleWallRing : MonoBehaviour
{
    public DoubleWall parentWall;
    [HideInInspector] public bool hasBeenPassed = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            hasBeenPassed = true;
        }
    }
}
