using System.Collections;
using System.Collections.Generic;
using Den.Tools.GUI;
using UnityEngine;

public class FearEffectLogic : EffectBase
{
    [Header("Specific Effect Vars")]
    public GameObject createdUIObj;
    public bool activate;

    //public Modifier mod = new Modifier("fearRelic", -.4f, Modifier.ChangeType.PERCENT, true);   
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
                Debug.Log("remove effect object and UI from enemy");

                // Undo the effect to the enemy

                enemy.RemoveEffect(this.effectName, createdUIObj);
            }
        }
    }
    public override void CallEffect()
    {
        //Debug.Log("Add UI to enemy\nAdd mod to enemy");
        createdUIObj.transform.SetParent(enemy.effectCont.uiSatusEffectParent.transform);
        createdUIObj.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.Euler(0,0,0));

        //enemymovemnt already effected when this effect is applied
        // if you want fear to do something else enter it here
        activate = true;
        enemy.movementController.enemyState = EnemyMovementController.EnemyStates.MOVE;

        //change enemy color
        //enemy.spriteR.color = new Color(0.9104829f, 0.5613208f, 1, 1);
    }
}
