using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private GameObject playerClone;
    private bool cloneCreated = false;
    private Vector3 cloneCreatedPosition;
    private float lastTapTime = 0;
    private float doubleTapTimeBetween = 0.3f;
    private bool doubleTapped = false;
    void Update()
    {
        #region Mobile cloning implementation
            if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began){
                float timeSinceLastTap = Time.time - lastTapTime;
                if (timeSinceLastTap <= doubleTapTimeBetween){
                    doubleTapped = true;
                }
                lastTapTime = Time.time;
            }
        #endregion
        if (Input.GetKeyDown(KeyCode.Space) || doubleTapped)
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
            doubleTapped = false;
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
