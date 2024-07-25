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

        // �������x�̐ݒ�
        // ���͒l�̓��̎擾
        var inputValue = movementInputValue * movementInputValue;

        // ���͒l�̓�悪�ő�l�i�P�j�ɒB�����瑬�x�����X�ɑ���������B
        if (inputValue == 1)
        {
            moveS += Time.deltaTime;
            print(moveS);

            // �ő呬�x�̐ݒ�
            if (moveS > 7)
            {
                moveS = 7;
            }
        }

        // ���͒l���O�ɂȂ����瑬�x���O�ɖ߂��B
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
