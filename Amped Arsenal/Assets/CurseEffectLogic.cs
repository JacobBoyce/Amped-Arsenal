 using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine;

public class CurseEffectLogic : EffectBase
{
    [Header("Specific Effect Vars")]
    public GameObject createdUIObj;
    public Color damageColor;
    public float intensity;
    public bool activate;

    public Modifier modStr = new("curseRelicSTR", -.3f, Modifier.BuffOrDebuff.DEBUFF, Modifier.ChangeType.PERCENT, true);   
    public Modifier modDef = new("curseRelicDEF", -.3f, Modifier.BuffOrDebuff.DEBUFF, Modifier.ChangeType.PERCENT, true); 
    public void Start()
    {
        TickSystem.OnSubTick += delegate (object sender, TickSystem.OnTickEventArgs e) 
        {
            tickAmtDuration++;
        };
    }
    public void Update()
    {
        if(activate == true)
        {
            if(tickAmtDuration == tickMaxDuration)
            {
                enemy._stats["str"].RemoveMod(modStr.modName);
                enemy._stats["def"].RemoveMod(modDef.modName);
                //enemy._stats["def"].RemoveMod(mod.modName);
                enemy.RemoveEffect(this.effectName, createdUIObj);
            }
        }
    }
    public override void CallEffect()
    {
        //set effect damage (scales with weapon)
                //20      *  .5 = 10
        //damage = weap dmg * poison damage
        //createdUIObj = Instantiate(enemy.effectCont.uiStatusEffectPrefab);
        createdUIObj.transform.SetParent(enemy.effectCont.uiSatusEffectParent.transform);
        //createdUIObj.GetComponent<Image>().sprite = uiEffectImg.sprite;
        createdUIObj.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.Euler(0,0,0));

        enemy._stats["str"].AddMod(modStr);
        enemy._stats["def"].AddMod(modDef);
        //enemy._stats["def"].AddMod(mod);
        activate = true;

        //change enemy color
        //enemy.spriteR.color = new Color(0.9104829f, 0.5613208f, 1, 1);
    }
}
