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

    public List<Modifier> mods = new List<Modifier>();

    [SerializeField]
    public float Value
    {
        get => _value + CalcMods();
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
    public void IncreaseMaxByPercent(float percent) => Max = Max + (int)(Max * percent);
    public void IncreaseMaxBy(float amount) => Max = (int)(Max + amount);
    public void DecreaseMaxByPercent(float percent) => Max = Max - (int)(Max * percent);  
    public void DecreaseMaxBy(float amount) => Max = (int)(Max - amount);

    //add a list of a data type(modifier)
    //modifier, string, float

    public float CalcMods()
    {
        float addAmt = 0;
        foreach(Modifier mod in mods)
        {
            if(mod.maxMod == false)
            {
                if(mod.modType == Modifier.ChangeType.INT)
                {
                    addAmt += mod.modAmount;
                }
                else if(mod.modType == Modifier.ChangeType.PERCENT)
                {
                    addAmt += (Max * mod.modAmount);
                }
            }
        }
        return 0;
    }

    public void AddMod(string mName, float amt, Modifier.ChangeType ctype, bool mMod)
    {
        Modifier tempMod = new Modifier(mName, amt, ctype, mMod);

        //if the mods max bool is true then add it right away to the max of the stat using the methods above
        if(tempMod.maxMod == true)
        {
            if(tempMod.modType == Modifier.ChangeType.PERCENT)
            {
                IncreaseMaxByPercent(tempMod.modAmount);
            }
            else if(tempMod.modType == Modifier.ChangeType.INT)
            {
                IncreaseMaxBy(tempMod.modAmount);
            }
        }

        mods.Add(tempMod);
    }

    public void RemoveMod(string mName)
    {
        Modifier tempMod = new Modifier();
        tempMod = GetMod(mName);
        //if maxMode is true remove max using methods above
        if(tempMod.maxMod == true)
        {
            if(tempMod.modType == Modifier.ChangeType.PERCENT)
            {
                DecreaseMaxByPercent(tempMod.modAmount);
            }
            else if(tempMod.modType == Modifier.ChangeType.INT)
            {
                DecreaseMaxBy(tempMod.modAmount);
            }
        }

        mods.Remove(tempMod);
    }

    public Modifier GetMod(string mName)
    {
        Modifier findMod = new Modifier();

        foreach(Modifier mod in mods)
        {
            if(mod.modName.Equals(mName))
            {
                findMod = mod;
            }
        }
        //check if its empty
        if(findMod.modName.Equals("empty"))
        {
            Debug.Log("Couldn't find Mod / Mod doesn't exist");
        }
        return findMod;
    }
}

[System.Serializable]
public class Modifier
{
    [SerializeField]
    public enum ChangeType
    {
        NONE,
        INT,
        PERCENT
    };
    [SerializeField]
    public string modName;
    [SerializeField]
    public float modAmount;
    [SerializeField]
    public ChangeType modType;
    [SerializeField]
    public bool maxMod;

    public Modifier()
    {
        modName = "empty";
        modAmount = 0;
        modType = ChangeType.NONE;
        maxMod = false;
    }

    public Modifier(string mName, float amt, Modifier.ChangeType ctype, bool mMod)
    {
        modName = mName;
        modAmount = amt;
        modType = ctype;
        maxMod = mMod;
    }
}