using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonuses : MonoBehaviour
{
    public string bonusName;
    public Text coinCount;

    private void Start()
    {
        coinCount.text = PlayerPrefs.GetInt("coins").ToString();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            switch(bonusName)
            {
                case "coin":
                    int coins = PlayerPrefs.GetInt("coins");
                    PlayerPrefs.SetInt("coins", coins + 1);
                    coinCount.text = (coins + 1).ToString();
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
