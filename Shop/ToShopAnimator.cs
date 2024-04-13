using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToShopAnimator : MonoBehaviour
{
    public Animator cameraToShopAnim;

    public void toShop()
    {
        cameraToShopAnim.SetBool("toShop", true);
    }

    public void backFromShop()
    {
        cameraToShopAnim.SetBool("toShop", false);
    }
}
