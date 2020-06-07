using UnityEngine;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour
{
    //主界面
    public GameObject main;

    //准备界面
    public GameObject ready;

    //结束界面
    public GameObject over;

    //金牌
    public GameObject jinpai;


    //初始化
    private void Start()
    {
        main.SetActive(true);
        ready.SetActive(false);
        over.SetActive(false);
    }

    //退出游戏
    public void OnExit()
    {
        Application.Quit();
    }


    //开始按钮
    public void OnPlay()
    {
        //关闭主界面
        main.SetActive(false);
        //打开准备界面
        ready.SetActive(true);
        //游戏准备
        GameManager.Instance.Ready();

        //播放声音
        AudioManager.Instance.OnPlay(AudioManager.AudioType.sfx_swooshing);
    }

    //游戏开始按钮
    public void OnGamePlay()
    {
        main.SetActive(false);
        ready.SetActive(false);
        over.SetActive(false);
        //游戏开始
        GameManager.IsPlay = true;
    }

    //游戏重新开始按钮
    public void OnGameResetPlay()
    {
        main.SetActive(false);
        ready.SetActive(true);
        over.SetActive(false);
        //游戏设置为未开始
        GameManager.IsPlay = false;
        //游戏准备
        GameManager.Instance.Ready();

        //播放声音
        AudioManager.Instance.OnPlay(AudioManager.AudioType.sfx_swooshing);
    }

    public Text overText, restText;
    public void Over(bool jin)
    {
        over.SetActive(true);
        jinpai.SetActive(jin);
        overText.text = GameManager.Score.ToString();
        restText.text = GameManager.scoreRest.ToString();


        //开启定时器 0.5秒后开始执行
        Invoke("OnOverAudio", 0.3f);
    }


    private void OnOverAudio()
    {
        //播放声音
        AudioManager.Instance.OnPlay(AudioManager.AudioType.sfx_swooshing);
    }
    

}
