using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public GameObject Ito;
    public float speed;
    [Header("プレイヤーの最高速度")] public float maxSpeed;
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
    [HideInInspector]public Vector3 movedir;
    float CatDistance;
    int groundDirection = 0;
    Vector3 lookPos;
    public Transform test;
    bool isRote;
    int roteInt;
    Quaternion playerRote;
    bool jumpBool = false;
    public GameObject camera;
    Transform cameraRote;
    bool onWire;
    bool onFire1 = false;
    Quaternion onWireRote;

    [Range(0f, 1f)]public float Ypos;
    public GameObject catchOBJ;
    CatchColliderScript catchCollider;

    Animator animator;
    public GameObject[] effects;

    [HideInInspector] public AudioSource AS;
    public AudioClip[] AC = new AudioClip[5];

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
        animator = GetComponent<Animator>();
        Ito.SetActive(false);
        ito = Ito.GetComponent<ItoScript>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        //catchOBJ.SetActive(false);
        catchCollider = catchOBJ.GetComponent<CatchColliderScript>();
        GameObject[] cats = GameObject.FindGameObjectsWithTag("cat");
        catlist.AddRange(cats);
        test.position = transform.position + new Vector3(0, 0, 1);
        cameraRote = camera.GetComponent<Transform>();
        AC[0] = (AudioClip)Resources.Load("Sounds/SE/RollerSkate");
        AC[1] = (AudioClip)Resources.Load("Sounds/SE/Wire");
        AC[2] = (AudioClip)Resources.Load("Sounds/SE/WireMove");
        AC[3] = (AudioClip)Resources.Load("Sounds/SE/CatCatch");
        AC[4] = (AudioClip)Resources.Load("Sounds/SE/Land");
        AS = GetComponent<AudioSource>();
        for(int i = 0;i < effects.Length;i++)
        {
            effects[i].SetActive(false);
        }
    }


    void Update()
    {
        transform.localScale = Vector3.one;
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        if (!Input.GetButton("Fire1"))
        {
            isGravity();
        }

        
        CheakGround();
        GravityDirection();
        //CheakWall();
        OnMove();
        CatCatch();
        ItoEve();
        LookRote();
        //Inspectorで重力の向きが見れるようにしている
        direction = Physics.gravity;
        catlist.RemoveAll(item => item == null);
        //重力をかけている
        rb.AddForce(rb.mass * Physics.gravity, ForceMode.Force);
        if(catlist.Count == 0)
        {
            Invoke("load", 1);
        }

        if(gameObject.transform.position.x >= 195)
        {
            transform.position = new Vector3(-190,transform.position.y,transform.position.z);
        }
        else if(gameObject.transform.position.x <= -195)
        {
            transform.position = new Vector3(190, transform.position.y, transform.position.z);
        }

        if (gameObject.transform.position.z >= 195)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,-190);
        }
        else if (gameObject.transform.position.z <= -195)
        {
            transform.position = new Vector3(195, transform.position.y, 190);
        }
        if(gameObject.transform.position.y <= -10)
        {
            transform.position = new Vector3(transform.position.x,50,transform.position.z);
        }

    }

    void load()
    {
        SceneManager.LoadScene("ClearScene");
    }


    void ForwardRote(Vector3 moveForward)
    {
        //Quaternion.Slerpをするために、別のオブジェクトの回転を進行方向に向ける(地面の位置によって軸の中心を変えている)
        if (groundDirection == 0)
        {
            test.transform.rotation = Quaternion.LookRotation(moveForward, Vector3.up);
        }
        else if (groundDirection == 1)
        {
            test.transform.rotation = Quaternion.LookRotation(moveForward, Vector3.back);
        }
        else if (groundDirection == 2)
        {
            test.transform.rotation = Quaternion.LookRotation(moveForward, Vector3.forward);
        }
        else if (groundDirection == 3)
        {
            test.transform.rotation = Quaternion.LookRotation(moveForward, Vector3.left);
        }
        else if (groundDirection == 4)
        {
            test.transform.rotation = Quaternion.LookRotation(moveForward, Vector3.right);
        }
    }

    void GravityDirection()
    {
        //重力の方向を示す
        if (groundDirection == 0)
        {
            movedir = new Vector3(1, 0, 1);
        }
        else if (groundDirection == 1)
        {
            movedir = new Vector3(1, 1, 0);
        }
        else if (groundDirection == 2)
        {
            movedir = new Vector3(1, 1, 0);
        }
        else if (groundDirection == 3)
        {
            movedir = new Vector3(0, 1, 1);
        }
        else if (groundDirection == 4)
        {
            movedir = new Vector3(0, 1, 1);
        }
    }

    void LookRote()
    {
        //カメラの方向から地面のベクトルを取得
        Vector3 cameraForward = Vector3.Scale(camera.transform.forward, movedir).normalized;
        //方向キーの入力値とカメラの向きから、移動方向の取得
        Vector3 moveForward = cameraForward * z + camera.transform.right * x;
        if (moveForward != Vector3.zero && !ito.onwool)
        {
            ForwardRote(moveForward);
        }
        if (ito.onwool)
        {
            transform.LookAt(ito.TouchPos);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, test.transform.rotation, 0.2f);
        }
        if(onFire1)
        {
            transform.rotation = onWireRote;
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
                    if(speed < maxSpeed)
                    {
                        speed += 0.001f;
                        animator.SetBool("WalkBool", true);
                    }
                    //進行方向に向いて、移動する
                    if (!AS.isPlaying && !jumpBool)
                    {
                        AS.PlayOneShot(AC[0]);
                    }
                    if (jumpBool)
                    {
                        AS.Stop();
                    }
                }
                else
                {
                    animator.SetBool("WalkBool", false);
                    if (speed > 0)
                    {
                        speed -= 0.001f;
                    }
                }
            }
        }
        transform.Translate(0, 0, speed);
        if (Input.GetButtonDown("Fire1"))
        {
            if (!ito.outbool)
            {
                groundDirection = 0;
                onWireRote = camera.transform.rotation;
                onFire1 = true;
                Ito.SetActive(true);
                speed = 0.03f;
            }
        }
        if (Input.GetButton("Fire1"))
        {
            if(!ito.outbool)
            {
                Ito.SetActive(true);
                rb.isKinematic = true;
                oncol = false;
                rb.velocity = Vector3.zero;
                //rb.useGravity = false;
                animator.SetBool("UPWireBool", true);
            }
            else
            {
                Ito.SetActive(false);
                animator.SetBool("WireBool", false);
                animator.SetBool("UPWireBool", false);
                rb.isKinematic = false;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            transform.parent = null;
            Ito.SetActive(false);
            rb.isKinematic = false;
            oncol = true;
            //groundDirection = 0;
            onFire1 = false;
            animator.SetBool("WireBool", false);
            animator.SetBool("UPWireBool", false);
            onWire = true;
            ito.outbool = true;
        }
        if (Input.GetButtonDown("Jump") && !jumpBool)
        {
            if(groundDirection == 0)
            {
                transform.position = transform.position + new Vector3(0,0.1f,0);
            }
            jumpBool = true;
            groundDirection = 0;
            rb.velocity = Physics.gravity * -1;
            onWool = false;
            animator.SetBool("JumpBool", true);
            effects[0].SetActive(true);
            effects[4].SetActive(true);
            effects[5].SetActive(true);
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
                if(!AS.isPlaying)
                {
                    AS.Stop();
                    AS.PlayOneShot(AC[2]);
                }
                animator.SetBool("WireBool", true);
                effects[2].SetActive(true);
                effects[3].SetActive(true);
                if (!AS.isPlaying)
                {
                    AS.Stop();
                    AS.PlayOneShot(AC[2]);
                }
            }
            if (x != 0)
            {
                //ito.touchPosOBJ.transform.Rotate(0, 1.5f * x, 0);
                transform.RotateAround(ito.TouchPos, Vector3.up, 1.5f * -x);
                transform.LookAt(ito.TouchPos);
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
            effects[2].SetActive(false);
            effects[3].SetActive(false);
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
            test.transform.rotation = Quaternion.AngleAxis(90,Vector3.left);
        }
        else if (Physics.Raycast(transform.position, -Vector3.forward, 1))
        {
            groundDirection = 2;
            test.transform.rotation = Quaternion.Euler(-90, 180, 0);
        }
        else if (Physics.Raycast(transform.position, Vector3.right,1))
        {
            groundDirection = 3;
            test.transform.rotation = Quaternion.Euler(-90, 90, 0);
        }
        else if(Physics.Raycast(transform.position,-Vector3.right, 1))
        {
            groundDirection = 4;
            test.transform.rotation = Quaternion.Euler(-90, -90, 0);
        }
        else if(Physics.Raycast(transform.position, Vector3.down, 1))
        {
            groundDirection = 0;
            //test.transform.rotation = Quaternion.identity;
        }
        if (jumpBool)
        {
            groundDirection = 0;
        }
        animator.SetBool("WallRunBool", true);
    }

    public void isGravity()
    {
        if(groundDirection == 0)
        {
            Physics.gravity = new Vector3(0, -10, 0);
        }
        else if(groundDirection == 1)
        {
            Physics.gravity = new Vector3(0, 0, 10);
        }
        else if(groundDirection == 2)
        {
            Physics.gravity = new Vector3(0, 0, -10);
        }
        else if(groundDirection == 3)
        {
            Physics.gravity = new Vector3(10, 0, 0);
        }
        else
        {
            Physics.gravity = new Vector3(-10, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        onWire = false;
        ito.outbool = false;
        if (collision.gameObject.tag == "Ground")
        {
            onWool = false;
            //test.transform.rotation = Quaternion.identity;
        }
            JumpEvent();
            animator.SetBool("WallRunBool", false);
            animator.SetBool("JumpKeepBool", false);
        effects[1].SetActive(true);
        effects[4].SetActive(false); 
        effects[5].SetActive(false);
    }

    private void OnCollisionStay(Collision collision)
    {
        CheakWall();
        oncol = true;
        jumpBool = false;
        animator.SetBool("JumpKeepBool", false);
        if (CheakWall())
        {
            onWool = true;
            OnWool();
            rb.velocity = Vector3.zero;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        jumpBool = true;
        groundDirection = 0;
        if (!Input.GetButton("Fire1"))
        {
            animator.SetBool("JumpKeepBool", true);
        }
        effects[4].SetActive(true);
        effects[5].SetActive(true);
    }

    void JumpEvent()
    {
        animator.SetBool("JumpBool", false);
    }


    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((transform.position + new Vector3(0, -Ypos, 0)) + Vector3.down * groundCheckOffsetY, 0.5f);
        Gizmos.DrawRay(transform.position + Vector3.up * offSetY, transform.forward);
    }*/
}
