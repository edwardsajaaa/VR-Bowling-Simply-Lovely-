using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    // Nama scene yang ingin dituju
    public string sceneName;

    // Method untuk berpindah scene berdasarkan nama
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Nama scene tidak diisi!");
        }
    }

    // Method untuk berpindah scene dengan parameter nama
    public void LoadSceneByName(string targetSceneName)
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogWarning("Nama scene tidak valid!");
        }
    }

    // Method untuk berpindah scene berdasarkan index
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Method untuk reload scene yang sedang aktif
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Method untuk keluar dari aplikasi
    public void QuitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        Debug.Log("Aplikasi ditutup");
    }
}
