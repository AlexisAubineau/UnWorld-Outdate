﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class KingSlime: MonoBehaviour, IEnemy
{
    public LayerMask aggroLayerMask;
    public float currentHealth;
    public float maxHealth;
    public int Experience { get; set; }
    public DropTable DropTable { get; set; }
    public Spawner Spawner { get; set; }
    public PickupItem pickupItem;

    private Player player;
    private UnityEngine.AI.NavMeshAgent navAgent;
    private CharacterStats characterStats;
    private Collider[] withinAggroColliders;

    void Start()
    {
        DropTable = new DropTable();
        DropTable.loot = new List<LootDrop>
        {
            new LootDrop("sword", 4),
            new LootDrop("staff", 4)
        };

        Experience = 200;
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        characterStats = new CharacterStats(5, 10, 2);
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        withinAggroColliders = Physics.OverlapSphere(transform.position, 10, aggroLayerMask);
        if (withinAggroColliders.Length > 0)
        {
            ChasePlayer(withinAggroColliders[0].GetComponent<Player>());
        }
    }

    public void PerformAttack()
    {
        player.TakeDamage(5);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    void ChasePlayer(Player player)
    {
        navAgent.SetDestination(player.transform.position);
        this.player = player;
        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            if (!IsInvoking("PerformAttack"))
                InvokeRepeating("PerformAttack", .5f, 2f);
        }
        else
        {
            CancelInvoke("PerformAttack");
        }
    }

    public void Die()
    {
        DropLoot();
        CombatEvents.EnemyDied(this);
        this.Spawner.Respawn();
        Destroy(gameObject);
    }

    void DropLoot()
    {
        Item item = DropTable.GetDrop();
        if (item != null)
        {
            PickupItem instance = Instantiate(pickupItem, transform.position, Quaternion.identity);
            instance.ItemDrop = item;
        }
    }
}
