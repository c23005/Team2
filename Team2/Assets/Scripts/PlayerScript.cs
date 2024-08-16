using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public GameObject Ito;
    public float speed;
    [Header("�v���C���[�̍ō����x")] public float maxSpeed;
    ItoScript ito;
    Rigidbody rb;
    [HideInInspector]public float x;
    [HideInInspector]public float y;
    [HideInInspector]public float z;
    int cheakint = 0;
    bool oncol;
    List<GameObject> catlist = new List<GameObject>();
    public GameObject PlayerRotePos;
    Vector3 playeroldPos;
    CapsuleCollider capsuleCollider;
    [Header("�߂܂����鋗��������")]public float DisPos;
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

    [HideInInspector]public AudioSource AS;
    public AudioClip[] AC = new AudioClip[4];

    [Range(0f, 1f)]public float Ypos;
    public GameObject catchOBJ;
    CatchColliderScript catchCollider;

    [Min(0), Header("�ڒn����p�̒��S�_�̃I�t�Z�b�g")]
    public float groundCheckOffsetY = 0.3f;
    [Min(0), Header("�ڒn����̋���")]
    public float groundCheckDistance = 0;
    [Min(0), Header("�ڒn����̔��a")]
    public float groundCheckRadius = 0.5f;
    [Header("���̃��C���[�̃I�u�W�F�N�g�Ƀ��C�������������ɐڒn�����Ɣ��肷��")]
    public LayerMask groundLayers = 0;

    Vector3 groundNormal;
    RaycastHit hit;

    [Range(0f, 1f)]public float offSetY;
    [Range(0f, 1f)]public float Distance; 
    [Header("�d��(����悤������G��Ȃ�)")]public Vector3 direction;

    [Header("Wall�̃��C���[��I������")] public LayerMask WallLayer;

    Vector3 wallNormal;
    bool onWool;

    bool CheakGround()
    {    //Physics.SphereCast(�X�^�[�g�n�_,���a,���̒ʉ߂������,�����q�b�g�����ꏊ�Ɋւ�����,Cast�̍ő�̑傫��,���C���[�}�X�N(���C�L���X�g����Ƃ��ɑI��I�ɏՓ˂𖳎�����,�g���K�[�ɐݒ肳��Ă�����̂��Ώۂ��ǂ���))
         //��SphereCast�͊O���������肪�Ȃ�(���̂ق��ɂ͓����蔻�肪�Ȃ�)����Ypos�ňʒu�𒲐�����
        if (Physics.SphereCast((transform.position + new Vector3(0, -Ypos, 0)) + Vector3.down * groundCheckOffsetY, 0.5f, -transform.up, out hit, groundCheckOffsetY + groundCheckDistance, groundLayers, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log("���n");
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
        //�ǂ����邩�𒲂ׂ�
        if(Physics.Raycast(transform.position + Vector3.up * offSetY,transform.forward,out hit,Distance + capsuleCollider.radius, WallLayer, QueryTriggerInteraction.Ignore))
        {
            wallNormal = hit.normal;

            return true;
        }
        else
        {
            if (Physics.SphereCast((transform.position + new Vector3(0, -Ypos, 0)) + Vector3.down * groundCheckOffsetY, 0.5f, Vector3.down, out hit, groundCheckOffsetY + groundCheckDistance, WallLayer, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("���n");
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
        test.position = transform.position + new Vector3(0, 0, 1);
        cameraRote = camera.GetComponent<Transform>();
        AC[0] = (AudioClip)Resources.Load("Sounds/SE/RollerSkate");
        AC[1] = (AudioClip)Resources.Load("Sounds/SE/Wire");
        AC[2] = (AudioClip)Resources.Load("Sounds/SE/WireMove");
        AC[3] = (AudioClip)Resources.Load("Sounds/SE/CatCatch");
        AS = GetComponent<AudioSource>();
    }


    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        if (!Input.GetButton("Fire1"))
        {
            isGravity();
        }
        //�J�����̕�������n�ʂ̃x�N�g�����擾
        Vector3 cameraForward = Vector3.Scale(camera.transform.forward, movedir).normalized;
        //�����L�[�̓��͒l�ƃJ�����̌�������A�ړ������̎擾
        Vector3 moveForward = cameraForward * z + camera.transform.right * x;
        if(moveForward != Vector3.zero && !ito.onwool)
        {
            ForwardRote(moveForward);
        }
        if(ito.onwool)
        {
            transform.LookAt(ito.TouchPos);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, test.transform.rotation, 0.2f);
        }
        CheakGround();
        GravityDirection();
        //CheakWall();
        OnMove();
        CatCatch();
        ItoEve();
        //Inspector�ŏd�͂̌����������悤�ɂ��Ă���
        direction = Physics.gravity;
        catlist.RemoveAll(item => item == null);
        //�d�͂������Ă���
        rb.AddForce(rb.mass * Physics.gravity, ForceMode.Force);

        if(catlist.Count == 0)
        {
            Invoke("load", 1);
        }
    }

    void load()
    {
        SceneManager.LoadScene("ClearScene");
    }

    void ForwardRote(Vector3 moveForward)
    {
        //Quaternion.Slerp�����邽�߂ɁA�ʂ̃I�u�W�F�N�g�̉�]��i�s�����Ɍ�����(�n�ʂ̈ʒu�ɂ���Ď��̒��S��ς��Ă���)
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
        //�d�͂̕���������
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

    public void OnMove()
    {
        //������W�����v���̊�{�I�ȑ���
        if (!ito.onwool && !Input.GetButton("Fire1"))
        {
            if (oncol)
            {
                if (x != 0 || z != 0)
                {
                    if(speed < maxSpeed)
                    {
                        speed += 0.001f;
                    }
                    //�i�s�����Ɍ����āA�ړ�����
                    if(!AS.isPlaying && !jumpBool)
                    {
                        AS.PlayOneShot(AC[0]);
                    }
                    if(jumpBool)
                    {
                        AS.Stop();
                    }
                }
                else
                {
                    if(speed > 0)
                    {
                        speed -= 0.001f;
                    }
                }
            }
        }
        transform.Translate(0, 0, speed);
        if (Input.GetButtonDown("Fire1"))groundDirection = 0;
        if (Input.GetButton("Fire1"))
        {
            Ito.SetActive(true);
            test.transform.rotation = camera.transform.rotation;
            Ito.transform.rotation = camera.transform.rotation;
            rb.isKinematic = true;
            oncol = false;
            rb.velocity = Vector3.zero;
            //rb.useGravity = false;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            transform.parent = null;
            Ito.SetActive(false);
            rb.isKinematic = false;
            oncol = true;
            //groundDirection = 0;
        }
        if (Input.GetButtonDown("Jump") && !jumpBool)
        {
            if(groundDirection == 0)
            {
                transform.position = transform.position + new Vector3(0,0.1f,0);
            }
            jumpBool = true;
            groundDirection = 0;
            rb.velocity = Physics.gravity * -0.7f;
            onWool = false;
            Debug.Log("�W�����v");
        }
    }

    void CatCatch()
    {
        //�L��߂܂��铮��
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
        //���C���[���I�u�W�F�N�g�ɂԂ��������̓���
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
        }
        if(ito.transform.localScale.z < 0)
        {
            Ito.SetActive(false);
        }
    }

    void OnWool()
    {
        //�d�͂��ǂ̕����ɕt���邩
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
        Debug.Log(groundDirection);
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
        if (collision.gameObject.tag == "Ground")
        {
            onWool = false;
            //test.transform.rotation = Quaternion.identity;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        CheakWall();
        oncol = true;
        jumpBool = false;
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
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((transform.position + new Vector3(0, -Ypos, 0)) + Vector3.down * groundCheckOffsetY, 0.5f);
        Gizmos.DrawRay(transform.position + Vector3.up * offSetY, transform.forward);
    }*/
}
