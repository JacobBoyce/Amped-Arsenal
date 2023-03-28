using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public string actorName;
    [SerializeField]
    public Stats _stats;

    public void Set(string stat, float value) => _stats[stat].Value = value;
}
