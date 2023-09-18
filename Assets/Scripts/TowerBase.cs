using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TowerBase : MonoBehaviour
{
    public float Range = 5f;
    public float AttackCooldown = 1f;
    public float Damage = 10f;


    public Transform Target;
    float AttackTimer = 0f;
    public float RotatonSpeed = 5f;

    public bool IsSearchingForTarget = false;
    float searchTimer = 0f;
    float maxsearchTimer = 2f;
    float Delay = 0f;
    float MaxDelay = 3f;
    public List<Transform> TargetPoints;

    Coroutine AutoRotate;

    protected virtual void Update()
    {
        if (Target == null && FindNearestEnemy() == null)
        {
            if (!IsSearchingForTarget)
            {
                IsSearchingForTarget = true;
                searchTimer = maxsearchTimer;
                Delay = 0f;
            }


            if (IsSearchingForTarget)
            {
                if (Delay == 0)
                {
                    if (AutoRotate == null)
                    {
                        AutoRotate = StartCoroutine(RotateTurretRandomly());
                    }
                    else if (AutoRotate != null)
                    {
                        StopCoroutine(AutoRotate);
                        AutoRotate = StartCoroutine(RotateTurretRandomly());
                    }
                }

                Delay += Time.deltaTime;
                if (Delay >= MaxDelay)
                {
                    Delay = 0;
                }
            }
            if (searchTimer <= 0f)
            {
                IsSearchingForTarget = false;
            }
            searchTimer -= Time.deltaTime;

            return;
        }

        if (Target == null || Vector3.Distance(transform.position, Target.position) < Range)
        {
            Target = FindNearestEnemy();
        }
        else if (Target != null && Vector3.Distance(transform.position, Target.position) > Range)
        {
            Target = null;
        }

        if (Target != null)
        {
            if (AttackTimer <= 0f)
            {
                Attack();
                AttackTimer = 1f / AttackCooldown;
            }
            AttackTimer -= Time.deltaTime;
        }
    }

    IEnumerator RotateTurretRandomly()
    {
        yield return null;

        int randomIndex = Random.Range(0, TargetPoints.Count);

        while (IsSearchingForTarget)
        {
            yield return null;

            Vector3 randomRotation = TargetPoints[randomIndex].position;

            Vector3 direction = (randomRotation - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotatonSpeed);
            yield return new WaitForSeconds(.5f);
        }
        if (AutoRotate != null)
        {
            StopCoroutine(AutoRotate);
            AutoRotate = null;
        }
    }
    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                nearestEnemy = enemy.transform;
            }
        }
        if (nearestEnemy!= null && Vector3.Distance(nearestEnemy.position, transform.position) < Range)
        {
            IsSearchingForTarget = false;
            return nearestEnemy;
        }
        else
        {
            return null;
        }
    }

    protected virtual void Attack()
    {
        if (Target != null)
        {
            Enemy enemy = Target.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
                if (enemy.currentHealth <= 0)
                {
                    Target = null;
                }
            }
        }
    }
}
