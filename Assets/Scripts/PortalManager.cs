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
            AssignOtherPortalFromScene(scene);
        }
    }

    private void AssignOtherPortalFromScene(Scene scene)
    {
        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            if (obj.CompareTag("OtherPortal"))
            {
                OtherPortal = obj.transform;
                Debug.Log($"OtherPortal assigned from scene: {scene.name}");
                return;
            }
        }

        Debug.LogWarning($"No OtherPortal found in scene: {scene.name}");
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
            Debug.Log("Moving from scene " + SceneManager.GetActiveScene().name + " to " + (SceneManager.GetActiveScene().buildIndex + 1));

        }
    }

    public void IncrementLevel(){
        currentLevelIndex++;
        if (currentLevelIndex > GameManager.Instance.record){
            GameManager.Instance.SetRecord(currentLevelIndex);
        }
    }

    public void ResetToInitialScenes(){
        currentLevelIndex = 0;
        OtherPortal = null;
        for (int i= 1;i< SceneManager.sceneCount;i++){
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded){
                SceneManager.UnloadSceneAsync(scene);
            }
        }
        PreloadNextLevel();

        // ✅ Reset the portal trigger script manually
        var portal = GameObject.FindWithTag("Portal");
        if (portal != null)
        {
            portal.GetComponent<PortalCollision>()?.ResetPortalState();
        }

    }
}
