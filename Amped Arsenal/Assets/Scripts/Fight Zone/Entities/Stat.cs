using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    public string  _statName;
    [SerializeField]
    float _value;

    [SerializeField]
    public float Max { get; set; }
    [SerializeField]
    public string Name 
    { 
        get => _statName; 
        set => _statName = value; 
    }
    [SerializeField]
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
    public void IncreaseMaxByPercent(float percent)
    {
        Max = Max + (Max * percent);
    }
    public void IncreaseMaxBy(float amount) => Max = (Max + amount);
    public void DecreaseMaxByPercent(float percent) => Max = Max - (Max * percent);  
    public void DecreaseMaxBy(float amount) => Max = (Max - amount);

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
        return addAmt;
    }

    public void AddMod(string mName, float amt, Modifier.ChangeType ctype, bool mMod)
    {
        Modifier tempMod = new Modifier(mName, amt, ctype, mMod);
        float chgAmt = 0;
        //if the mods max bool is true then add it right away to the max of the stat using the methods above
        if(tempMod.maxMod == true)
        {
            if(tempMod.modType == Modifier.ChangeType.PERCENT)
            {
                chgAmt = Max;
                IncreaseMaxByPercent(tempMod.modAmount);
                tempMod.amtChanged = Max - chgAmt;
            }
            else if(tempMod.modType == Modifier.ChangeType.INT)
            {
                IncreaseMaxBy(tempMod.modAmount);
            }
        }

        mods.Add(tempMod);
    }

    public void AddMod(Modifier mod)
    {
        Modifier tempMod = new Modifier(mod);
        float chgAmt = 0;
        //if the mods max bool is true then add it right away to the max of the stat using the methods above
        if(tempMod.maxMod == true)
        {
            if(tempMod.modType == Modifier.ChangeType.PERCENT)
            {
                chgAmt = Max;
                IncreaseMaxByPercent(tempMod.modAmount);
                tempMod.amtChanged = Max - chgAmt;
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
                Max = Max - tempMod.amtChanged;
                //DecreaseMaxByPercent(tempMod.modAmount);
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

    public bool IfExists(string mName)
    {
        Modifier findMod = mods.Find(x=> x.modName == mName);

        if(findMod == null)
        {
            return false;
        }
        else
        {
            return true;
        }
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
    public float amtChanged;

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

    public Modifier(Modifier mod)
    {
        modName = mod.modName;
        modAmount = mod.modAmount;
        modType = mod.modType;
        maxMod = mod.maxMod;
    }
}