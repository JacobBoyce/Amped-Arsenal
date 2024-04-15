using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPoofLogic : MonoBehaviour
{
    public float timer, timerMax;
    //public float vertOffset;
    // Update is called once per frame
    public void OneEnabled()
    {
        GetComponentInChildren<Animator>().Play("DPoofy");
    }
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z), 5f * Time.deltaTime);
        if(timer < timerMax)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ObjectPoolManager.ReturnObjectToPool(this.gameObject);
            timer = 0;
            //Destroy(this.gameObject);
        }
    }
}
