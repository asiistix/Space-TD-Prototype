using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGuntower : TowerBase
{
    public GameObject bulletPrefab;
    public float BulletSpeed = 10f;

    

    protected override void Update()
    {
        base.Update();

        if(Target != null)
        {
            Vector3 direction = (Target.position - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotatonSpeed);
        }
    }

    protected override void Attack()
    {
        base.Attack();

        if(Target != null && bulletPrefab != null)
        {
            Vector3 direction = (Target.position - transform.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            rb.velocity = direction * BulletSpeed;

            Destroy(bullet, 2f);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f);

        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
