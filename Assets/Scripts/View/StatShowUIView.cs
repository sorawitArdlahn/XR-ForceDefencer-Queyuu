using System.Collections;
using System.Collections.Generic;
using GameController;
using Model.Level;
using Model.Stats;
using TMPro;
using UnityEngine;

namespace View.Preparation {
    public class StatShowUIView : MonoBehaviour
{
    [Header("==== This UI Screen ====")]
    [SerializeField] TextMeshProUGUI HPMultiplierText;
    [SerializeField] TextMeshProUGUI DamageMultiplierText;
    [SerializeField] TextMeshProUGUI ArmorMultiplierText;
    [SerializeField] TextMeshProUGUI FuelMultiplierText;

    PlayerData playerStats;

    void Start()
    {
        playerStats = GameManager.Instance?.currentGameData.playerData;         
    }

    public void UpdateText() {
        HPMultiplierText.text = playerStats.HealthPointMultiplier.ToString("F2");
        DamageMultiplierText.text = playerStats.DamageMultiplier.ToString("F2");
        ArmorMultiplierText.text = playerStats.ArmorMultiplier.ToString("F2");
        FuelMultiplierText.text = playerStats.FuelMultiplier.ToString("F2");
    }

}
}
