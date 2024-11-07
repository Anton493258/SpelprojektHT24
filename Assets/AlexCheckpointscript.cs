using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlexCheckpointscript : MonoBehaviour
{
    AlexGameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("Player").GetComponent<AlexGameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameController.UpdateCheckpoint(transform.position);
        }

    }

}
