using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System.IO;
using System.Data.Common;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    [SerializeField] private GameObject player;
    private GameObject playerClone;
    private bool cloneCreated = false;
    private Vector3 cloneCreatedPosition;
    private float lastTapTime = 0;
    private float doubleTapTimeBetween = 0.3f;
    private bool doubleTapped = false;
    public int record = 1;
    [SerializeField] private TextMeshProUGUI highestLevelText;
    [SerializeField] private GameObject GameWonMenu;
    private void Awake()
    {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
        LoadScore();
        highestLevelText.text = "Highest Level : " + record;
    }

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

    public void ResetScore(){
        SetRecord(0);
    }

    public void SetRecord(int level){
        record = level;
        highestLevelText.text = "Highest Level : " + record;
        SaveScore();
    }

    public void GameWon(){
        GameWonMenu.SetActive(true);
    }
    
    [System.Serializable]
    class SaveData{
        public int score;
    }

    public void SaveScore(){
        SaveData data = new SaveData();
        data.score = record;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log(json);
        Debug.Log("Was Saved");
    }

    public void LoadScore(){
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path)){
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            record = data.score;
            Debug.Log(data.score + "has been loaded");
        }
        else{
            Debug.Log("didnt find path to load");
        }
    }
}
