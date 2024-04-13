using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsCount : MonoBehaviour
{
    [SerializeField]public Text coinCount;

    private void Awake()
    {
        coinCount.text = PlayerPrefs.GetInt("coins").ToString();
    }
}
