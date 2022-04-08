using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    public enum EnemyStates
    {
        STAGGER,
        MOVE,
        IDLE,
        ATTACK,
        DEAD
    }

    public EnemyStates enemyState;
    public Animator animC;

    private GameObject target;

    private EnemyController eController;
    private Rigidbody thisRB;
    
    public float distance;
    public float attackRange, attackCooldown, attackCooldownMax;
    private bool inRange = false;

    [Header("Stagger vars")]
    public float knockbackAmount;
    public float stagCD, stagCDMax;
    public bool isStaggered;

    [Header("Death Visuals")]
    public GameObject deathPartSys;
    private Quaternion partSysRotation;


    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController.playerObj.gameObject;
        eController = gameObject.GetComponent<EnemyController>();
        animC.GetComponentInChildren<Animator>();
        enemyState = EnemyStates.MOVE;
        thisRB = GetComponent<Rigidbody>();

        partSysRotation = deathPartSys.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyState != EnemyStates.DEAD)
        {
            distance = Vector3.Distance(target.transform.position, transform.position);
            transform.LookAt(target.transform, Vector3.up);
        }
        
        switch(enemyState)
        {
            case EnemyStates.MOVE:
                inRange = distance > attackRange ? false : true;
                
                //CHASE
                if(!inRange)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, 0, target.transform.position.z), eController._stats["spd"].Value * Time.deltaTime);
                    //mcontroller.Move(Vector3.forward * eController._stats["spd"].Value * Time.deltaTime);
                }
                //IN RANGE
                else
                {
                    animC.SetBool("inRange", true);
                    eController.AttackPlayer(target.GetComponent<PlayerController>());
                    enemyState = EnemyStates.ATTACK;
                }
            
            break;

            case EnemyStates.ATTACK:
                if(attackCooldown < attackCooldownMax)
                {
                    attackCooldown += Time.deltaTime;
                }
                else
                {
                    attackCooldown = 0;
                    animC.SetBool("inRange", false);                    
                    enemyState = EnemyStates.MOVE;
                }
            break;

            
            case EnemyStates.STAGGER:
                if(stagCD < stagCDMax)
                {
                    stagCD += Time.deltaTime;
                }
                else
                {
                    stagCD = 0;
                    thisRB.velocity = Vector3.zero;
                    isStaggered = false;
                    animC.SetBool("isStaggered", isStaggered);
                    if(eController.AmDead())
                    {
                        enemyState = EnemyStates.DEAD;
                        deathPartSys.GetComponentInChildren<ParticleSystem>().Play();
                        break;
                    }
                    enemyState = EnemyStates.MOVE;
                }
            break;

            case EnemyStates.DEAD:
                deathPartSys.transform.rotation = partSysRotation;
                GetComponent<BoxCollider>().enabled = false;
                animC.SetBool("inRange", false); 
                animC.SetBool("isDead", true);
                Destroy(this.gameObject, 2f);
            break;
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Weapon")
        {
            Rigidbody rb = collision.GetComponent<Rigidbody>();
        
            if(rb != null && enemyState != EnemyStates.STAGGER)
            {
                //use stagger anim and stop movement
                isStaggered = true;
                animC.SetBool("inRange", false);
                animC.SetBool("isStaggered", isStaggered);
                enemyState = EnemyStates.STAGGER;

                //calculate knockback
                Vector3 direction = collision.transform.position - transform.position;
                direction.y = 0;
                thisRB.AddForce(-direction.normalized * knockbackAmount, ForceMode.Impulse);
            }
        }
        
    }
}
