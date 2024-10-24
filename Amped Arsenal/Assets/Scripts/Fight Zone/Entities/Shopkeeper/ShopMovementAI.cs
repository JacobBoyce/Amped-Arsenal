using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMovementAI : MonoBehaviour
{
    public Vector3 moveDir, lastDir, runAwayDir;
    public bool runAway, inPlayerRange, finishMovedToPlayer;
    //look dir true = looking right
    //         false = looking left

    public Animator animCont;
    public Rigidbody thisRB;
    public SpriteRenderer thisSR;
    public TextMeshProUGUI alertUI;

    public float moveSpeed;
    public int randomNum, enemyCount;

    public enum ShopStates
    {
        STAND,
        WALK,
        MOVEAWAY,
        MOVETO
    }

    public ShopStates shopState;

    [Header("If Hell")]
    public float standingTimer;
    public float standingTimerMax, walkingTimer, walkingTimerMax, moveToTimer, moveToTimerMax;
    public bool startMoveToPlayerInit;

    private GameObject playerPos, runFromPos;

    public int walkBlinkMod, standBlinkMod;

    public void Start()
    {
        //StartCoroutine(StandStill());
        SwitchToStanding();

    }

    void Update()
    {
        switch (shopState)
        {
            case ShopStates.STAND:
                if(inPlayerRange)
                {
                    //switch to moving to player
                    shopState = ShopStates.MOVETO;
                    moveToTimer = 0;
                    break;
                }


                if(standingTimer < standingTimerMax)
                {
                    standingTimer += Time.deltaTime;
                }
                else
                {
                    standingTimer = 0;
                    //decide to walk or stand again

                    randomNum = Random.Range(1,5);
                    if(randomNum > 2)
                    {
                        SwitchToWalking();
                        break;
                    }
                    else
                    {
                        SwitchToStanding();
                        break;
                    }
                }
            break;

            case ShopStates.WALK:
                if(inPlayerRange)
                {
                    //switch to moving to player
                    shopState = ShopStates.MOVETO;
                    moveToTimer = 0;
                    break;
                }

                if(walkingTimer < walkingTimerMax)
                {
                    walkingTimer += Time.deltaTime;
                    thisRB.velocity = moveDir.normalized * moveSpeed;
                }
                else
                {
                    walkingTimer = 0;
                    SwitchToStanding();
                    break;
                }

            break;

            case ShopStates.MOVETO:
                if(!inPlayerRange)
                {
                    //switch to moving to player
                    shopState = ShopStates.STAND;
                    startMoveToPlayerInit = true;
                    moveToTimer = 0;
                    break;
                }

                if(!finishMovedToPlayer)
                {
                    if(moveToTimer < moveToTimerMax)
                    {
                        //Debug.Log("moving...");
                        if(startMoveToPlayerInit)
                        {
                            StopMove();
                            FlipLogic(true);
                            thisRB.isKinematic = true;
                            startMoveToPlayerInit = false;
                            animCont.Play("FrogWalking");
                        }
                        moveToTimer += Time.deltaTime;
                        this.transform.position = Vector3.MoveTowards(this.transform.position, playerPos.transform.position, (moveSpeed * .5f) * Time.deltaTime);
                        //thisRB.velocity = moveDir.normalized * (moveSpeed * .5f);
                    }
                    else
                    {
                        moveToTimer = 0;
                        StopMove();
                        animCont.Play("FromStanding");
                        finishMovedToPlayer = true;
                        startMoveToPlayerInit = true;
                        // open shop after he moves towards player
                        if(Vector3.Distance(gameObject.transform.position, playerPos.transform.position) < 10)
                        {
                            playerPos.GetComponent<PlayerController>().mainController.GetComponent<GameZoneController>().OpenShop();
                        }
                    }
                }
            break;
        }
    }

    public void SwitchToWalking()
    {
        walkingTimer = 0;
        StartMove();
        //decide blink
        int chance = Random.Range(1,10);
        chance += walkBlinkMod;
        if(chance > 10)
        {
            walkBlinkMod = 0;
            animCont.Play("FrogBlinkAnim");
        }
        else
        {
            walkBlinkMod++;
            animCont.Play("FrogWalking");
        }
        walkingTimerMax = Random.Range(2,3);
        shopState = ShopStates.WALK;
    }

    public void SwitchToStanding()
    {
        standingTimer = 0;
        StopMove();
        //decide blink
        int chance = Random.Range(1,5);
        chance += standBlinkMod;
        if(chance > 5)
        {
            standBlinkMod = 0;
            animCont.Play("FrogStandingBlink");
        }
        else
        {
            standBlinkMod++;
            animCont.Play("FromStanding");
        }

        shopState = ShopStates.STAND;
    }

    public void StartMove()
    {
        if(!runAway)
        {
            lastDir = moveDir;
            moveDir = Random.onUnitSphere.normalized;
            //DECIDE FLIP HERE
            
        }
        FlipLogic(false);
        moveDir.y = 0f;
        thisRB.isKinematic = false;
    }

    public void StopMove()
    {
        runAway = false;
        thisRB.isKinematic = true;
    }

    public void RunawayLogic(GameObject runFrom)
    {
        if(shopState != ShopStates.MOVETO)
        {
            if(!runAway)
            {
                walkingTimerMax += .5f;
                runAway = true;
                lastDir = moveDir;
                runAwayDir = runFrom.transform.position - transform.position;
                moveDir = -runAwayDir;
                SwitchToWalking();
            }
        }
    }

    public void RunToLogic(GameObject runTo)
    {
        inPlayerRange = true;
        playerPos = runTo;
    }


    public void FlipLogic(bool bypass)
    {
        if(!bypass)
        {
            //if looking left
            if(thisSR.flipX == false)
            {
                //check direction about to go
                //if movedir.x > 0 == moving right
                if(moveDir.x > 0)
                {
                    animCont.SetTrigger("Flip");
                }
            }
            //if looking right
            else
            {
                //if movedir.x < 0 == moving left
                if(moveDir.x < 0)
                {
                    animCont.SetTrigger("Flip");
                }
            }
        }
        else
        {
            Vector3 heading = playerPos.transform.position - transform.position;
		    //dirNum = AngleDir(transform.forward, heading, transform.up);
            var cross = Vector3.Cross(transform.forward, heading);
            float dir = Vector3.Dot(cross, transform.up);
            //if looking left
            if(thisSR.flipX == false)
            {
                //check direction about to go
                //if movedir.x > 0 == moving right
                if(dir > 0)
                {
                    animCont.SetTrigger("Flip");
                }
            }
            //if looking right
            else
            {
                //if movedir.x < 0 == moving left
                if(dir < 0)
                {
                    animCont.SetTrigger("Flip");
                }
            }
        }
    }
}
