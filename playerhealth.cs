using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_health : MonoBehaviour
{
    public Image healthbar;
    public float currenthealth;
    public float starthealth = 90;
    public int playerLevel = 0;
    public int coinCount = 0;

    public int bulletDamage;

    public AudioSource punchsound;
    public GameObject bullet;
    public Transform spawnpoint;
    public float force = 50f;

    void Start()
    {
        playerLevel = 0;
        SetHealthByLevel();
        currenthealth = starthealth;
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();

        if (currenthealth <= 0)
        {
            SceneManager.LoadScene(2); // Game Over scene index
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            Shoot();
        }
    }


    void UpdateHealthBar()
    {
        if (healthbar != null && starthealth > 0)
        {
            healthbar.fillAmount = currenthealth / starthealth;
        }
    }

    public void TakeDamage(int dmg)
    {
        currenthealth -= dmg;
        if (punchsound != null) punchsound.Play();

        if (currenthealth <= 0)
        {
            currenthealth = 0;
            SceneManager.LoadScene(2); // Game Over scene index
        }
    }

    public void CollectCoin()
    {
        coinCount++;

        if (coinCount % 10 == 0 && playerLevel < 3)
        {
            playerLevel++;
            SetHealthByLevel();
            currenthealth = starthealth; // Full heal on level up
        }
    }

    void SetHealthByLevel()
    {
        if (playerLevel == 0)
        {
            starthealth = 50;
            bulletDamage = 5;
        }
        else if (playerLevel == 1)
        {
            starthealth = 120;
            bulletDamage = 15;
        }
        else if (playerLevel == 2)
        {
            starthealth = 250;
            bulletDamage = 25;
        }
        else if (playerLevel == 3)
        {
            starthealth = 360;
            bulletDamage = 35;
        }
    }

    public void Shoot()
    {
        GameObject bl = Instantiate(bullet, spawnpoint.position, Quaternion.identity);

        Collider bulletCol = bl.GetComponent<Collider>();
        Collider playerCol = GetComponent<Collider>();
        if (bulletCol != null && playerCol != null)
        {
            Physics.IgnoreCollision(bulletCol, playerCol);
        }

        Rigidbody rb = bl.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * force, ForceMode.Impulse);
        }

        bullet blScript = bl.GetComponent<bullet>();
        if (blScript != null)
        {
            blScript.damage = bulletDamage;
        }
    }
}
