using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//地面类  控制地面循环移动 与 生成钢管
public class Ground : MonoBehaviour
{

    //单个地面宽度
    public float width;

    //背景循环个数
    public int groundNumber;

    //地面移动速度
    public float moveSpeed;

    //x临界值
    public float cutOffX;

    //钢管预制体
    public GameObject pipe;

    //钢管实例
    private GameObject pipeInstan;

    //初始化
    private void Awake ()
    {
        SetMoveState(true);
        //GameManager.IsPlay = true;
    }
	
	//每帧执行
	private void Update ()
    {
        //判断X坐标是否小于左临界值
		if(transform.localPosition.x < cutOffX)
        {
            //改变x坐标 移动到最右边
            Vector2 v2 = transform.localPosition;
            v2.x += width * groundNumber;

            //重新赋值
            transform.localPosition = v2;

            //游戏如果已经开始
            if (GameManager.IsPlay)
            {
                //生成钢管
                InstancePipe();
            }
        }
	}
    
    /// <summary>
    /// 生成钢管
    /// </summary>
    private void InstancePipe()
    {
        //如果钢管不存在 则实例化
        if(pipeInstan == null)
        {
            pipeInstan = Instantiate(pipe, transform);
            pipeInstan.transform.localPosition = Vector3.zero;
            return;
        }

        //否则关闭钢管并重新打开
        pipeInstan.SetActive(false);
        pipeInstan.SetActive(true);
    }

    /// <summary>
    /// 关闭钢管
    /// </summary>
    public void OnClosePipe()
    {
        //关闭钢管
        if(pipeInstan != null)
        {
            pipeInstan.SetActive(false);
        }
    }


    /// <summary>
    /// 设置钢管移动状态
    /// </summary>
    /// <param name="isPlay">是否移动</param>
    public void SetMoveState(bool isMove)
    {
        //获取自身刚体组件
        Rigidbody2D rig = this.GetComponent<Rigidbody2D>();
        //关闭钢管
        if (rig != null)
        {
            if (isMove)
            {
                //设置速度 向左
                rig.velocity = Vector2.left * moveSpeed;
            }
            else
            {
                //设置速度 为0
                rig.velocity = Vector2.zero;
            }
        }
    }

    //触发器
    private void OnCollisionEnter2D(Collision2D collision)
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

                    player.IsDie = true;
                }
            }
        }
    }

}
