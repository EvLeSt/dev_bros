using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 0.15f; // Degrees per second (full cycle in 40 minutes)

    [Header("Lighting Settings")]
    public Light sunLight; // Directional Light (assign if not on this object)
    public float dayIntensity = 1.5f; // Day light intensity
    public float nightIntensity = 0.3f; // Night light intensity
    public Color dayColor = Color.white; // Day light color
    public Color nightColor = Color.yellow; // Night light color (sunset/sunrise)

    [Header("Skybox Settings")]
    public Material daySkybox; // Day skybox material
    public Material nightSkybox; // Night skybox material
    public float nightAngle = 180f; // Angle when night starts (sun below horizon)

    private float angleX; // Current X angle (0-360)

    void Start()
    {
        DontDestroyOnLoad(gameObject); // Keep sun synced across scenes

        if (sunLight == null)
            sunLight = GetComponent<Light>();

        // Initialize skybox
        RenderSettings.skybox = daySkybox;
    }

    void Update()
    {
        // 1. Rotate with angle limit (0-360)
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        angleX = transform.rotation.eulerAngles.x % 360f;

        // 2. Lighting based on angle
        if (angleX > nightAngle)
        {
            // Night
            sunLight.intensity = Mathf.Lerp(sunLight.intensity, nightIntensity, Time.deltaTime * 2f);
            sunLight.color = Color.Lerp(sunLight.color, nightColor, Time.deltaTime * 2f);
        }
        else
        {
            // Day
            sunLight.intensity = Mathf.Lerp(sunLight.intensity, dayIntensity, Time.deltaTime * 2f);
            sunLight.color = Color.Lerp(sunLight.color, dayColor, Time.deltaTime * 2f);
        }

        // 3. Skybox switching
        if (angleX > nightAngle && RenderSettings.skybox != nightSkybox)
        {
            RenderSettings.skybox = nightSkybox;
            DynamicGI.UpdateEnvironment();
        }
        else if (angleX <= nightAngle && RenderSettings.skybox != daySkybox)
        {
            RenderSettings.skybox = daySkybox;
            DynamicGI.UpdateEnvironment();
        }
    }
}
