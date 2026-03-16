using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartGame : MonoBehaviour, IPointerClickHandler
{
    public string levelName;

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(levelName);
    }
}
