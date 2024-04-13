using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPos : MonoBehaviour
{
    public CheckpointTrigger checkpointTrigger;
    private GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            checkpointTrigger.CheckpointPosition =  Player.transform.position;
            Debug.Log("Checkpoint");
        }
    }
}
