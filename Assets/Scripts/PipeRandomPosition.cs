using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//钢管随机类   随机钢管位置
public class PipeRandomPosition : MonoBehaviour
{
    //脚本开启时
    private void OnEnable()
    {
        Vector3 v3 = transform.localPosition;
        v3.y = Random.Range(0.9f, 2.7f);
        transform.localPosition = v3;
    }


    //当物体离开触发器时
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null && !player.IsDie)
            {
                GameManager.Score++;

                AudioManager.Instance.OnPlay(AudioManager.AudioType.sfx_point);
            }
        }
    }

}