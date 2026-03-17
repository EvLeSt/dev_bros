using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour
{
    [Header("Animation Settings")]
    public float openSpeed = 2f; // Degrees per second
    public float openAngle = 90f; // Open angle for door
    public bool openForward = true; // true = forward (Z), false = sideways (Y)

    [Header("Interaction")]
    public float interactionDistance = 3f; // Player distance to open
    public KeyCode openKey = KeyCode.E; // Key to open/close

    [Header("Visual/Audio")]
    public AudioSource openSound; // Door sound (optional)

    private bool isOpen = false;
    private Transform door; // Door leaf (child or self)
    private Transform player; // Player reference
    private Vector3 closedPosition; // Initial position
    private Quaternion closedRotation; // Initial rotation

    void Start()
    {
        // Door = child "Door" or first child, or self
        door = transform.Find("Door");
        if (door == null && transform.childCount > 0)
            door = transform.GetChild(0);
        if (door == null)
            door = transform;

        // Save initial closed state
        closedPosition = door.localPosition;
        closedRotation = door.localRotation;

        // Find player
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (openSound == null)
            openSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionDistance)
        {
            if (Input.GetKeyDown(openKey))
            {
                ToggleDoor();
            }
        }
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;

        if (openSound) openSound.Play();

        StopCoroutine("AnimateDoor");
        StartCoroutine(AnimateDoor());
    }

    IEnumerator AnimateDoor()
    {
        Quaternion targetRotation;

        if (openForward)
        {
            // Open forward/back (Z axis) - for double doors
            targetRotation = Quaternion.Euler(0, isOpen ? openAngle : 0f, 0);
        }
        else
        {
            // Open sideways (Y axis) - standard door
            targetRotation = Quaternion.Euler(0, isOpen ? openAngle : 0f, 0);
        }

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * openSpeed;
            door.localRotation = Quaternion.Lerp(door.localRotation, targetRotation, elapsed);
            yield return null;
        }
        door.localRotation = targetRotation;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
