using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Ai_controller : MonoBehaviour
{
    int currentwp = 0;
    public GameObject[] waypoint;
    NavMeshAgent agent;
    Animator anim;
    public Image Ehealthbar;
    public GameObject player;

    private Player_health playerScript;

    public int enemyLevel = 1;
    private float startEhealth;
    private float enemyhealth;
    private int damage;

    private float attackCooldown = 1.5f; // Cooldown time between attacks
    private float lastAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        playerScript = player.GetComponent<Player_health>();

        // Setting health and damage by level
        if (enemyLevel == 1)
        {
            startEhealth = 100;
            damage = 10;
        }
        else if (enemyLevel == 2)
        {
            startEhealth = 200;
            damage = 20;
        }
        else if (enemyLevel == 3)
        {
            startEhealth = 300;
            damage = 30;
        }

        enemyhealth = startEhealth;
        if (Ehealthbar != null)
            Ehealthbar.fillAmount = 1f;
    }

    void Update()
    {
        if (enemyhealth <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (Ehealthbar != null)
            Ehealthbar.fillAmount = enemyhealth / startEhealth;

        float distToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distToPlayer > 10f)
        {
            Patrol();
        }
        else
        {
            if (distToPlayer < 1.5f)
            {
                anim.SetInteger("attacking", 5);

                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    playerScript.TakeDamage(damage);
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                anim.SetInteger("attacking", -5);
                agent.SetDestination(player.transform.position);
            }
        }
    }

    void Patrol()
    {
        if (waypoint.Length == 0) return;

        if (Vector3.Distance(transform.position, waypoint[currentwp].transform.position) < 1f)
        {
            currentwp = (currentwp + 1) % waypoint.Length;
        }

        agent.SetDestination(waypoint[currentwp].transform.position);
    }

    public void TakeDamage(int dmg)
    {
        enemyhealth -= dmg;
        if (enemyhealth < 0) enemyhealth = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            TakeDamage(playerScript.bulletDamage);
            Destroy(collision.gameObject);
        }
    }
}
