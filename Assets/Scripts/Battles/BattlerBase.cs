using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦闘するキャラクター基底クラス
/// </summary>
public class BattlerBase : MonoBehaviour
{

    public void UpdateBase()
    {
        if (_isActive)
        {
            var model = GameObject.Find(name + "/Geometry/Armature_Mesh");
            model.GetComponent<Material>().color = new Color(1.0f, 1.0f, 1.0f, Mathf.Sin(Time.time) / 2 + 0.5f);
        }
    }

    private bool _isActive;
}
