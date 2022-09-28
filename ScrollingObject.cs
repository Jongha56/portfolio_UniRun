using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 오브젝트를 계속 왼쪽으로 움직이는 스크립트
public class ScrollingObject : MonoBehaviour
{
    public float speed = 10f; //이동 속도


    // Update is called once per frame
    void Update()
    {
        //게임오버가 아니라면
        if (!GameManager.instance.isGameover)
        {
            //초다 speed의 속도로 왼쪽으로 평행이동
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }
}
