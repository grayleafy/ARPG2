using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    HealthBarPanel healthBarPanel;
    private void OnEnable()
    {
        UIMgr.GetInstance().ShowPanel<HealthBarPanel>("HealthBarPanel", panel =>
        {
            panel.Bind(GetComponent<CharacterStats>());
            healthBarPanel = panel;
        });
    }

    private void OnDisable()
    {
        if (healthBarPanel != null)
        {
            healthBarPanel.HideSelf();
        }
        healthBarPanel = null;
    }
}
