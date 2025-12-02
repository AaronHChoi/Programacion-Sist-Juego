using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CursorManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image cursorImage;
    [SerializeField] private Canvas cursorCanvas;

    [Header("Cursor Settings")]
    [SerializeField] private Vector2 cursorSize = new Vector2(64, 64); // FORCE the size here!
    [SerializeField] private float rotationSpeed = 200f;

    [Header("Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite crosshairSprite;
    [SerializeField] private Sprite reloadSprite;

    [Header("States")]
    [SerializeField] private List<string> combatStates = new List<string> { "Shooting", "Aiming" };

    private Simulacro.PlayerMovement player;
    private string currentStateName = "Idle";
    private Sprite currentActiveSprite;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        // Force the size immediately on start
        if (cursorImage != null)
        {
            cursorImage.rectTransform.sizeDelta = cursorSize;
        }
    }

    void OnEnable()
    {
        player = FindFirstObjectByType<Simulacro.PlayerMovement>();
        if (player != null) player.OnStateChanged += HandlePlayerStateChanged;
    }

    void OnDisable()
    {
        if (player != null) player.OnStateChanged -= HandlePlayerStateChanged;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        MoveCursorImage();

        bool isReloading = AmmoManager.Instance != null && AmmoManager.Instance.IsReloading;

        if (isReloading)
        {
            SetCursor(reloadSprite);
            RotateCursor();
        }
        else
        {
            ResetRotation();

            if (combatStates.Contains(currentStateName))
            {
                SetCursor(crosshairSprite);
            }
            else
            {
                SetCursor(normalSprite);
            }
        }
    }

    private void MoveCursorImage()
    {
        if (cursorImage != null)
        {
            cursorImage.transform.position = Input.mousePosition;
        }
    }

    private void RotateCursor()
    {
        if (cursorImage != null)
        {
            cursorImage.transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
        }
    }

    private void ResetRotation()
    {
        if (cursorImage != null)
        {
            cursorImage.transform.rotation = Quaternion.identity;
        }
    }

    private void SetCursor(Sprite newSprite)
    {
        if (cursorImage == null || newSprite == null) return;

        if (currentActiveSprite != newSprite)
        {
            cursorImage.sprite = newSprite;
            // WE REMOVED SetNativeSize() so it doesn't resize based on the file!
            // Instead, we ensure the size is correct:
            cursorImage.rectTransform.sizeDelta = cursorSize;

            currentActiveSprite = newSprite;
        }
    }

    private void HandlePlayerStateChanged(string stateName)
    {
        currentStateName = stateName;
    }
}