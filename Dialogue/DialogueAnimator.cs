using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimator : MonoBehaviour
{
    public Animator startAnim;
    public Animator startAnimQuest;
    public DialogueManager dm;
    private PickUp pickUp;

    private void Start()
    {
        pickUp = GameObject.FindGameObjectWithTag("QuestItem").GetComponent<PickUp>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!pickUp.questItem)
            {
                startAnim.SetBool("StartOpen", true);
            }
            else
            {
                startAnimQuest.SetBool("StartOpen", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!pickUp.questItem)
            {
                startAnim.SetBool("StartOpen", false);
            }
            else
            {
                startAnimQuest.SetBool("StartOpen", false);
            }
            dm.EndDialogue();
        }
    }
}
