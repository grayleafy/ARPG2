using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMgr : SingletonBase<TeamMgr>
{
    /// <summary>
    /// 两个阵营是否敌对
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool IsEnemy(TeamType a,  TeamType b)
    {
        if (a == b) return false;
        return true;
    }
}
