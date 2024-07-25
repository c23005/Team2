using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private float moveS;
    public float turnS;
    private Rigidbody rb;
    private float movementInputValue;
    private float turnInputValue;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        TnakMove();
        TankTurn();
    }

    void TnakMove()
    {
        movementInputValue = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * movementInputValue * moveS * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        // ★加速度の設定
        // 入力値の二乗の取得
        var inputValue = movementInputValue * movementInputValue;

        // 入力値の二乗が最大値（１）に達したら速度を徐々に増加させる。
        if (inputValue == 1)
        {
            moveS += Time.deltaTime;
            print(moveS);

            // 最大速度の設定
            if (moveS > 7)
            {
                moveS = 7;
            }
        }

        // 入力値が０になったら速度を０に戻す。
        if (inputValue == 0)
        {
            moveS = 0;
        }
    }

    void TankTurn()
    {
        turnInputValue = Input.GetAxis("Horizontal");
        float turn = turnInputValue * turnS * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turn, 0);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}
