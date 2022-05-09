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
    public SpriteRenderer theSR;

    private GameObject target;

    private EnemyController eController;
    private Rigidbody thisRB;
    public GameObject visuals;
    
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

    public Vector3 dir;


    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController.playerObj.gameObject;
        eController = gameObject.GetComponent<EnemyController>();
        //animC.GetComponentInChildren<Animator>();
        enemyState = EnemyStates.MOVE;
        thisRB = GetComponent<Rigidbody>();

        partSysRotation = deathPartSys.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        dir = target.transform.position - transform.position;
        //Debug.Log(dir);
        // ignore any height difference:
        
        // calculate velocity limited to the desired speed:
        var velocity = Vector3.ClampMagnitude(dir * eController._stats["spd"].Value, eController._stats["spd"].Value);
        //Debug.Log(velocity);
        dir.y = 0;
        velocity.y = 0;
        Debug.Log(velocity);
        //transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        //flip sprite
        if(dir.x > 0)
        {
            theSR.flipX = true;
        }
        else if(dir.x < 0)
        {
            theSR.flipX = false;
        }

        if(enemyState != EnemyStates.DEAD)
        {
            distance = Vector3.Distance(target.transform.position, transform.position);
        }
        
        switch(enemyState)
        {
            case EnemyStates.MOVE:
                inRange = distance > attackRange ? false : true;
                
                //CHASE
                if(!inRange)
                {
                    thisRB.velocity = velocity;
                }
                //IN RANGE
                else
                {
                    //animC.SetBool("inRange", true);
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
                    //animC.SetBool("inRange", false);                    
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
                    enemyState = EnemyStates.MOVE;
                }
            break;

            case EnemyStates.DEAD:
                deathPartSys.transform.rotation = partSysRotation;
                GetComponent<CapsuleCollider>().enabled = false;
                thisRB.velocity = Vector3.down *5;
                visuals.GetComponent<VisualEffects>().Died();
                //animC.SetBool("inRange", false); 
                //animC.SetBool("isDead", true);
                Destroy(this.gameObject, 2f);
            break;
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Weapon")
        {
            Rigidbody rb = collision.GetComponent<Rigidbody>();

            if(eController.AmDead())
            {
                enemyState = EnemyStates.DEAD;
                deathPartSys.GetComponentInChildren<ParticleSystem>().Play();
            }
            else if(rb != null && enemyState != EnemyStates.STAGGER)
            {
                //use stagger anim and stop movement
                isStaggered = true;
                //animC.SetBool("inRange", false);
                //animC.SetBool("isStaggered", isStaggered);
                enemyState = EnemyStates.STAGGER;

                //calculate knockback
                Vector3 direction = collision.transform.position - transform.position;
                direction.y = 0;
                thisRB.AddForce(-direction.normalized * knockbackAmount, ForceMode.Impulse);
            }
        }
        
    }
}
