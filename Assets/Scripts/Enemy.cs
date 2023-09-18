using System.Collections;
using System.Transactions;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float MaxHealth = 100;
    public float currentHealth;

    public float MoveSpeed;

    public GameObject[] WayPoints;
    public Transform Destination;

    void Start()
    {
        currentHealth = MaxHealth;
    }

    private void OnEnable()
    {
      //  WayPoints = GameObject.FindGameObjectsWithTag("WayPoints");
        Move();
    }

    public void TakeDamage(float Damage)
    {
        currentHealth -= Damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Move()
    {
        StartCoroutine(MoveRoutine());
    }


    IEnumerator MoveRoutine()
    {
        if(Destination == null)
        {
            Destination = WayPoints[0].transform;
        }
        for (int i = 1; i < WayPoints.Length; i++)
        {
            while (Vector3.Distance(transform.position, Destination.position) > .5f)
            {
                yield return null;
                transform.position = Vector3.MoveTowards(transform.position, Destination.position, MoveSpeed);
            }
            Destination = WayPoints[i].transform;
        }
    }
    void Die()
    {
        gameObject.SetActive(false);
    }
}
