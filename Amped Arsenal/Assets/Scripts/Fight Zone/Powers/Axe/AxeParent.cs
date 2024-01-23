using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeParent : MonoBehaviour
{
    public List<GameObject> AxesPos = new List<GameObject>();
    public float rotationSpeed, distFromPlayer;
    public AxeController controller;
    private List<AxeChild> axes = new List<AxeChild>();



    // Update is called once per frame
    void Update()
    {
        //rotate
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime,0));
    }

    public void InitAxes(AxeController cont, WeaponMods mod, float rotSpeed, float distFP, float spo)
    {
        controller = cont;
        rotationSpeed = rotSpeed;
        distFromPlayer = distFP;

        foreach(GameObject go in AxesPos)
        {
            //have the init spawn the axe then keep it deactive
            //then have a bool set in the child to run on the axe
            go.GetComponent<AxeChild>().InitStuff(controller, mod, spo, distFromPlayer);
            axes.Add(go.GetComponent<AxeChild>());
        }
    }

    public void UpdateAndActivateAxes(float rotSpeed, float spo, float distFP, int axeNum, WeaponMods mod)
    {
        rotationSpeed = rotSpeed;
        distFromPlayer = distFP;
        foreach(AxeChild go in axes)
        {
            go.UpdateValues(spo, distFromPlayer, mod);
        }
        //determine what level, to deceide which axes to turn on.
        ActivateSpecficAxes(axeNum);
    }

    public void TurnOffAxes()
    {
        foreach(AxeChild go in axes)
        {
            go.TurnOffAxe();
        }
    }

    public void ActivateSpecficAxes(int axeNum)
    {
        /*
            0 = Front
            1 = Back
            2 = Right
            3 = Left
            4 = Front/Left
            5 = Back/Right
            6 = Front/Right
            7 = Back/Left
        */

        switch(axeNum)
        {
            case 1:
                axes[0].TurnOnSpawnAxe();
            break;

            case 2:
                axes[0].TurnOnSpawnAxe();
                axes[1].TurnOnSpawnAxe();
            break;

            case 3:
                axes[0].TurnOnSpawnAxe();
                axes[1].TurnOnSpawnAxe();
                axes[2].TurnOnSpawnAxe();
            break;

            case 4:
                axes[0].TurnOnSpawnAxe();
                axes[1].TurnOnSpawnAxe();
                axes[2].TurnOnSpawnAxe();
                axes[3].TurnOnSpawnAxe();
            break;

            case 5:
                axes[0].TurnOnSpawnAxe();
                axes[1].TurnOnSpawnAxe();
                axes[2].TurnOnSpawnAxe();
                axes[3].TurnOnSpawnAxe();
                axes[4].TurnOnSpawnAxe();
            break;

            case 6:
                axes[0].TurnOnSpawnAxe();
                axes[1].TurnOnSpawnAxe();
                axes[2].TurnOnSpawnAxe();
                axes[3].TurnOnSpawnAxe();
                axes[4].TurnOnSpawnAxe();
                axes[5].TurnOnSpawnAxe();
            break;

            case 7:
                axes[0].TurnOnSpawnAxe();
                axes[1].TurnOnSpawnAxe();
                axes[2].TurnOnSpawnAxe();
                axes[3].TurnOnSpawnAxe();
                axes[4].TurnOnSpawnAxe();
                axes[5].TurnOnSpawnAxe();
                axes[6].TurnOnSpawnAxe();
            break;

            case 8:
                axes[0].TurnOnSpawnAxe();
                axes[1].TurnOnSpawnAxe();
                axes[2].TurnOnSpawnAxe();
                axes[3].TurnOnSpawnAxe();
                axes[4].TurnOnSpawnAxe();
                axes[5].TurnOnSpawnAxe();
                axes[6].TurnOnSpawnAxe();
                axes[7].TurnOnSpawnAxe();
            break;
        }   
    }
}
