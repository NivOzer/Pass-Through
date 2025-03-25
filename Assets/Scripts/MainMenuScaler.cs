using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScaler : MonoBehaviour
{
    [SerializeField] private Button PlayButtonUI;
    [SerializeField] private TextMeshProUGUI PlayText;
    [SerializeField] private TextMeshProUGUI descriptionTextUI;
    [SerializeField] private TextMeshProUGUI descriptionTextUI2;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI record;

    void Start()
    {
        AdjustUI();
        Time.timeScale = 0;
        if (GameManager.Instance.record != 0){
            record.text = "" + GameManager.Instance.record;
        }
    }

    void AdjustUI()
    {
        if (Screen.height > Screen.width) // Portrait mode
        {
            // Resize the Play Button
            RectTransform buttonRect = PlayButtonUI.GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(Screen.width * 0.5f, Screen.width * 0.5f);

            float lowerScreenBoundary = Screen.height * 0.50f;

            RectTransform descRect1 = descriptionTextUI.GetComponent<RectTransform>();
            descRect1.anchoredPosition = new Vector2(descRect1.anchoredPosition.x, lowerScreenBoundary * 0.70f);
            descriptionTextUI.fontSize = 50;

            RectTransform descRect2 = descriptionTextUI2.GetComponent<RectTransform>();
            descRect2.anchoredPosition = new Vector2(descRect2.anchoredPosition.x, lowerScreenBoundary * 0.60f);
            descriptionTextUI2.fontSize = 50;

            PlayText.fontSize = 130;


        }
    }
}
