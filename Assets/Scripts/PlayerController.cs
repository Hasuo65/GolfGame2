using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

/// <summary>
/// �v���[���[�̑���֌W
/// </summary>
public class PlayerController : MonoBehaviourPunCallbacks
{
    Vector2 initialVec;//�N���b�N�������̍��W
    Vector2 fireVec;//���˂���x�N�g��
    [SerializeField] private GameObject arrowImage;
    [SerializeField] private GameObject initialPoint;//initialVec�ɒu���_�̃I�u�W�F�N�g
    private GameObject ip;//initialPoint�̃C���X�^���X

    [System.NonSerialized]public bool IsOnDragZone = false;//ViewZoneCOntroller�ő��삷��

    private bool isStop = false;//�{�[�����ł�����~�܂��Ă��邩��bool
    [SerializeField] private int maxHitTime;
    private int hitTime = 0;

    private Rigidbody2D rb;//�A�o�^�[��RigidBody

    private SpriteRenderer sprite;//�A�o�^�[��SpriteRenderer
    private GameObject ball;//�{�[���̃I�u�W�F�N�g

    [SerializeField] private bool simulateTrail;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ball = transform.GetChild(0).transform.gameObject;
        sprite = ball.GetComponent<SpriteRenderer>();
        hitTime = maxHitTime;//�ŏ��͑ł��Ƃ��ł��Ȃ��悤�ɂ���
    }

    private void Update()
    {
        CheckStopped();
        if (IsOnDragZone&& hitTime < maxHitTime)
        {
            if (Input.GetMouseButton(0))
            {
                DuringClick();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnClickExit();
            }
        }
    }

    private bool firstStop = true;//�~�܂������Ɉ�񂾂��ĂԂ��߂�bool
    private float stopTimer;//�~�܂�����̎���

    private void CheckStopped()//�~�܂�܂ŐF��ς��Ȃ��悤�ɂ���
    {
        if(rb.velocity.magnitude <= 0.03f)
        {
            if (!isStop)//�{�[����ł�����J�E���g����
            {
                stopTimer += Time.deltaTime;
            }
            if (firstStop && stopTimer >= 1)
            {
                //---�����Ɏ~�܂������̏���������
                sprite.color = Color.white;
                hitTime = 0;
                //----
                firstStop = false;
                isStop = true;
                stopTimer = 0;
            }
        }
        else
        {
            sprite.color = Color.red;
        }
    }

    private bool clickFirst = true;//��ԍŏ��̃t���[���𔻒肷��bool

    private GameObject arrowObjectInstance;//���̃C���X�^���X

    public void DuringClick()
    {
        if (clickFirst)//���߂̃t���[���͂��ꂪ�Ăяo�����
        {
            initialVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            arrowObjectInstance = Instantiate(arrowImage, (Vector2)transform.position, Quaternion.FromToRotation(initialVec, fireVec) * Quaternion.Euler(0, 0, 90));
            ip = Instantiate(initialPoint, initialVec, Quaternion.identity);
            clickFirst = false;
            if (simulateTrail)
            {
                BeginSimulation();
            }
            return;
        }

        //���̃t���[������Ăяo�����
        fireVec = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - initialVec;
        arrowObjectInstance.transform.position = transform.position;//���̑���
        arrowObjectInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, fireVec.normalized) * Quaternion.Euler(0, 0, -90);//���̑���
        arrowObjectInstance.transform.localScale = new Vector2(0.3f, 0.1f) * fireVec.magnitude;//���̑���
        if (simulateTrail)
        {
            DuringSimulation();
        }
    }

    public void OnClickExit()//�h���b�O����߂���
    {
        Destroy(arrowObjectInstance);
        Destroy(ip);
        fireVec = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - initialVec;
        OwnerShoot(fireVec);
        photonView.RPC(nameof(Shoot), RpcTarget.Others, fireVec, (Vector2)transform.position);
    }

    private void OwnerShoot(Vector2 force)//�{�[���̏��L�҂̏���
    {
        rb.AddForce(force * -50);
        ExitGames.Client.Photon.Hashtable hashTable = PhotonNetwork.LocalPlayer.CustomProperties;
        hashTable["hit"] = (int)hashTable["hit"] + 1;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);
        hitTime++;
        isStop = false;
        firstStop = true;
        clickFirst = true;
    }

    [PunRPC]//�ق��̐l�̏���
            //PunRPC�ɂ��� https://zenn.dev/o8que/books/bdcb9af27bdd7d/viewer/2e3520
    private void Shoot(Vector2 force, Vector2 position)
    {
        rb.AddForce(force * -50);
        transform.position = position;//���O��h��
    }

    private void OnTriggerEnter2D(Collider2D collision)//�S�[������̓v���C���[���Ƃɍs��
    {
        if (collision.tag == "Goal"&&photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            ExitGames.Client.Photon.Hashtable hashTable = photonView.Owner.CustomProperties;
            hashTable["score"] = (int)hashTable["score"] + 1;
            photonView.Owner.SetCustomProperties(hashTable);
        }
    }


    //unity 2d�ł̒e���\�� https://www.matatabi-ux.com/entry/2018/11/08/100000

    /// <summary>
    /// �^���O��
    /// </summary>
    [SerializeField]
    private LineRenderer simulationLine = null;

    /// <summary>
    /// �ő�t�^�͗�
    /// </summary>
    private const float MaxMagnitude = 2f;

    /// <summary>
    /// ���˕����̗�
    /// </summary>
    private Vector3 currentForce = Vector3.zero;

    /// <summary>
    /// ���C���J����
    /// </summary>
    private Camera mainCamera = null;

    /// <summary>
    /// ���C���J�������W
    /// </summary>
    private Transform mainCameraTransform = null;

    /// <summary>
    /// �h���b�O�J�n�_
    /// </summary>
    //private Vector2 dragStart = Vector2.zero;

    /// <summary>
    /// �{�[���ʒu
    /// </summary>
    private Vector2 currentPosition = Vector2.zero;

    /// <summary>
    /// �Œ�t���[���E�F�C�g
    /// </summary>
    private static float DeltaTime;

    /// <summary>
    /// �Œ�t���[���҂�����
    /// </summary>
    private static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    /// <summary>
    /// ����������
    /// </summary>
    public void Awake()
    {
/*        mainCamera = Camera.main;
        mainCameraTransform = mainCamera.transform;*/
        DeltaTime = Time.fixedDeltaTime;
    }

    /// <summary>
    /// �}�E�X���W�����[���h���W�ɕϊ����Ď擾
    /// </summary>
    /// <returns></returns>
/*    private Vector3 GetMousePosition()
    {
        // �}�E�X����擾�ł��Ȃ�Z���W��⊮����
        var position = Input.mousePosition;
        position.z = mainCameraTransform.position.z;
        position = mainCamera.ScreenToWorldPoint(position);
        position.z = 0;

        return position;
    }*/

    /// <summary>
    /// �h���b�N�J�n�C�x���g�n���h��
    /// </summary>
    public void BeginSimulation()
    {
        currentPosition = rb.position;
    }

    /// <summary>
    /// �h���b�O���C�x���g�n���h��
    /// </summary>
    public void DuringSimulation()
    {
        //var position = GetMousePosition();

/*        currentForce = position - dragStart;
        if (currentForce.magnitude > MaxMagnitude * MaxMagnitude)
        {
            currentForce *= MaxMagnitude / currentForce.magnitude;
        }*/

        StartCoroutine(Simulation());
    }

    /// <summary>
    /// �O�Ղ�\�����ĕ`�悷��R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator Simulation()
    {

        // �����I�ȕ����^�����~������
        Physics.simulationMode = SimulationMode.Script;

        var points = new List<Vector2> { currentPosition };
        rb.AddForce(fireVec*-50,ForceMode2D.Impulse);

        // �^���̋O�Ղ��V�~�����[�V�������ċL�^����
        for (var i = 1; i < 16; i++)
        {
            Physics.Simulate(1);
            points.Add(transform.TransformPoint(transform.position));
            Debug.Log(transform.TransformPoint(transform.position));
        }

        // ���Ƃ̈ʒu�ɖ߂�
        rb.velocity = Vector2.zero;
        transform.position = currentPosition;

        // �\���n�_���Ȃ��ŋO�Ղ�`��
        simulationLine.positionCount = points.Count;
        List<Vector3> vec3List = new List<Vector3>();
        foreach(Vector2 point in points)
        {
            vec3List.Add(point);
        }
        simulationLine.SetPositions(vec3List.ToArray());

        Physics.simulationMode = SimulationMode.FixedUpdate;

        yield return WaitForFixedUpdate;
    }

    /*
        /// <summary>
        /// �h���b�O�I���C�x���g�n���h��
        /// </summary>
        public void OnShoot()
        {
            Flip(fireVec * 6f);
        }

        /// <summary>
        /// �{�[�����͂���
        /// </summary>
        /// <param name="force"></param>*/
/*    public void Flip(Vector3 force)
    {
        // �u�ԓI�ɗ͂������Ă͂���
        this.rb.AddForce(force, ForceMode.Impulse);
    }*/
}