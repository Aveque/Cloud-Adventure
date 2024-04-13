using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyQuestItem : MonoBehaviour
{
    public void DestroyGO()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
