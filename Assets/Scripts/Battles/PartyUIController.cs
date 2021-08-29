using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyUIController : MonoBehaviour
{
    public CharacterData _characterData { get; set; }

    private void UpdatePartyUI()
    {
        for (int index = 0; index < MyGameManager.Instance.PartyMembers.Length; ++index)
        {
            var member = MyGameManager.Instance.PartyMembers[index];
            if (member == null) continue;

            var characterStatus = GameObject.Find("StatusCharacter" + index);

            var image = GameObject.Find(characterStatus.name + "/Background/Face");
            image.GetComponent<Image>().sprite = member.Face;

            var levelText = GameObject.Find(characterStatus.name + "/Background/Level");
            levelText.GetComponent<Text>().text = $"Lv {member.BaseStatus.Level.ToString()}";

            var characterText = GameObject.Find(characterStatus.name + "/Background/Name");
            characterText.GetComponent<Text>().text = member.Name;

            var hpText = GameObject.Find(characterStatus.name + "/Background/HP");
            hpText.GetComponent<Text>().text = member.CurrentStatus.HitPoint.ToString();

            var hpGauge = GameObject.Find(characterStatus.name + "/Background/HPGaugeCharacter");
            var hpPointGauge = hpGauge.GetComponent<PointGauge>();
            hpPointGauge.SetGaugeRatio(member.CurrentStatus.HitPoint / member.BaseStatus.HitPoint);

            var mpText = GameObject.Find(characterStatus.name + "/Background/MP");
            mpText.GetComponent<Text>().text = member.CurrentStatus.MagicPoint.ToString();

            var mpGauge = GameObject.Find(characterStatus.name + "/Background/MPGaugeCharacter");
            var mpPointGauge = hpGauge.GetComponent<PointGauge>();
            mpPointGauge.SetGaugeRatio(member.CurrentStatus.MagicPoint / member.BaseStatus.MagicPoint);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Battles/StatusCharacter");

        for (int index = 0; index < MyGameManager.Instance.PartyMembers.Length; ++index)
        {
            var member = MyGameManager.Instance.PartyMembers[index];

            if (member == null) continue;
            var characterStatus = Instantiate(prefab, transform);
            characterStatus.name = "StatusCharacter" + index;
        }

        UpdatePartyUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
