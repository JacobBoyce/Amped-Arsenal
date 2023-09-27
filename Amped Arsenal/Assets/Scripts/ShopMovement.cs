using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopMovement : MonoBehaviour
{
    [Header("Relevant Objects")]
    public Animator animCont;
    public Rigidbody thisRB;
    public Material standMat, walkMat;
    public MeshRenderer meshRend;
    public TextMeshProUGUI alertUI;

    [Header("Chance Vars")]
    public float waitTime;
    public float waitMin, waitMax; 
    public int waitOrWalkChance, waitFlipChance;
    public int tempWalkChance, tempFlipChance;
    public bool flipFlag = false;

    [Header("Walking Vars")]
    public bool walking = true;
    public float moveSpeed;
    public float walkTimer, walkTimerMin, walkTimerMax;
    public Vector3 moveDir;
    public GameObject runFromObject = null;

    public void Start()
    {
        StartCoroutine("WaitLogic");
    }

    IEnumerator WaitLogic()
    {
        //Enter standing state
        thisRB.isKinematic = true;
        animCont.SetBool("Stand", true);
        meshRend.material = standMat;


        //decide wait time
        waitTime = Random.Range(waitMin,waitMax);
        //wait
        yield return new WaitForSeconds(waitTime);

        tempWalkChance = Random.Range(0,101);
        tempFlipChance = Random.Range(0,101);

        //Choose to stand again
        if(tempWalkChance < waitOrWalkChance)
        {
            if(tempFlipChance < waitFlipChance)
            {
                //flip
                flipFlag = !flipFlag;
                animCont.SetBool("Flip", flipFlag);
                yield return new WaitForSeconds(1.2f);
            }
            StartCoroutine("WaitLogic");
        }
        // Choose to walk
        else
        {
            //decide where and how long to walk
            walkTimer = Random.Range(walkTimerMin, walkTimerMax);
            PickMoveDirection();
            moveDir.y = 0;
            //check what direction its gonna go and which way youre facing, if need to flip then flip
            //if going left and looking right
            if(moveDir.x < 0 && flipFlag == true)
            {
                flipFlag = !flipFlag;
                animCont.SetBool("Flip", flipFlag);
                yield return new WaitForSeconds(1.2f);
            }
            //if going right and looking left
            else if(moveDir.x > 0 && flipFlag == false)
            {
                flipFlag = !flipFlag;
                animCont.SetBool("Flip", flipFlag);
                yield return new WaitForSeconds(1.2f);
            }

            //switch material to walk
            animCont.SetBool("Stand", false);
            meshRend.material = walkMat;
            //activate walking
            thisRB.isKinematic = false;
            walking = true;
        }
    }

    public void Update()
    {
        if(walking)
        {
            walkTimer -= Time.deltaTime;
            //walk logic
            thisRB.velocity = moveDir.normalized * moveSpeed;

            if(walkTimer <= 0)
            {
                walking = false;
                StartCoroutine("WaitLogic");
            }
        }
    }

    public void PickMoveDirection()
    {
        if(runFromObject != null)
        {
            moveDir = runFromObject.transform.position - transform.position;
            moveDir = -moveDir;
        }
        else
        {
            moveDir = Random.onUnitSphere.normalized;
        }
    }
}