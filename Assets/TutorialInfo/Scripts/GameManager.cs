
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// STARTER — Score and win condition for the "Orb Collector" lab goal.
/// Create an empty GameObject named "GameManager" and attach this script.
///
/// ALREADY WORKS: a public Collect() method the Player calls when it grabs
/// an orb, plus an OnGUI() label showing the live score in the top-left corner.
///
/// YOUR LAB TASKS:
///   TODO 6: in Awake(), find all existing orbs with GameObject.FindGameObjectsWithTag
///           and set totalOrbs from the result.
///   TODO 7: in OnGUI(), when collected == totalOrbs show a big centered
///           "YOU WIN" message instead of the score line.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("State")]
    [SerializeField] private int totalOrbs = 12;

    private int collected;

    private void Awake()
    {
        // Prevent duplicate GameManagers
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // TODO 6: Find all Orbs in the scene
        GameObject[] orbs = GameObject.FindGameObjectsWithTag("Orb");

        totalOrbs = orbs.Length;

        collected = 0;

        Debug.Log("Total Orbs: " + totalOrbs);
    }

    // Increase score when Player collects an Orb
    public void Collect()
    {
        collected++;

        Debug.Log("Collected: " + collected + " / " + totalOrbs);
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();

        style.fontSize = 25;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.UpperLeft;

        // TODO 7: Show YOU WIN when all Orbs are collected
        if (totalOrbs > 0 && collected >= totalOrbs)
        {
            style.fontSize = 50;
            style.normal.textColor = Color.green;
            style.alignment = TextAnchor.MiddleCenter;

            GUI.Label(
                new Rect(0, 0, Screen.width, Screen.height),
                "YOU WIN!",
                style
            );
        }
        else
        {
            // Display current score
            GUI.Label(
                new Rect(15, 15, 250, 50),
                "Orbs: " + collected + " / " + totalOrbs,
                style
            );
        }
    }
}
