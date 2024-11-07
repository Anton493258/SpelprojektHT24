using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlexGameController : MonoBehaviour
{
    Vector2 checkpointPos;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        checkpointPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    public void UpdateCheckpoint(Vector2 pos)
    {
        checkpointPos = pos; 
    }
    void Die()
    {
        StartCoroutine(Respawn(0.5f));
    }

   
    IEnumerator Respawn(float duration)
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(duration);
        transform.position = checkpointPos;
        spriteRenderer.enabled = true; 
    }
}
