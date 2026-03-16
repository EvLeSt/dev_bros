using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    [Header("Настройки")]
    public KeyCode interactKey = KeyCode.E;
    public string playerTag = "Player";
    [Range(0.1f, 10f)]
    public float rotationSpeed = 2f;
    public float openAngle = 90f;
    public Collider doorCollider;

    private bool playerInRange = false;
    private bool isDoorOpen = false;
    private Vector3 closedRotation;
    private Quaternion openRotation;
    private Quaternion targetRotation;

    private void Start()
    {
        closedRotation = transform.eulerAngles;
        openRotation = Quaternion.Euler(closedRotation + new Vector3(0, openAngle, 0));
        targetRotation = transform.rotation;

        if (doorCollider != null)
            doorCollider.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        // Toggle только при игроке в триггере
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            ToggleDoor();
        }

        // ПОВОРОТ ТОЛЬКО при игроке в триггере
        if (playerInRange)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void ToggleDoor()
    {
        isDoorOpen = !isDoorOpen;
        targetRotation = isDoorOpen ? openRotation : Quaternion.Euler(closedRotation);

        if (doorCollider != null)
            doorCollider.enabled = !isDoorOpen;
    }
}
