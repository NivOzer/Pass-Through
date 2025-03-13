using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private GameObject playerClone;
    private bool cloneCreated = false;
    private Vector3 cloneCreatedPosition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!cloneCreated)
            {
                cloneCreatedPosition = player.transform.position;
                playerClone = Instantiate(player, cloneCreatedPosition, player.transform.rotation);
                cloneCreated = true;
            }
            else
            {
                Destroy(playerClone);
                cloneCreated = false;
            }
        }

        // If the clone exists, move it in mirrored relation to the player's movements
        if (cloneCreated && playerClone != null)
        {
            Vector3 playerOffset = player.transform.position - cloneCreatedPosition;
            //Mirroring
            playerClone.transform.position = cloneCreatedPosition - playerOffset;

            playerClone.transform.position = new Vector3(
                playerClone.transform.position.x, 
                playerClone.transform.position.y, 
                player.transform.position.z // Z remains untouched
            );
            // Same Rotation as player
            playerClone.transform.rotation = player.transform.rotation;
        }
    }
}
