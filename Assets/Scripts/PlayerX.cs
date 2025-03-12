using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerX : MonoBehaviour
{
    public Joystick joystick;
    private bool passedThroughRing = false;
    [SerializeField] float flySpeed;
    [SerializeField] float yawAmount;
    private float yaw;
    private int ringWallAmount;
    private int passedThrough = 0;
    private GameObject portal;
    private int currentlevel = 0;
    public GameObject MisedText;
    [SerializeField] GameObject audioObj;
    private AudioManagerX audioManager;

    void Start(){
        portal = GameObject.FindGameObjectWithTag("Portal");
        portal.GetComponent<Renderer>().enabled = false;
        audioManager = audioObj.GetComponent<AudioManagerX>();   
    }
    void Update()
    {
        //Move forward
        transform.position += transform.forward * flySpeed * Time.deltaTime;

        //Inputs
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput == 0) horizontalInput = joystick.Horizontal;
        
        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput == 0) verticalInput = joystick.Vertical*(-1);


        //YAW, PITCH, ROLL
        yaw += horizontalInput * yawAmount * Time.deltaTime;
        float pitch = Mathf.Lerp(0,20,Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);
        float roll = Mathf.Lerp(0,30,Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);
        //Apply Rotation
        transform.localRotation = Quaternion.Euler(Vector3.up * yaw + Vector3.right * pitch + Vector3.forward * roll);

        if (passedThrough == ringWallAmount -1){
            portal.GetComponent<Renderer>().enabled = true;
        }
    }

    void Awake(){
        ringWallAmount = GameObject.FindGameObjectsWithTag("MissZone").Length;
    }


    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Ring")){
            // Destroy(other.gameObject);
            passedThrough++;
            passedThroughRing = true;
        }
        else if(other.CompareTag("MissZone")){
            if(!passedThroughRing){
                StartCoroutine( HitMissZone(other.gameObject));
            }
            else{
                StartCoroutine(PassRing());
            }
        }
    }

    IEnumerator PassRing(){
        yield return new WaitForSeconds(1f);
        passedThroughRing = false;
    }

    IEnumerator HitMissZone(GameObject MissZone){
        audioManager.PlayMissSound();
        Renderer renderer = MissZone.GetComponent<Renderer>();
        Material material = renderer.material;
        material.SetFloat("_Visibility", 1);
        MisedText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        MisedText.SetActive(false);
        Debug.Log("Missed, Restarting");
        SceneManager.LoadScene(currentlevel);
        passedThroughRing = false;
    }
    
}
