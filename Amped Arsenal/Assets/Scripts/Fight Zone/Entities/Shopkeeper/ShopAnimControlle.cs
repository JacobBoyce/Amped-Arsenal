using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAnimControlle : MonoBehaviour
{
    public SpriteRenderer thisSR;

    public void FlipSprite()
    {
        thisSR.flipX = thisSR.flipX == false ? true : false;
    }
}
