using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public GameObject sPoint;

    public Color GizmosColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);

    void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
