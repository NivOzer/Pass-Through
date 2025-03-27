using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    private Transform playerCamera;
    private Transform portal;
    public Transform otherPortal;

    void Start()
    {
        playerCamera = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>()?.transform;
        portal = GameObject.FindWithTag("Portal")?.transform;
    }


    void LateUpdate()
    {
        if (!playerCamera || !portal || !otherPortal){
            Debug.LogError("PlayerCamera/Portal/OtherPortal is Missing");
            return;
        }
        
        // Correct position based on portal offset
        Vector3 playerOffsetFromPortal = playerCamera.position - portal.position;

        // Only apply offset if the player is before the portal
        if (playerOffsetFromPortal.magnitude < 500f)  // adjust threshold as needed
            transform.position = otherPortal.position + playerOffsetFromPortal;
        else
            transform.position = otherPortal.position;

        transform.rotation = playerCamera.rotation;
    }

}
