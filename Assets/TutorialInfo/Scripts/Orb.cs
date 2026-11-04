using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// STARTER — A collectable orb for the "Orb Collector" lab goal.
/// Attach to a small sphere (scale 0.6), give it a Collider with
/// "Is Trigger" checked, and tag the object "Orb".
///
/// ALREADY WORKS: click-to-recolor via OnMouseDown (GetMouseButtonDown
/// fallback included), a gentle bob + spin every frame, and a
/// gameObject.SetActive(false) hide on trigger entry is handled by the
/// player (see PlayerController TODO 2).
/// </summary>
public class Orb : MonoBehaviour
{
    [Header("Click palette")]
    [SerializeField]
    private Color[] clickColors =
    {
        Color.cyan,
        Color.magenta,
        Color.yellow
    };

    [Header("Bob & spin")]
    [SerializeField] private float bobHeight = 0.15f;
    [SerializeField] private float spinSpeed = 90f;

    private Vector3 basePosition;
    private Vector3 baseScale;
    private int clickIndex;
    private float hideTime = -1f;

    void Awake()
    {
        basePosition = transform.position;
        baseScale = transform.localScale;
    }

    void Update()
    {
        // Bob up and down using Time.time (seconds since Play started).
        float bob = Mathf.Sin(Time.time * 3f) * bobHeight;

        transform.position = basePosition + Vector3.up * bob;

        transform.Rotate(
            Vector3.up,
            spinSpeed * Time.deltaTime,
            Space.World
        );
    }

    private void OnMouseDown()
    {
        // Click the orb's collider → cycle its color.
        if (clickColors == null || clickColors.Length == 0)
            return;

        Renderer orbRenderer = GetComponent<Renderer>();

        if (orbRenderer == null)
            return;

        clickIndex = (clickIndex + 1) % clickColors.Length;

        orbRenderer.material.color = clickColors[clickIndex];

        Debug.Log($"[Orb] Clicked → {clickColors[clickIndex]}");
    }

    private void OnMouseDrag()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            return;
        }

        Vector3 mousePosition = Input.mousePosition;

        float distanceFromCamera =
            mainCamera.WorldToScreenPoint(transform.position).z;

        mousePosition.z = distanceFromCamera;

        Vector3 worldPosition =
            mainCamera.ScreenToWorldPoint(mousePosition);

        transform.position = new Vector3(
            worldPosition.x,
            transform.position.y,
            worldPosition.z
        );

        // Update base position so the orb stays where it was dragged.
        basePosition = new Vector3(
            worldPosition.x,
            basePosition.y,
            worldPosition.z
        );
    }

    // NOTE: The player script collects orbs in its OnTriggerEnter.
    // This script handles appearance, clicking, dragging, and bobbing.
}