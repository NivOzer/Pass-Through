using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    private Transform playerCamera;
    private Transform portal;
    public Transform otherPortal;

    void Start()
    {
        // Find the player's camera in Level 1
        GameObject playerCamObj = GameObject.FindWithTag("MainCamera");
        if (playerCamObj != null)
            playerCamera = playerCamObj.GetComponent<Camera>().transform;

        // Find the portal in Level 1
        GameObject portalObj = GameObject.FindWithTag("Portal");
        if (portalObj != null)
            portal = portalObj.transform;
    }

    void LateUpdate()
    {
        
        if (!playerCamera)
            Debug.LogWarning("Player Camera reference is missing!");

        if (!portal)
            Debug.LogWarning("Portal reference is missing!");

        if (!otherPortal)
            Debug.LogWarning("Other Portal reference is missing!");

        // Correct position based on portal offset
        Vector3 playerOffsetFromPortal = playerCamera.position - portal.position;
        transform.position = otherPortal.position + playerOffsetFromPortal;

        // Directly match rotation
        transform.rotation = playerCamera.rotation;

    }
}
