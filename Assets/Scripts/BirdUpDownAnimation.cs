using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//鸟 上下动画
public class BirdUpDownAnimation : MonoBehaviour
{

    private bool isUp;
    private void OnEnable()
    {
        isUp = true;
    }
    public float speed;

    public float minY, maxY;

    private Vector2 temp;
    private void Update()
    {
        temp = transform.localPosition;
        if (isUp)
        {
            temp.y += Time.deltaTime * speed;
            if(temp.y >= maxY)
            {
                isUp = false;
            }
        }
        else
        {
            temp.y -= Time.deltaTime * speed;
            if (temp.y <= minY)
            {
                isUp = true;
            }
        }
        transform.localPosition = temp;
    }
}
