using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    float speed = 4;
    private bool isFacingLeft = true;
    Vector3 dir = new Vector3(-1, 0, 0);
    public int enemyHP = 100;
    public CapsuleCollider2D attackCollider;
    // Start is called before the first frame update
    void Start()
    {
      attackCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
        if (enemyHP <= 0)
        {
            Debug.Log("Enemy Dead");
            EnemyDie();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PatrolBarrier"))
        {
            dir.x *= -1;
            Flip();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("PlayerAttack"))
        {
            Debug.Log("enemy hit");
            enemyHP -= 50;
            
        }
    }
    private void EnemyDie()
    {
        Destroy(gameObject);
    }
    private void Flip()
    {
        isFacingLeft = !isFacingLeft;
        transform.Rotate(0f, 180f, 0f);
    }
}
