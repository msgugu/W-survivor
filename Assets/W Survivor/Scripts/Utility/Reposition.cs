using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    private Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
        {
            return;
        }
        else
        {
            Vector3 playerPos = InGameManager.Instance.player.transform.position;
            Vector3 myPos = transform.position;
            float diffX = Math.Abs(playerPos.x - myPos.x);
            float diffY = Math.Abs(playerPos.y - myPos.y);

            Vector3 playerDir = InGameManager.Instance.player.playerInputVec;
            float dirX = playerDir.x < 0 ? -1 : 1;
            float dirY = playerDir.y < 0 ? -1 : 1;

            switch (transform.tag)
            {
                case "Ground":
                    // if (diffX > diffY)
                    // {
                    //     transform.Translate(Vector3.right * dirX * 40); // tile의 길이 * 2 = 40
                    // }
                    // else if (diffX < diffY)
                    // {
                    //     transform.Translate(Vector3.up * dirY * 40); // tile의 길이 * 2 = 40
                    // }
                    break;
                case "Enemy":
                    if (coll.enabled)
                    {
                        // transform.Translate(playerDir * 20 + new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f), 0f));
                    }
                    break;
            }
        }
    }
    
}
