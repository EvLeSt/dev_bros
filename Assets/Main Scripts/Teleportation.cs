using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleportation : MonoBehaviour
{
    public string sceneToLoad; // Название сцены для загрузки (из Build Settings)
    public string spawnPointTag = "SpawnPoint"; // Тег точки спавна

    private string currentSceneName;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DontDestroyOnLoad(other.gameObject); // Сохраняем игрока
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneToLoad)
        {
            GameObject spawnPoint = GameObject.FindWithTag(spawnPointTag);
            if (spawnPoint != null)
            {
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    player.transform.position = spawnPoint.transform.position; // Телепорт
                }
            }
            SceneManager.UnloadSceneAsync(currentSceneName); // Выгружаем старую сцену
            currentSceneName = sceneToLoad;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
