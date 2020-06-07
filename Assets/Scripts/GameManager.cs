using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 单例与初始化
    /// <summary>
    /// 游戏管理静态实例对象
    /// </summary>
    public static GameManager Instance;
    //初始化
    private void Awake()
    {
        //初始化静态实例为自身
        Instance = this;
        //初始化默认游戏为停止状态
        IsPlay = false;
        //获取或设置最高分数
        GetRestScore();
    }
    #endregion


    public UnityEngine.UI.Text scoreText;

    /// <summary>
    /// 分数
    /// </summary>
    private static int score = 0;
    public static int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            Instance.scoreText.text = score.ToString();
            if (score >= scoreRest)
            {
                Instance.isJinPai = true;
                scoreRest = score;
                SetRestScore();
            }
        }
    }

    /// <summary>
    /// 最高分数
    /// </summary>
    public static int scoreRest;

    //获取设置最高分数
    public static void SetRestScore()
    {
        PlayerPrefs.SetInt("scoreRest", scoreRest);
    }

    //获取记录分数
    public static void GetRestScore()
    {
        scoreRest = PlayerPrefs.GetInt("scoreRest", 0);
    }

    //地面
    public Transform ground;

    //角色
    public Player player;

    //UI
    public UIRoot uiRoot;

    /// <summary>
    /// 设置角色状态
    /// </summary>
    /// <param name="isDie">是否死亡</param>
    public void SetPlayerState(bool isDie)
    {
        if (Instance.ground != null)
        {
            //遍历停止
            for (int i = 0; i < Instance.ground.childCount; i++)
            {
                Ground g = Instance.ground.GetChild(i).GetComponent<Ground>();
                if (g != null)
                {
                    g.SetMoveState(!isDie);
                }
            }
        }
    }


    private bool isJinPai = false;
    /// <summary>
    /// 准备方法
    /// </summary>
    public void Ready()
    {
        isJinPai = false;
        Score = 0;
        scoreText.gameObject.SetActive(true);
        player.gameObject.SetActive(false);
        player.gameObject.SetActive(true);
    }

    /// <summary>
    /// 结束方法
    /// </summary>
    public void Over()
    {
        scoreText.gameObject.SetActive(false);
        uiRoot.Over(isJinPai);
    }


    //封装游戏开始
    private static bool isPlay;

    /// <summary>
    /// 获取或者设置游戏是否开始
    /// </summary>
    public static bool IsPlay
    {
        get
        {
            return isPlay;
        }
        set
        {
            isPlay = value;
            //如果单例对象为null
            if (Instance == null)
            {
                return;
            }
            //初始化Player状态
            Instance.player.InitState(isPlay);
            if (!isPlay)
            {
                if (Instance.ground != null)
                {
                    //则隐藏所有钢管
                    for (int i = 0; i < Instance.ground.childCount; i++)
                    {
                        Ground g = Instance.ground.GetChild(i).GetComponent<Ground>();
                        if (g != null)
                        {
                            g.OnClosePipe();
                        }
                    }
                }
            }
        }
    }


}
