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
    public static CharacterData.Status GetBaseStatus(EnemyIds id)
    {
        switch (id)
        {
            case EnemyIds.Goblin:
                return new CharacterData.Status()
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
