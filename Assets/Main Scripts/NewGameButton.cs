using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour
{
    public Button newGameButton;  // Ссылка на кнопку (если не на той же)

    void Start()
    {
        // Подписываемся на нажатие кнопки
        if (newGameButton == null)
            newGameButton = GetComponent<Button>();

        newGameButton.onClick.AddListener(StartNewGame);
    }

    public void StartNewGame()
    {
        // Переход на сцену "First Location"
        SceneManager.LoadScene("Scene1");
    }
}