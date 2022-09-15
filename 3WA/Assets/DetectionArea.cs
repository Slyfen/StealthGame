using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{

    [SerializeField] EnemyAI enemy;
    [SerializeField] LayerMask areaLayer;

    // CHECK TOUTES LES FRAMES SI LE PLAYER EST DANS LA ZONE DE DETECTION
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (IsVisible(other.transform))
            {
                Debug.Log("PLAYER VISIBLE");
            }
        }
    }

    bool IsVisible(Transform player)
    {
        RaycastHit hitInfo;
        bool hitPlayer = false;


        Vector3 dirToPlayer = player.position + new Vector3(0, 1, 0) - enemy.transform.position;

        if (Physics.Raycast(enemy.transform.position, dirToPlayer, out hitInfo, Mathf.Infinity, areaLayer))
        {

            hitPlayer = hitInfo.collider.tag == "Player";
            Debug.DrawRay(enemy.transform.position, dirToPlayer.normalized * hitInfo.distance, hitPlayer ? Color.green : Color.red);
        }



        return hitPlayer;
    }


}
