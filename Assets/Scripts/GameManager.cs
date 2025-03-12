using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    private GameObject playerClone;
    private bool cloneCreated = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            if(!cloneCreated){
                Vector3 clonePosition = player.transform.position - (player.transform.right * 10);
                playerClone = Instantiate(player,clonePosition,player.transform.rotation);
                cloneCreated = true;
            }
            else{
                cloneCreated = false;
                Destroy(playerClone);
            }
        }

        if (cloneCreated && playerClone != null)
        {
            playerClone.transform.position = player.transform.position - (player.transform.right * 10);
            playerClone.transform.rotation = player.transform.rotation;
        }
    }
}
