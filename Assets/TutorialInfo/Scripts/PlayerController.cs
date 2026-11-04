
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// STARTER — Player controller for the "Orb Collector" lab goal.
/// Attach to a cube named "Player" (add a Rigidbody, freeze rotation X/Z).
///
/// ALREADY WORKS: WASD movement with GetKey + GetAxis, frame-rate independent
/// via Time.deltaTime. A Debug.Log heartbeat proves Update runs every frame.
///
/// YOUR LAB TASKS live in the TODO comments. Each TODO uses one function
/// from this week's lecture. Complete them all to finish the game.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Lab wiring (drag your objects here)")]
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private Transform cameraTransform;

    private Rigidbody rb;

    // Controls whether the bonus Orb should spawn.
    private bool allowBonusOrb = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Debug.Log("[PlayerController] Ready. Collect all orbs to win.");
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(h, 0f, v).normalized;

        transform.Translate(
            movement * moveSpeed * Time.deltaTime,
            Space.World
        );
    }

    // ---------------------------------------------------------------
    // TODO 1 (OnCollisionEnter): log the name of any wall you bump.
    // TODO 2 (OnTriggerEnter): if other.CompareTag("Orb"), Destroy the orb
    //       and Debug.Log the collection. (Orb prefab already tagged "Orb".)
    // TODO 3 (LateUpdate): move cameraTransform to
    //       player position + offset so it follows you smoothly.
    // TODO 4 (OnMouseDown is on the Orb script, not here — see Orb.cs.)
    // TODO 5 (OnDestroy): when the Player is destroyed, Instantiate one
    //       bonus orbPrefab at your last position.
    // ---------------------------------------------------------------

    // TODO 1: Log the name of any wall you bump.
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("I hit: " + collision.gameObject.name);
    }

    // TODO 2: Collect an orb when the Player touches it.
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Orb"))
            return;

        // Prevent collecting the same Orb twice.
        if (!other.gameObject.activeInHierarchy)
            return;

        other.gameObject.SetActive(false);

        Debug.Log("Orb collected!");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Collect();
        }

        Destroy(other.gameObject);
    }

    // TODO 3: Make the camera follow the Player.
    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            cameraTransform.position =
                transform.position + new Vector3(0f, 8f, -8f);

            cameraTransform.LookAt(transform);
        }
    }

    // TODO 5: Spawn a bonus orb when the Player is destroyed.
    private void OnDestroy()
    {
        // Do not spawn a bonus orb when Unity is closing the scene.
        if (!gameObject.scene.isLoaded || !allowBonusOrb)
            return;

        if (orbPrefab != null)
        {
            Instantiate(orbPrefab, transform.position, Quaternion.identity);
            Debug.Log("Bonus orb spawned!");
        }
    }
}
