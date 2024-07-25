using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public GameObject Ito;
    [Header("プレイヤーのスピード")]public float speed;
    ItoScript ito;
    Rigidbody rb;
    [HideInInspector]public float x;
    [HideInInspector]public float y;
    [HideInInspector]public float z;
    int cheakint = 0;
    bool oncol;
    public List<GameObject> catlist = new List<GameObject>();
    public GameObject PlayerRotePos;
    Vector3 playeroldPos;
    CapsuleCollider capsuleCollider;
    [Header("捕まえられる距離を入れる")]public float DisPos;
    public GameObject catchIMG;
    float length;
    public Vector3 movedir;
    float CatDistance;
    int groundDirection = 0;
    [Range(0f, 1f)]public float Ypos;
    public GameObject catchOBJ;
    CatchColliderScript catchCollider;
    [Min(0), Header("接地判定用の中心点のオフセット")]
    public float groundCheckOffsetY = 0.3f;
    [Min(0), Header("接地判定の距離")]
    public float groundCheckDistance = 0;
    [Min(0), Header("接地判定の半径")]
    public float groundCheckRadius = 0.5f;
    [Header("このレイヤーのオブジェクトにレイが当たった時に接地したと判定する")]
    public LayerMask groundLayers = 0;

    Vector3 groundNormal;
    RaycastHit hit;

    [Range(0f, 1f)]public float offSetY;
    [Range(0f, 1f)]public float Distance; 
    [Header("重力(見るようだから触らない)")]public Vector3 direction;

    [Header("Wallのレイヤーを選択する")] public LayerMask WallLayer;

    Vector3 wallNormal;
    bool onWool;

    bool CheakGround()
    {    //Physics.SphereCast(スタート地点,半径,球の通過する方向,球がヒットした場所に関する情報,Castの最大の大きさ,レイヤーマスク(レイキャストするときに選択的に衝突を無視する,トリガーに設定されているものも対象かどうか))
         //※SphereCastは外側しか判定がない(中のほうには当たり判定がない)からYposで位置を調整する
        if (Physics.SphereCast((transform.position + new Vector3(0, -Ypos, 0)) + Vector3.down * groundCheckOffsetY, 0.5f, -transform.up, out hit, groundCheckOffsetY + groundCheckDistance, groundLayers, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log("着地");
            groundNormal = hit.normal;
            return true;
        }
        else
        {
            groundNormal = Vector3.zero;
            return false;
        }
    }

    bool CheakWall()
    {
        //壁があるかを調べる
        if(Physics.Raycast(transform.position + Vector3.up * offSetY,transform.forward,out hit,Distance + capsuleCollider.radius, WallLayer, QueryTriggerInteraction.Ignore))
        {
            wallNormal = hit.normal;

            return true;
        }
        else
        {
            if (Physics.SphereCast((transform.position + new Vector3(0, -Ypos, 0)) + Vector3.down * groundCheckOffsetY, 0.5f, Vector3.down, out hit, groundCheckOffsetY + groundCheckDistance, WallLayer, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("着地");
                groundNormal = hit.normal;
                return true;
            }
            else
            {
                groundNormal = Vector3.zero;
                return false;
            }
        }
    }

    void Start()
    {
        Ito.SetActive(false);
        ito = Ito.GetComponent<ItoScript>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        //catchOBJ.SetActive(false);
        catchCollider = catchOBJ.GetComponent<CatchColliderScript>();
        GameObject[] cats = GameObject.FindGameObjectsWithTag("cat");
        catlist.AddRange(cats);
    }


    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        float Mousex = Input.GetAxis("Mouse X");
        float Mousey = Input.GetAxis("Mouse Y");
        CheakGround();
        if (CheakGround())
        {
            //groundDirection = 0;
        }
        //CheakWall();
        OnMove();
        CatCatch();
        ItoEve();
        Gravity();
        //Inspectorで重力の向きが見れるようにしている
        direction = Physics.gravity;
        catlist.RemoveAll(item => item == null);
        //重力をかけている
        rb.AddForce(rb.mass * Physics.gravity, ForceMode.Force);
        if(catlist.Count == 0)
        {
            SceneManager.LoadScene("ClearScene");
        }
    }

    void Gravity()
    {
        //重力の方向を示す
        if (groundDirection == 0)
        {
            movedir = new Vector3(x, 0, z);
        }
        else if (groundDirection == 1)
        {
            movedir = new Vector3(x, z, 0);
        }
        else if (groundDirection == 2)
        {
            movedir = new Vector3(-x, z, 0);
        }
        else if (groundDirection == 3)
        {
            movedir = new Vector3(0, z, -x);
        }
        else if (groundDirection == 4)
        {
            movedir = new Vector3(0, z, x);
        }
    }

    public void OnMove()
    {
        //歩きやジャンプ等の基本的な操作
        if (!ito.onwool && !Input.GetButton("Fire1"))
        {
            if (oncol)
            {
                if (x != 0 || z != 0)
                {
                    transform.forward = Vector3.Slerp(transform.forward, movedir, Time.deltaTime * 10);
                    transform.Translate(0, 0, speed);
                    //transform.Translate(speed * x, 0, speed * z);
                }
            }
        }
        if (Input.GetButton("Fire1"))
        {
            Ito.SetActive(true);
            rb.velocity = Vector3.zero;
            oncol = false;
            //rb.useGravity = false;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            transform.parent = null;
            Ito.SetActive(false);
            //rb.useGravity = true;
            oncol = true;
            groundDirection = 0;
        }
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = Physics.gravity * -0.7f;
            onWool = false;
            groundDirection = 0;
            Physics.gravity = new Vector3(0, -8.91f, 0);
            Debug.Log("ジャンプ");
        }
    }

    void CatCatch()
    {
        //猫を捕まえる動作
        //Debug.Log(Distance);
        if (catchCollider.isCatch)
        {
            catchIMG.SetActive(true);
        }
        else
        {
            catchIMG.SetActive(false);
        }
    }

    void ItoEve()
    {
        //ワイヤーがオブジェクトにぶつかった時の動作
        if (ito.onwool)
        {
            Quaternion touchPosrote = ito.touchPosOBJ.transform.rotation;
            transform.parent = ito.touchPosOBJ.transform;
            //rb.useGravity = false;
            if (z > 0)
            {
                transform.Translate(0, 0, (ito.maxSpeed / 1.5f));
                cheakint = 1;

            }
            if (x != 0)
            {
                ito.touchPosOBJ.transform.Rotate(0, 1.5f * x, 0);
                cheakint = 2;
            }
        }
        else if (!onWool)
        {
            //rb.useGravity=true;
            if (cheakint == 2)
            {
                rb.velocity = new Vector3(-6 * x, 0, 0);
                cheakint = 0;
            }
            else if (cheakint == 1)
            {
                rb.velocity = new Vector3(0, 0, 6);
                cheakint = 0;
            }
        }
        if(ito.transform.localScale.z < 0)
        {
            Ito.SetActive(false);
        }
    }

    void OnWool()
    {
        //重力をどの方向に付けるか
        if (Physics.Raycast(transform.position, Vector3.forward,1))
        {
            groundDirection = 1;
            Physics.gravity = new Vector3(0, 0, 10);
            transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
        else if (Physics.Raycast(transform.position, -Vector3.forward, 1))
        {
            groundDirection = 2;
            Physics.gravity = new Vector3(0, 0, -10);
            transform.rotation = Quaternion.Euler(-90, 180, 0);
        }
        else if (Physics.Raycast(transform.position, Vector3.right,1))
        {
            groundDirection = 3;
            Physics.gravity = new Vector3(10, 0, 0);
            transform.rotation = Quaternion.Euler(-90, 90, 0);
        }
        else if(Physics.Raycast(transform.position,-Vector3.right, 1))
        {
            groundDirection = 4;
            Physics.gravity = new Vector3(-10, 0, 0);
            transform.rotation = Quaternion.Euler(-90, -90, 0);
        }
        else if(Physics.Raycast(transform.position, Vector3.down, 1))
        {
            groundDirection = 0;
            Physics.gravity = new Vector3(0, -10, 0);
        }
        else
        {
            return;
        }
        Debug.Log(groundDirection);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheakWall();
            oncol = true;
        if (CheakWall())
        {
            onWool = true;
            OnWool();
            rb.velocity = Vector3.zero;
            Debug.Log("壁歩き");
        }
        if(collision.gameObject.tag == "Groung")
        {
            onWool = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((transform.position + new Vector3(0, -Ypos, 0)) + Vector3.down * groundCheckOffsetY, 0.5f);
        Gizmos.DrawRay(transform.position + Vector3.up * offSetY, transform.forward);
    }


}
