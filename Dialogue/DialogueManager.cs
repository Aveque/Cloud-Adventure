using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    public Text coinCount;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;

    public Animator boxAnim;
    public Animator startAnim;

    private bool gotQuestReward = false;

    private Queue<string> sentences;

    public GameObject Player;
    public GameObject Controls;

    public DestroyQuestItem destroyQuestItem;
    private PickUp pickUp;


    public Sprite completedQuestNPC;
    public SpriteRenderer NPC;
    private void Start()
    {
        sentences = new Queue<string>();
        pickUp = GameObject.FindGameObjectWithTag("QuestItem").GetComponent<PickUp>();
        destroyQuestItem = GameObject.FindGameObjectWithTag("Slot").GetComponent<DestroyQuestItem>();
        NPC = GameObject.FindGameObjectWithTag("NPC").GetComponent<SpriteRenderer>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (pickUp.questItem)
        {
            destroyQuestItem.DestroyGO();
            NPC.sprite = completedQuestNPC;
            if (!gotQuestReward)
            {
                gotQuestReward = true;
                int coins = PlayerPrefs.GetInt("coins");
                PlayerPrefs.SetInt("coins", coins + 10);
                coinCount.text = (coins + 10).ToString();
            }
        }
        boxAnim.SetBool("boxOpen", true);
        startAnim.SetBool("StartOpen", false);
        Player.GetComponent<Hero>().enabled = false;
        Controls.SetActive(false);

        nameText.text = dialogue.name;
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0) 
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; 
        }
         
    }

    public void EndDialogue()
    {
        boxAnim.SetBool("boxOpen", false);

        Player.GetComponent<Hero>().enabled = true;
        Controls.SetActive(true);
    }
}
