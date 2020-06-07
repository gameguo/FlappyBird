using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 音频管理对象池
/// <summary>
/// 音频管理对象池
/// </summary>
public class ObjectPool
{
    #region 单例
    private static ObjectPool instance;
    private ObjectPool()
    {
        pool = new Dictionary<string, List<AudioSource>>();
    }
    /// <summary>
    /// 获取单例
    /// </summary>
    /// <returns></returns>
    public static ObjectPool GetInstance()
    {
        if (instance == null)
        {
            instance = new ObjectPool();
        }
        return instance;
    }

    #endregion


    /// <summary>
    /// 对象池
    /// </summary>
    private Dictionary<string, List<AudioSource>> pool;

    /// <summary>
    /// 音频游戏对象
    /// </summary>
    private GameObject m_go;
    /// <summary>
    /// 设置游戏对象
    /// </summary>
    public void SetGo(GameObject go)
    {
        m_go = go;
    }

    /// <summary>
    /// 从对象池中获取对象
    /// </summary>
    /// <param name="objName"></param>
    /// <returns></returns>
    public AudioSource GetObj(string objName)
    {
        if (m_go == null)
        {
            return null;
        }
        //结果对象
        AudioSource result = null;
        //判断是否有该名字的对象池
        if (pool.ContainsKey(objName))
        {
            //对象池里有对象
            if (pool[objName].Count > 0)
            {
                //获取结果
                result = pool[objName][0];
                //激活对象
                result.enabled = true;
                //从池中移除该对象
                pool[objName].Remove(result);
                //返回结果
                return result;
            }
        }
        //如果没有该名字的对象池或者该名字对象池没有对象
        //生成
        result = m_go.AddComponent<AudioSource>();
        //返回
        return result;
    }

    /// <summary>
    /// 回收对象到对象池
    /// </summary>
    /// <param name="objName"></param>
    public void RecycleObj(AudioSource obj, string m_name)
    {
        //设置为非激活
        obj.enabled = false;
        //判断是否有该对象的对象池
        if (pool.ContainsKey(m_name))
        {
            //放置到该对象池
            pool[m_name].Add(obj);
        }
        else
        {
            //创建该类型的池子，并将对象放入
            pool.Add(m_name, new List<AudioSource>() { obj });
        }
    }
}

#endregion
public class AudioManager : MonoBehaviour
{

    //所有声音名称枚举
    public enum AudioType
    {
        sfx_die,
        sfx_hit,
        sfx_point,
        sfx_swooshing,
        sfx_wing,
    }

    /// <summary>
    /// 单例
    /// </summary>
    public static AudioManager Instance;

    /// <summary>
    /// 音频
    /// </summary>
    public List<AudioClip> clips;

    private void Awake()
    {
        Instance = this;
        ObjectPool.GetInstance().SetGo(gameObject);
    }


    #region 播放声音 单次
    /// <summary>
    /// 播放声音 单次
    /// </summary>
    /// <param name="audioType">音频风格</param>
    public void OnPlay(AudioType audioType)
    {
        for (int i = 0; i < clips.Count; i++)
        {
            if (audioType.ToString() == clips[i].name)
            {
                StartCoroutine(Play(clips[i], audioType.ToString()));
            }
        }
    }
    /// <summary>
    /// 播放声音
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    private IEnumerator Play(AudioClip clip, string type)
    {
        AudioSource source = ObjectPool.GetInstance().GetObj(type);
        source.playOnAwake = false;
        source.loop = false;
        source.clip = clip;
        source.Play();
        while (true)
        {
            if (!source.isPlaying)
            {
                ObjectPool.GetInstance().RecycleObj(source, type);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion


    #region 播放声音 循环

    //private AudioSource sourceLoop;
    ///// <summary>
    ///// 播放声音 循环
    ///// </summary>
    ///// <param name="audioType">音频风格</param>
    //public void OnPlayLoop(string audioType)
    //{
    //    for (int i = 0; i < clips.Count; i++)
    //    {
    //        if (audioType == clips[i].name)
    //        {
    //            if (sourceLoop == null)
    //            {
    //                sourceLoop = ObjectPool.GetInstance().GetObj(audioType);
    //                sourceLoop.playOnAwake = false;
    //                sourceLoop.loop = true;
    //                sourceLoop.clip = clips[i];
    //                sourceLoop.Play();
    //            }
    //            //StopCoroutine("Stop");
    //            //StartCoroutine("Stop", audioType);
    //        }
    //    }
    //}

    ///// <summary>
    ///// 播放声音
    ///// </summary>
    ///// <param name="clip"></param>
    ///// <returns></returns>
    //private IEnumerator Stop(string type)
    //{
    //    yield return new WaitForSecondsRealtime(0.1f);
    //    sourceLoop.Stop();
    //    ObjectPool.GetInstance().RecycleObj(sourceLoop, type);
    //    sourceLoop = null;
    //}
    #endregion

}
