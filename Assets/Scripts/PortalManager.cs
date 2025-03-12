using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalManager : MonoBehaviour
{
    public static PortalManager Instance { get; private set; }

    // public Transform Portal { get; private set; }
    public Transform OtherPortal { get; private set; }

    private int currentLevelIndex = 0;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PreloadNextLevel();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // public void SetPortals(Transform portal, Transform otherPortal)
    // {
    //     // Portal = portal;
    //     OtherPortal = otherPortal;
    // }

    public void PreloadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;
        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadSceneAsync(nextLevelIndex, LoadSceneMode.Additive);
            Debug.Log($"Preloaded level: {nextLevelIndex}");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Additive)
        {
            Debug.Log($"New scene loaded: {scene.name}");

            // Search ONLY inside the newly loaded scene
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                if (obj.CompareTag("OtherPortal"))
                {
                    OtherPortal = obj.transform;
                    Debug.Log($"OtherPortal assigned from scene: {scene.name}");
                    return; // Stop searching after finding it
                }
            }

            Debug.LogWarning($"No OtherPortal found in scene: {scene.name}");
        }
    }





    public void TeleportPlayerToOtherPortal(Transform player,Transform Portal)
    {
        if (Portal != null && OtherPortal != null)
        {
            // Move player from "Portal" to "OtherPortal"
            Vector3 offset = player.position - Portal.position;
            player.position = OtherPortal.position + offset;

            // Adjust rotation if necessary
            float rotationDiff = -Quaternion.Angle(Portal.rotation, OtherPortal.rotation) + 180;
            player.Rotate(Vector3.up, rotationDiff);

            Debug.Log("Player teleported to OtherPortal.");
        }
    }

    public void IncrementLevel(){
        currentLevelIndex++;
    }
}
