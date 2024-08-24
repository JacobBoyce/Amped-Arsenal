using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class SlowEffectLogic : EffectBase
{
    [Header("Specific Effect Vars")]
    public GameObject createdUIObj;
    public bool activate;

    public Modifier mod = new("slowRelic", -.5f, Modifier.BuffOrDebuff.DEBUFF, Modifier.ChangeType.PERCENT, true);   
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
            if(tickAmtDuration >= tickMaxDuration)
            {
                enemy.RemoveEffect(this.effectName, createdUIObj);
                enemy._stats["spd"].RemoveMod(mod.modName);
            }
        }
    }
    public override void CallEffect()
    {
        //Debug.Log("Add UI to enemy\nAdd mod to enemy");
        //GameObject tempUIEffect = Instantiate(enemy.effectCont.uiStatusEffectPrefab);
        createdUIObj.transform.SetParent(enemy.effectCont.uiSatusEffectParent.transform);
        //createdUIObj.GetComponent<Image>().sprite = uiEffectImg.sprite;
        createdUIObj.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.Euler(0,0,0));

        enemy._stats["spd"].AddMod(mod);
        activate = true;

        //change enemy color
        //enemy.spriteR.color = new Color(0.9104829f, 0.5613208f, 1, 1);
    }
}
