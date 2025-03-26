using System.Collections;
using System.Collections.Generic;
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

    RobotInGameStats playerStats;

    void Start()
    {
        playerStats = gameObject.GetComponent<RobotInGameStats>();
        UpdateText();         
    }

    public void UpdateText() {
        HPMultiplierText.text = playerStats.data.HealthPointMultiplier.ToString("F2");
        DamageMultiplierText.text = playerStats.data.MovementSpeedMultiplier.ToString("F2");
        ArmorMultiplierText.text = playerStats.data.ArmorMultiplier.ToString("F2");
        FuelMultiplierText.text = playerStats.data.FuelMultiplier.ToString("F2");
    }

}
}
