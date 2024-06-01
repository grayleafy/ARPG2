using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarPanel : BasePanel
{
    public CharacterStats stats;


    private void Reset()
    {
        destroyOnHide = false;
        uniquePanel = false;
    }

    private void Update()
    {
        FollowCharacter();
    }

    public void Bind(CharacterStats characterStats)
    {
        stats = characterStats;
    }

    public void UnBind()
    {
        stats = null;
    }

    void FollowCharacter()
    {
        Vector3 worldPos = stats.transform.position;
        var screenPos = Camera.main.WorldToScreenPoint(worldPos);
        (transform as RectTransform).position = screenPos;
    }
}
