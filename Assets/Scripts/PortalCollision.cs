using UnityEngine;

public class PortalCollision : MonoBehaviour
{
    private Transform player;
    private bool playerIsOverlapping = false;
    private bool hasTeleported = false; // Prevents multiple triggers
    [SerializeField] AudioClip FinishedLevelSound;
    private AudioSource PortalAudio;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null){
            Debug.LogWarning("Player wasnt assigned");
        }
        PortalAudio = GetComponent<AudioSource>(); 
    }

    void Update()
    {
        if (playerIsOverlapping && !hasTeleported)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            if (dotProduct < 0f) // Player moved across the portal
            {
                hasTeleported = true;
                PortalManager.Instance.TeleportPlayerToOtherPortal(player,this.transform);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && !hasTeleported)
        {
            PortalAudio.PlayOneShot(FinishedLevelSound,0.3f);
            playerIsOverlapping = true;
            hasTeleported = true;
            PortalManager.Instance.IncrementLevel();
            PortalManager.Instance.TeleportPlayerToOtherPortal(player,this.transform);
            PortalManager.Instance.PreloadNextLevel();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOverlapping = false;
        }
    }
}
