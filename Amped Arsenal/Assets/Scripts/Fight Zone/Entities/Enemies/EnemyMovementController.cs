using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.WSA;

public class EnemyMovementController : MonoBehaviour
{
    public enum EnemyStates
    {
        STAGGER,
        MOVE,
        INIT,
        ATTACK,
        DEAD
    }

    public EnemyStates enemyState;
    private GameObject target;

    private EnemyController eController;
    public Rigidbody thisRB;
    public GameObject visuals;
    public NavMeshAgent nMA;
    //public ParticleSystem dustCloud;
    
    public float distance;
    public float attackRange, attackCooldown, attackCooldownMax;
    private bool inRange = false, isDead;
    public bool facingLeft, facingRight;

    [Header("Stagger vars")]
    public float knockbackAmount;
    public float stagCD, stagCDMax;
    public bool isStaggered, knockbackCalc = true;
    public Vector3 dir;
    public float launchPower, initTimer = 0, initTimerMax = .5f;

    [Space(10)]
    [Header("Ground Detection")]
    public LayerMask layers, avoidWeapons;
    public float rayDistance, fallSpeed;
    public bool grounded;


    // Start is called before the first frame update
    void Start()
    {
        knockbackCalc = true;
        target = PlayerController.playerObj.gameObject;
        eController = gameObject.GetComponent<EnemyController>();
        nMA = GetComponent<NavMeshAgent>();
        //animC.GetComponentInChildren<Animator>();
        enemyState = EnemyStates.INIT;
        //thisRB = GetComponent<Rigidbody>();
        //thisRB.constraints = RigidbodyConstraints.FreezeRotation;
        stagCD = stagCDMax;
        GetComponent<BoxCollider>().isTrigger = true;

        // launch up
        //thisRB?.AddForce(Vector3.up * launchPower, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //nMA.destination = target.transform.position;// - transform.position;
        dir = target.transform.position - transform.position;
        // calculate velocity limited to the desired speed:
        var velocity = Vector3.ClampMagnitude(dir * eController._stats["spd"].Value, eController._stats["spd"].Value);
        //if user has fear effect then inverse the movement

        if (eController.HasEffect("Fear"))
        {
            dir *= -1;
            velocity *= -1;
        }

        //turn off colosion with weapons after getting hit. once stagger ends turn collisions back on?
        
        
        dir.y = 0;
        //velocity.y = 0;

        Ray ray = new(transform.position, Vector3.down);

        grounded = Physics.Raycast(ray, out RaycastHit hit, rayDistance, layers);

        /*if(enemyState != EnemyStates.INIT)
        {
            if(!grounded)
            {
                thisRB.useGravity = true;
                thisRB.constraints = RigidbodyConstraints.None;
                thisRB.constraints = RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                velocity.y = 0;
                thisRB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
                thisRB.useGravity = false;
            }
        }*/
        
        //transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        //flip sprite
        if(!isDead)
        {
            if(dir.x > 0 && facingRight != true)
            {
                //theSR.flipX = true;
                facingRight = true;
                facingLeft = false;
                eController.spriteObj.GetComponent<Animator>().SetBool("Flip", true);
                //dustCloud.velocityOverLifetime.x = 0 - dustCloud.velocityOverLifetime.x;
            }
            else if(dir.x < 0 && facingLeft != true)
            {
                //theSR.flipX = false;
                facingRight = false;
                facingLeft = true;
                eController.spriteObj.GetComponent<Animator>().SetBool("Flip", false);
            }
        }
        

        if(enemyState != EnemyStates.DEAD)
        {
            distance = Vector3.Distance(target.transform.position, transform.position);
        }
        
        switch(enemyState)
        {
            case EnemyStates.INIT:
                if(initTimer < initTimerMax)
                {
                    initTimer += Time.deltaTime;
                }
                else
                {
                    //thisRB.constraints = RigidbodyConstraints.FreezeRotation;// | RigidbodyConstraints.FreezePositionY;
                    GetComponent<BoxCollider>().isTrigger = false;
                    enemyState = EnemyStates.MOVE;
                }
            break;

            case EnemyStates.MOVE:
                inRange = distance <= attackRange;
                if(isStaggered)
                {
                    enemyState = EnemyStates.STAGGER;
                    break;
                }
                if(isDead)
                {
                    enemyState = EnemyStates.DEAD;
                }
                //CHASE
                if(!inRange)
                {
                    dir = target.transform.position - transform.position;
                    velocity = Vector3.ClampMagnitude(dir * eController._stats["spd"].Value, eController._stats["spd"].Value);
                    if (eController.HasEffect("Fear"))
                    {
                        //dir *= -1;
                        //velocity *= -1;
                        nMA.destination = -target.transform.position;
                    }
                    else
                    {
                        nMA.destination = target.transform.position;
                    }

                    dir.y = 0;
                    //velocity.y = 0;
                    if(grounded)
                    {
                        //thisRB.velocity = velocity;
                    }
                }
                //IN RANGE
                else
                {
                    if(isStaggered)
                    {
                        enemyState = EnemyStates.STAGGER;
                        break;
                    }
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
                if(isStaggered)
                {
                    enemyState = EnemyStates.STAGGER;
                    break;
                }
            break;

            
            case EnemyStates.STAGGER:
                if(stagCD > 0)
                {
                    stagCD -= Time.deltaTime;
                }
                else
                {
                    stagCD = stagCDMax;
                    //thisRB.velocity = Vector3.zero;
                    isStaggered = false;
                    enemyState = EnemyStates.MOVE;
                    knockbackCalc = true;
                }
            break;

            case EnemyStates.DEAD:
                if(!isDead)
                {
                    isDead = true;
                    visuals.GetComponent<VisualEffects>().Died();
                    //animC.SetBool("inRange", false); 
                    //animC.SetBool("isDead", true);
                    Destroy(this.gameObject);
                }
                
            break;
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Weapon")
        {
            if(isStaggered)
            {   
                Physics.IgnoreCollision(collision.GetComponent<Collider>(), GetComponent<Collider>(), true);
            }
            else
            {
                Physics.IgnoreCollision(collision.GetComponent<Collider>(), GetComponent<Collider>(), false);
            }
            if(eController.AmDead())
            {
                enemyState = EnemyStates.DEAD;
                return;
            }
            if(collision.gameObject.GetComponent<WeaponMods>().giveKnockback)
            {
                Rigidbody rb = collision.GetComponent<Rigidbody>();
                if(rb != null && enemyState != EnemyStates.STAGGER)
                {
                    isStaggered = true;
                    enemyState = EnemyStates.STAGGER;

                    //calculate knockback
                    // if(knockbackCalc == true)
                    // {
                    //     Vector3 direction = collision.transform.position - transform.position;
                    //     direction.y = 0;
                    //     if(collision.gameObject.GetComponent<WeaponMods>().knockbackModAmount > 0)
                    //     {
                    //         //Debug.Log("adding extra knockback");
                    //         thisRB.AddForce(-direction.normalized * (knockbackAmount + collision.gameObject.GetComponent<WeaponMods>().knockbackModAmount), ForceMode.Impulse);
                    //     }
                    //     else
                    //     {
                    //         //Debug.Log("applying normal knockback");
                    //         thisRB.AddForce(-direction.normalized * knockbackAmount, ForceMode.Impulse);
                    //     }
                        
                    //}
                }
            }
            visuals.GetComponentInChildren<VisualEffects>().damaged = true; 
        }
    }
}
