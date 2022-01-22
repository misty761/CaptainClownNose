using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffPart : MonoBehaviour
{
    public float jumpForceH = -10f;
    public float jumpForceV = 200f;

    Rigidbody2D mRigidBody;
    float presentTime;

    // Start is called before the first frame update
    void Start()
    {
        mRigidBody = GetComponent<Rigidbody2D>();

        presentTime = 0f;

        mRigidBody.velocity = Vector2.zero;
        mRigidBody.AddForce(new Vector2(jumpForceH, jumpForceV));
    }

    private void Update()
    {
        presentTime += Time.deltaTime;

        if (presentTime > 1f) Destroy(this.gameObject);
    }


}
