using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlScreen : MonoBehaviour
{
    public void OpenLvl(int lvlID)
    {
        string lvlName = "Level " + lvlID; 
        SceneManager.LoadScene(lvlName);
    }
}
