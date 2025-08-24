using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class coincollector : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI missionText;
    public TextMeshProUGUI enemyMissionText;

    public Player_health playerHealth;
    public GameObject[] allEnemies;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
        UpdateEnemyMission();
    }

    private void UpdateUI()
    {
        if (coinText != null && levelText != null && missionText != null && playerHealth != null)
        {
            coinText.text = "Coins: " + playerHealth.coinCount;
            levelText.text = "Level: " + playerHealth.playerLevel;

            int coinsToNextLevel = 10 - (playerHealth.coinCount % 10);
            if (playerHealth.playerLevel == 3)
            {
                missionText.text = "Max level reached!";
            }
            else
            {
                missionText.text = "Collect 10 coins to level up (" + coinsToNextLevel + " remaining)";
            }
        }
    }

    private void UpdateEnemyMission()
    {
        if (enemyMissionText == null || allEnemies == null) return;

        int remaining = 0;
        foreach (GameObject enemy in allEnemies)
        {
            if (enemy != null)
                remaining++;
        }

        if (remaining == 0)
        {
            enemyMissionText.text = "All enemies defeated!";
        }
        else
        {
            enemyMissionText.text = "Defeat all enemies (" + remaining + " remaining)";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            playerHealth.CollectCoin();
            Destroy(other.gameObject);
        }
    }
}
