using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    float _value;

    [SerializeField]
    public int Max { get; set; }
    [SerializeField]
    public string Name { get; set; }

    [SerializeField]
    public float Value
    {
        get => _value;
        set
        {
            if (value < 0)
            {
                Empty();
            }
            else
            {
                _value = value > Max ? Max : value;
            }
        }
    }

    public void Fill() => Value = Max;
    public void Empty() => Value = 0;
    public void IncreaseMaxBy(float percent) => Max = (int)(Max * percent);
    public void IncreaseMaxBy(int amount) => Max = (int)(Max + amount);
    public void DecreaseBy(float percent) => Max = (int)(Max * percent);  
}