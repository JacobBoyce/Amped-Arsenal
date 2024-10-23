using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopMovement : MonoBehaviour
{
    public GameObject startpos, lobbyPos;
    public ShopkeepBarrierLogic barrierLogic;
    public SphereCollider col1,col2;
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
    public bool walking;
    public float moveSpeed;
    public float walkTimer, walkTimerMin, walkTimerMax;
    public Vector3 moveDir;
    public GameObject runFromObject = null;

    [Header("Despawning Vars")]
    public GameObject deathPoof;
    public GameObject visuals;
    public bool escapedPhase = false;
    private bool flag;
    public float dpoofYOffset;

    public void Start()
    {
        //StartCoroutine(WaitLogic());
        walking = false;
        col1.enabled = false;
        col2.enabled = false;
        StartCoroutine(StandLobbyLogic());
    }

    public void OnEnable()
    {
        //transform.position = startpos.transform.position;
        //flag = false;
        //StartCoroutine(WaitLogic());
    }

    public void GoToLevel()
    {
        transform.position = startpos.transform.position;
        walking = true;
        escapedPhase = false;
        flag = false;
        col1.enabled = true;
        col2.enabled = true;
        StartCoroutine(WaitLogic());
    }

    public void GoToLobby()
    {
        transform.position = lobbyPos.transform.position;
        walking = false;
        col1.enabled = false;
        col2.enabled = false;
        StopAllCoroutines();
        StartCoroutine(StandLobbyLogic());
    }

    public void EscapeBloodMoon()
    {
        if(escapedPhase && flag == true)
        {
            GameZoneController.Instance.p1.CleanInteractableObj();
            escapedPhase = false;
            GameObject largeDPoof = ObjectPoolManager.SpawnObject(deathPoof, new Vector3(transform.position.x , transform.position.y + dpoofYOffset, transform.position.z), transform.rotation, ObjectPoolManager.PoolType.DPoof);
            largeDPoof.transform.localScale += new Vector3(largeDPoof.transform.localScale.x *1.5f, largeDPoof.transform.localScale.y *1.5f, largeDPoof.transform.localScale.z *1.5f);
            largeDPoof.GetComponent<DeathPoofLogic>().isLarge = true;

            GoToLobby();
        }
        //barrierLogic.TriggerEscapeIfWasNearby(GameZoneController.Instance.p1);
        
    }
    public void UndoEscape()
    {
        escapedPhase = false;
        visuals.SetActive(true);
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
            StartCoroutine(WaitLogic());
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

    IEnumerator StandLobbyLogic()
    {
        //Enter standing state
        thisRB.isKinematic = true;
        animCont.SetBool("Stand", true);
        meshRend.material = standMat;

        //decide wait time
        waitTime = Random.Range(waitMin,waitMax);
        //wait
        yield return new WaitForSeconds(waitTime);

        tempFlipChance = Random.Range(0,101);
        if(tempFlipChance < waitFlipChance)
        {
            //flip
            flipFlag = !flipFlag;
            animCont.SetBool("Flip", flipFlag);
            yield return new WaitForSeconds(1.2f);
        }
        StartCoroutine(StandLobbyLogic());
    }

    public void Update()
    {
        
        if(walking)
        {
            walkTimer -= Time.deltaTime;
            //walk logic
            if(!thisRB.isKinematic)
            {
                thisRB.linearVelocity = moveDir.normalized * moveSpeed;
            }
            

            if(walkTimer <= 0)
            {
                walking = false;
                StartCoroutine(WaitLogic());
            }
        }

        if(flag == false)
        {
            if(GameZoneController.Instance.wvController.startExfilDelay)
            {
                if(escapedPhase == false)
                {
                    flag = true;
                    escapedPhase = true;
                    EscapeBloodMoon();
                }
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
