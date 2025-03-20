using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject MainMenuUI;
    public GameObject verticalPauseMenuUI;
    public RectTransform joyStickArea;
    [SerializeField] Button pauseButton;
    RectTransform canvasRect;
    private bool mobile = false;
    void Start()
    {
        #region Mobile detection
            if (Screen.height > Screen.width){
                pauseButton.gameObject.SetActive(true);
                PauseMenuUI = verticalPauseMenuUI;
                mobile = true;
            }
        #endregion
        // Setting the Joystick sizing
        canvasRect = GetComponent<RectTransform>();
        joyStickArea.sizeDelta = new Vector2(canvasRect.rect.width, canvasRect.rect.height);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    public void Resume(){
        if (mobile){ 
            pauseButton.gameObject.SetActive(true);
        }
        PauseMenuUI.SetActive(false);
        MainMenuUI.SetActive(false); //O(1)
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause(){
        if (mobile){ 
            pauseButton.gameObject.SetActive(false);
        }
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Application.Quit();
    }
}
