using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
  
    [SerializeField] private string sceneToLoad;

  
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("SceneLoader: 'sceneToLoad' is empty or null.");
        }
    }

   
    public void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("SceneLoader: 'sceneName' argument is empty or null.");
        }
    }

  

    
    public void ExitGame()
    {
        Debug.Log("SceneLoader: ExitGame called.");

        Application.Quit();

#if UNITY_EDITOR
        
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
