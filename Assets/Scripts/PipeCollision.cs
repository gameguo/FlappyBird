using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCollision : MonoBehaviour
{
    //触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                if (!player.IsDie)
                {
                    //播放声音
                    AudioManager.Instance.OnPlay(AudioManager.AudioType.sfx_hit);

                    Invoke("PlayDieAudio", 0.2f);
                    player.IsDie = true;
                }
            }
        }
    }

    private void PlayDieAudio()
    {
        //播放声音
        AudioManager.Instance.OnPlay(AudioManager.AudioType.sfx_die);
    }

}
