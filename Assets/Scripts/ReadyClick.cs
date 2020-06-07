using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReadyClick : MonoBehaviour
{
    //点击事件
    public UnityEvent onClick;



    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN  //如果是Windows编辑器或者Windows平台
        //如果按下空格键 或者按下鼠标左键 
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            //执行点击事件
            onClick.Invoke();
        }
#elif UNITY_ANDROID                      //如果是安卓
        //如果触摸屏幕按下
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //执行点击事件
            onClick.Invoke();
        }
#endif
    }

}
