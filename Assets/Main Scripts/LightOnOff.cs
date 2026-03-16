using UnityEngine;

public class LightOnOff : MonoBehaviour
{
    [Header("Ссылки на компоненты факела")]
    public ParticleSystem fireParticles; // Эффект горения
    public Light torchLight;             // Источник света

    [Header("Настройки")]
    public KeyCode interactKey = KeyCode.E;
    public string playerTag = "Player";

    private bool playerInRange = false;
    private bool isTorchOn = true; // Начальное состояние: включено

    private void Start()
    {
        // Инициализируем состояние при запуске
        if (fireParticles != null) fireParticles.Play();
        if (torchLight != null) torchLight.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
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
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            ToggleTorch();
        }
    }

    private void ToggleTorch()
    {
        isTorchOn = !isTorchOn; // Переключаем состояние

        if (fireParticles != null)
        {
            if (isTorchOn)
            {
                fireParticles.Play(); // Включаем частицы
            }
            else
            {
                fireParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Выключаем
            }
        }

        if (torchLight != null)
        {
            torchLight.enabled = isTorchOn; // Включаем/выключаем свет
        }
    }
}
