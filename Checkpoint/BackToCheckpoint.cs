using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToCheckpoint : MonoBehaviour
{
    public CheckpointTrigger checkpointTrigger;
    private GameObject Player;
    [SerializeField] private ParticleSystem dust3;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerToCheckpoint();
        }

    }

    public void PlayerToCheckpoint()
    {
        dust3.Play();
        Player.transform.position = checkpointTrigger.CheckpointPosition;
    }
}
