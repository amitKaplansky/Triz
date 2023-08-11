using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{

    [SerializeField] private float intensityIncreaseRate = 1.0f / 30.0f; // Intensity increase rate per second
    private List<Fire> allFires = new List<Fire>();


    private int activeFireIndex = 0; // Index of the currently active fire

    private bool isInitialDelayDone = false; // Flag to track if the initial delay is done
    private float initialDelay = 1f;

    private void InitializeFires()
    {
        Fire[] fireObjects = FindObjectsOfType<Fire>();
        allFires.AddRange(fireObjects);

        allFires.Sort((fire1, fire2) => CompareFireNames(fire1.gameObject.name, fire2.gameObject.name));
        foreach (var fire in allFires)
        {
            fire.CurrentIntensity = 0.0f;
        }
    }

    private int CompareFireNames(string name1, string name2)
    {
        // Extract numeric suffixes from the names
        int suffix1 = GetSuffixNumber(name1);
        int suffix2 = GetSuffixNumber(name2);

        // Compare based on the numeric suffixes
        int suffixComparison = suffix1.CompareTo(suffix2);

        // If suffixes are equal, use lexicographic comparison
        if (suffixComparison == 0)
        {
            return string.Compare(name1, name2);
        }

        return suffixComparison;
    }

    private int GetSuffixNumber(string name)
    {
        int openParenIndex = name.LastIndexOf('(');
        int closeParenIndex = name.LastIndexOf(')');

        if (openParenIndex != -1 && closeParenIndex != -1 && closeParenIndex > openParenIndex)
        {
            string suffixStr = name.Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1);
            int suffix;
            if (int.TryParse(suffixStr, out suffix))
            {
                return suffix;
            }
        }

        return int.MaxValue; // Default value for no suffix or invalid suffix
    }

    private void Update()
    {
        if (!isInitialDelayDone)
        {
            if (Time.time >= initialDelay)
            {
                InitializeFires(); // Start initializing the fires after the initial delay
                isInitialDelayDone = true; // Mark the initial delay as done
            }
        }
        if (activeFireIndex < allFires.Count)
        {

            Fire currentFire = allFires[activeFireIndex].GetComponent<Fire>();

            if (currentFire.CurrentIntensity < 1.0f)
            {
                IncreaseFireIntensity();
            }
            else
            {
                activeFireIndex++; // Move to the next fire
            }
        }
    }

    private void IncreaseFireIntensity()
    {

        if (activeFireIndex < allFires.Count)
        {
            Fire currentFire = allFires[activeFireIndex].GetComponent<Fire>();

            if (currentFire.CurrentIntensity < 1.0f)
            {
                currentFire.CurrentIntensity += intensityIncreaseRate * Time.deltaTime;
            }
            else
            {
                activeFireIndex++; // Move to the next fire
            }
        }

    }

}
