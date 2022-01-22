using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
	float moveSpeed;
	float destroyTime;

	Vector3 vector;
	private void Start()
	{

		moveSpeed = 1f; //위로 움직이는 속도값

		destroyTime = 1f; //몇초 후 삭제 될건지

	}

	void Update()
	{

		vector.Set(this.transform.position.x, this.transform.position.y
			+ (moveSpeed + Time.deltaTime), this.transform.position.z);

		this.transform.position = vector;

		destroyTime -= Time.deltaTime;

		if (destroyTime <= 0)
		{
			Destroy(this.gameObject);
		}

	}

}
