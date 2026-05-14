using System;
using UnityEngine;
using UObject = UnityEngine.Object;

/// <summary>
/// Spawns in enemies that can damage each other
/// </summary>
public static class EnemySpawningUtils
{
	/// <summary>
	/// Spawns in an enemy at the player's position
	/// </summary>
	/// <param name="enemyGO"></param>
	public static GameObject SpawnEnemy(GameObject enemyGO)
	{
        // Instantiate enemy
        GameObject newEnemy = UObject.Instantiate(enemyGO,
            HeroController.instance.transform.position, Quaternion.identity);
        newEnemy.SetActive(true);

        // Create a child object which will handle dealing damage to other enemies
        // This is done because objects on the enemy layer don't collide with each other
        GameObject damageEnemiesGO = new GameObject("Damage Enemies");
        damageEnemiesGO.transform.SetParent(newEnemy.transform, false);
        damageEnemiesGO.layer = 17; // Hero attack layer

        // Let this enemy damage other enemies
        DamageEnemies damageEnemies = damageEnemiesGO.AddComponent<DamageEnemies>();
        damageEnemies.damageDealt = 10;
        damageEnemies.ignoreInvuln = false;

        // Give this child object a collider to interact with other enemies
        Collider2D parentCol = newEnemy.GetComponent<Collider2D>();
        Collider2D childCol = parentCol.CopyCollider2DTo(damageEnemiesGO);
        childCol.isTrigger = true; // Make sure this doesn't collide with the ground

        // Log statement
        Modding.Logger.Log($"[EnemiesFightingEachOther] Spawning {newEnemy.name}");

        return newEnemy;
    }
}
