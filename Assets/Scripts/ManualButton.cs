using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualButton : MonoBehaviour
{
    public GameObject manualPanel;

    private void Start()
    {
        if (manualPanel != null)
            manualPanel.SetActive(false);
    }

    public void ManualActive()
    {
        if (manualPanel == null) return;

        bool isActive = !manualPanel.activeSelf;
        manualPanel.SetActive(isActive);
    }
}

