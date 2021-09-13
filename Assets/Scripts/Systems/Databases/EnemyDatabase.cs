using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDatabase
{
    public enum EnemyIds : int
    {
        /// <summary>
        /// 
        /// </summary>
        Goblin,
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static EnemyData GetEnemyData(EnemyIds id)
    {
        switch (id)
        {
            case EnemyIds.Goblin:
                return new EnemyData()
                {
                    Name = "ƒSƒuƒŠƒ“",
                    CurrentStatus = GetBaseStatus(id),
                    BaseStatus = GetBaseStatus(id),
                };
        }
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    public static BattlerData.Status GetBaseStatus(EnemyIds id)
    {
        switch (id)
        {
            case EnemyIds.Goblin:
                return new BattlerData.Status()
                {
                    Level = 1,
                    HitPoint = 8,
                    MagicPoint = 0,
                    Attack = 4,
                    Defense = 1,
                    Intelligence = 1,
                    Resist = 1,
                    Agility = 2,
                };
        }

        return null;
    }
}
