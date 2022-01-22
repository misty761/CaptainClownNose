using System.Linq;
using UnityEngine;

// 게임 오브젝트를 계속 왼쪽으로 움직이는 스크립트
public class ScrollingObject : MonoBehaviour 
{
    public float speed = 0.1f;              // 이동 속도
    public float outPositionX = 13.44f;     // 화면 밖으로 판단되는 위치

    private void Update() 
    {
        // 게임오버가 아니라면
        if (!GameManager.instance.isGameover)
        {
            // 초당 speed의 속도로 왼쪽으로 평행이동
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (transform.position.x < -outPositionX)
        {
            transform.Translate(outPositionX * 2, 0, 0);
        }
    }
}