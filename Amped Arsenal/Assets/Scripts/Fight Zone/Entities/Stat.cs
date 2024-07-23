using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
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
    public List<Modifier> mods = new();

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

    public void IncreaseByPercent(float percent) => _value += _value * percent;
    public void IncreaseByAmount(float amount) => _value += amount;
    public void IncreaseMaxByPercent(float amount, bool wantFill, bool wantAdd)
    {
        Max += Max * amount;
        
        if(wantFill)
        {
            Fill();
        }
        else if(wantAdd)
        {
            IncreaseByAmount(Max * amount);
        }
    }
    public void IncreaseMaxByAmount(float amount, bool wantFill, bool wantAdd)
    {
        Max += amount;
        if(wantFill)
        {
            Fill();
        }
        else if(wantAdd)
        {
            IncreaseByAmount(amount);
        }
    }

    public void DecreaseMaxBy(float amount)
    {
        float temp = Max - amount;
        if(temp > Max)
        {
            Max -= amount;
            _value = Max;
        }
        else
        {
            Max -= amount;
        }
    } 
    public void AddMod(Modifier mod)
    {
        Modifier tempMod = new(mod);

        if(tempMod.modType == Modifier.ChangeType.PERCENT)
        {
            tempMod.amtChanged = _value * tempMod.modAmount;
            IncreaseMaxByPercent(tempMod.modAmount,false,true);
        }
        else if(tempMod.modType == Modifier.ChangeType.INT)
        {
            tempMod.amtChanged = tempMod.modAmount;
            IncreaseMaxByAmount(tempMod.modAmount,false,true);
        }
        mods.Add(tempMod);
    }

    public void RemoveMod(string mName)
    {
        Modifier tempMod = new();
        tempMod = GetMod(mName);
        _value -= tempMod.amtChanged;
        if(tempMod.isMaxMod)
        {
            DecreaseMaxBy(tempMod.amtChanged);
        }

        mods.Remove(tempMod);
    }

    public Modifier GetMod(string mName)
    {
        Modifier findMod = new();

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
    public bool isMaxMod = false;

    [SerializeField]
    public float amtChanged;

    [SerializeField]
    public ChangeType modType;
    [SerializeField]

    public Modifier()
    {
        modName = "empty";
        modAmount = 0;
        modType = ChangeType.NONE;
    }

    public Modifier(string mName, float amt, Modifier.ChangeType ctype, bool mMod)
    {
        modName = mName;
        modAmount = amt;
        modType = ctype;
        isMaxMod = mMod;
    }

    public Modifier(Modifier mod)
    {
        modName = mod.modName;
        modAmount = mod.modAmount;
        modType = mod.modType;
    }
}