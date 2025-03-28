using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerX : MonoBehaviour
{
    [SerializeField] float flySpeed;
    [SerializeField] float yawAmount;
    [SerializeField] GameObject audioObj;
    public GameObject MissedText;
    public Joystick joystick;
    private GameObject portal;
    private AudioManagerX audioManager;
    private bool passedThroughRing = false;
    private float yaw;
    private int ringWallAmount;

    void Start(){
        audioManager = audioObj.GetComponent<AudioManagerX>();   
    }
    void Update()
    {
        Movement();
    }
    private void Movement(){
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
    }

    void Awake(){
        ringWallAmount = GameObject.FindGameObjectsWithTag("MissZone").Length;
    }


    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Ring")){
            passedThroughRing = true;
        }
        else if (other.CompareTag("MissZone"))
        {
            DoubleWall doubleWall = other.GetComponent<DoubleWall>();

            if (doubleWall != null) //double-ring wall
            {
                if (doubleWall.allPassed)
                    StartCoroutine(PassRing());
                else
                    StartCoroutine(HitMissZone(other.gameObject));
            }
            else //regular MissZone
            {
                if (passedThroughRing)
                    StartCoroutine(PassRing());
                else
                    StartCoroutine(HitMissZone(other.gameObject));
            }
        }

        else if(other.CompareTag("End")){
            GameManager.Instance.GameWon();
        }
    }

    IEnumerator PassRing(){
        yield return new WaitForSeconds(1f);
        passedThroughRing = false;
    }

    IEnumerator HitMissZone(GameObject missZone){

        // 🔁 Reset this RingWall if it has one
        DoubleWall wall = missZone.GetComponent<DoubleWall>();
        if (wall != null)
        {
            wall.ResetWall();
        }

        PlayMissFeedback(missZone);
        yield return ShowMissedText(0.5f);

        Debug.Log("Missed, Restarting");
        passedThroughRing = false;
        PortalManager.Instance.ResetToInitialScenes();
        transform.position = new Vector3(0, 0);

        ResetMaterialFlags(missZone);
    }

    private void PlayMissFeedback(GameObject zone){
        audioManager.PlayMissSound();
        var renderer = zone.GetComponent<Renderer>();
        if (renderer != null){
            var material = renderer.material;
            material.SetFloat("_Visibility", 1);
            material.SetInt("_InvokeMissZone",1);
        }
    }
    IEnumerator ShowMissedText(float duration){
        MissedText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        MissedText.SetActive(false);
    }
    private void ResetMaterialFlags(GameObject zone){
        var renderer = zone.GetComponent<Renderer>();
        if (renderer != null){
            var material = renderer.material;
            material.SetInt("_InvokeMissZone",0);
        }
    }



    
}