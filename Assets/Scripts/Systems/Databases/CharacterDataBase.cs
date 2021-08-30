using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase
{
    public enum CharacterIds : int
    {
        /// <summary>
        /// 
        /// </summary>
        Yozo,

        /// <summary>
        /// 
        /// </summary>
        Gregor,
    }

    /// <summary>
    /// 
    /// </summary>
    public static CharacterData.Status GetBaseStatus(CharacterIds id)
    {
        switch (id)
        {
            case CharacterIds.Yozo:
                return new CharacterData.Status()
                {
                    Level = 1,
                    HitPoint = 30,
                    MagicPoint = 25,
                    Attack = 10,
                    Defense = 10,
                    Intelligence = 12,
                    Resist = 13,
                    Agility = 15,
                };

            case CharacterIds.Gregor:
                return new CharacterData.Status()
                {
                    Level = 1,
                    HitPoint = 40,
                    MagicPoint = 10,
                    Attack = 18,
                    Defense = 12,
                    Intelligence = 9,
                    Resist = 10,
                    Agility = 10,
                };
        }
        return null;
    }

    public static GameObject GetBattleCharacter(CharacterIds id)
    {
        switch (id)
        {
            case CharacterIds.Yozo:
                return Resources.Load<GameObject>("Prefabs/Battles/BattleCharacterYozo");

            case CharacterIds.Gregor:
                return Resources.Load<GameObject>("Prefabs/Battles/BattleCharacterGregor");
        }
        return null;
    }
}