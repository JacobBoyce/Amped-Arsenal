using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMods : MonoBehaviour
{
    public bool giveKnockback;
    public int knockbackModAmount;

    public enum ElementType
    {
        NONE,
        FIRE,
        ICE,
        ELECTRIC
    };
    public ElementType eleType;
}
