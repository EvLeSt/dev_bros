using UnityEngine;
using System.Collections;

public class ChestOpener : MonoBehaviour
{
    [Header("Animation Settings")]
    public float openSpeed = 2f; // Degrees per second (90° lid rotation)
    public float openAngle = 90f; // Max open angle for lid

    [Header("Interaction")]
    public float interactionDistance = 3f; // Player distance to open
    public KeyCode openKey = KeyCode.E; // Key to open/close

    [Header("Visual/Audio")]
    public GameObject lootPrefab; // Item that appears when opened (optional)
    public AudioSource openSound; // Opening sound (assign AudioSource)

    private bool isOpen = false;
    private Transform lid; // Chest lid child object
    private Transform player; // Player reference

    void Start()
    {
        // Find lid (child named "Lid" or first child)
        lid = transform.Find("Lid");
        if (lid == null)
            lid = transform.GetChild(0);

        // Find player (tag "Player")
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (openSound == null)
            openSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Check interaction range
        if (distance <= interactionDistance)
        {
            // Show UI prompt (in real game use UI Text/Canvas)
            if (Input.GetKeyDown(openKey))
            {
                ToggleChest();
            }
        }
    }

    void ToggleChest()
    {
        isOpen = !isOpen;

        if (openSound) openSound.Play();

        // Show/hide loot
        if (lootPrefab)
            lootPrefab.SetActive(isOpen);

        // Animate lid rotation
        StopCoroutine("AnimateLid");
        StartCoroutine(AnimateLid());
    }

    IEnumerator AnimateLid()
    {
        Quaternion targetRotation = Quaternion.Euler(isOpen ? -openAngle : 0f, 0, 0);
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * openSpeed;
            lid.localRotation = Quaternion.Lerp(lid.localRotation, targetRotation, elapsed);
            yield return null;
        }

        lid.localRotation = targetRotation; // Snap to exact position
    }

    // Debug visualization (green = can interact)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
