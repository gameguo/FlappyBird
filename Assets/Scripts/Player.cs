using UnityEngine;

//角色类  角色控制类
public class Player : MonoBehaviour
{
    //刚体2D
    public Rigidbody2D rig;

    //动画组件
    public Animator animator;

    //上下动画组件
    public BirdUpDownAnimation birdUpDownAnimation;

    //力的强度
    public float forceIntension;

    //Z轴最大旋转角度和最小旋转角度
    public float zMaxAngle, zMinAngle;

    //默认坐标
    public Vector2 normalPosition;

    private float maxY;

    private bool isDie;
    public bool IsDie
    {
        get
        {
            return isDie;
        }
        set
        {
            isDie = value;
            if (isDie)
            {
                //游戏结束
                GameManager.Instance.Over();
                if (animator != null)
                {
                    animator.enabled = false;
                }
                //停止所有定时器
                CancelInvoke();
                //开启定时器 0秒后开始执行 每0.02f秒执行一次
                InvokeRepeating("OnZLerpMax", 0, 0.02f);
            }
            else
            {
                if (animator != null)
                {
                    animator.enabled = true;
                }

            }
            GameManager.Instance.SetPlayerState(isDie);
        }
    }

    //初始化状态
    public void InitState(bool isPlay)
    {
        //关闭开启受力
        if (rig != null)
        {
            rig.isKinematic = !isPlay;
        }
        if (isPlay)
        {
            //FlyOne();
            //关闭开启上下动画组件
            if (birdUpDownAnimation != null)
            {
                birdUpDownAnimation.enabled = false;
            }
        }
    }
    
    private void OnEnable()
    {
        transform.position = normalPosition;
        transform.rotation = Quaternion.identity;
        IsDie = false;

        //关闭开启上下动画组件
        if (birdUpDownAnimation != null)
        {
            birdUpDownAnimation.enabled = true;
        }

        maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y;
    }


    #region 飞和Z轴旋转
    //飞一下
    private void FlyOne()
    {
        if (rig == null)
        {
            return;
        }
        //施加一个向上的力
        rig.velocity = Vector2.up * forceIntension;

        //停止所有定时器
        CancelInvoke();
        //开启定时器 0秒后开始执行 每0.02f秒执行一次
        InvokeRepeating("OnZLerpMax", 0, 0.02f);


        //播放声音
        AudioManager.Instance.OnPlay(AudioManager.AudioType.sfx_wing);

        //print(AudioManager.AudioType.sfx_wing);
    }

    //Z轴缓动到最大值或者最小值
    private void OnZLerpMax()
    {
        Quaternion target;
        //目标角度
        if (IsDie)
        {
            target = Quaternion.Euler(0, 0, zMinAngle);
        }
        else
        {
            target = Quaternion.Euler(0, 0, zMaxAngle);
        }
        //target = Quaternion.Euler(0, 0, zMinAngle);
        if (Quaternion.Angle(transform.rotation, target) <= 0.02f)
        {
            //设置为最大旋转
            transform.rotation = target;
            //停止定时器
            CancelInvoke();
            //重置下落Z轴时间
            zTime = 0.2f;
        }
        else
        {
            //重置下落Z轴时间
            zTime = 0.2f;
            //缓动向最大旋转
            transform.rotation = Quaternion.Lerp(transform.rotation, target, 0.5f);
        }
    }

    //z轴旋转倒计时
    private float zTime = 0.2f;
    //控制Z轴旋转
    private void CtrlZRotation()
    {
        if (rig == null)
        {
            return;
        }
        zTime -= Time.deltaTime;
        if (zTime <= 0 && rig.velocity.y < 0)
        {
            if (Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, zMinAngle)) < 10f)
            {
                //最小旋转
                transform.rotation = Quaternion.Euler(0, 0, zMinAngle);
            }
            else
            {
                float s = 0.2f;
                if (rig.velocity.y <= -5)
                {
                    s = 0.8f;
                }
                //缓动向最小旋转
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, zMinAngle), Time.deltaTime * s * -rig.velocity.y);
            }
        }
        else if (rig.velocity.y == 0)
        {
            //最小旋转
            transform.rotation = Quaternion.Euler(0, 0, zMinAngle);
        }
    }

    #endregion

    #region Update
    private void Update()
    {
        //如果游戏结束 或者已经死亡 则跳出Update
        if (!GameManager.IsPlay || IsDie)
        {
            return;
        }

        //控制最大高度
        if (transform.position.y > maxY)
        {
            transform.position = new Vector2(transform.position.x, maxY);
        }

        //控制小鸟旋转
        CtrlZRotation();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN  //如果是Windows编辑器或者Windows平台

        //如果按下空格键 或者按下鼠标左键 
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            //飞一下
            FlyOne();
        }

#elif UNITY_ANDROID                      //如果是安卓

        CtrlZRotation();
        //如果触摸屏幕按下
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //飞一下
            FlyOne();
        }

#endif
    } 
    #endregion
}
