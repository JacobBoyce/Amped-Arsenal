using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    public float speed, distance;
    public int xpAmount;
    public bool inRange = false;
    private PlayerController p1;
    public GameObject partSys, visuals;

    public void Start()
    {
        p1 = PlayerController.playerObj;
    }

    public void Update()
    {
        distance = Vector3.Distance(p1.transform.position, transform.position);

        if(distance < p1._stats["pull"].Value)
        {
            transform.position = Vector3.MoveTowards(transform.position, p1.transform.position, speed * Time.deltaTime);
            speed += .1f;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            partSys.SetActive(true);
            for(int i = 0; i < partSys.GetComponentsInChildren<ParticleSystem>().Length; i++)
            {
                partSys.GetComponentsInChildren<ParticleSystem>()[i].Play();
            }
            visuals.SetActive(false);
            p1.AddXP(xpAmount);
            Destroy(this.gameObject,1f);
        }
    }
}
