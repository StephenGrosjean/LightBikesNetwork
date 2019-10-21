using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;


public class PlayerController : MonoBehaviourPun, IPunObservable, IPunInstantiateMagicCallback {
    [SerializeField] private int playerID;
    [SerializeField] private float speed = 1;
    private PhotonView view;
    public bool canControl = true;
    [SerializeField] private float timing;
    [SerializeField] private Transform sectionSpawnPoint;
    [SerializeField] private float trailUpdateTime;
    [SerializeField] private TextMeshProUGUI playerName;
    public float lag;
    public MeshCreator lightTrail;

    private Rigidbody m_Body;
    private Vector3 networkPosition;
    private Quaternion networkRotation;

    void Start()
    {
        canControl = view.IsMine;
        view.RPC("SetRigidbody", RpcTarget.All);

    }
    public void OnEnable() {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable() {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    [PunRPC]
    void SetRigidbody() {
        m_Body = GetComponent<Rigidbody>();
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        view = GetComponent<PhotonView>();
        if (view.IsMine) {
            view.RPC("SetPlayerID", RpcTarget.All, info.Sender.ActorNumber);
            view.RPC("SetPlayerColor", RpcTarget.All);
            SetPlayerName();
        }
    }

    public int GetPlayerID() {
        return playerID;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(this.m_Body.position);
            stream.SendNext(this.m_Body.rotation);
            stream.SendNext(this.m_Body.velocity);
        }
        else {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            m_Body.velocity = (Vector3)stream.ReceiveNext();

            lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            networkPosition += (this.m_Body.velocity * lag);
        }
    }

    void FixedUpdate()
    {
        if (canControl) {
            MovePlayer(Time.deltaTime);
        }

        if (!view.IsMine) {
            m_Body.position = Vector3.MoveTowards(m_Body.position, networkPosition, Time.fixedDeltaTime);
            m_Body.rotation = Quaternion.RotateTowards(m_Body.rotation, networkRotation, Time.fixedDeltaTime * 100.0f);
        }
    }

    void MovePlayer(float time) {
        transform.Translate(speed * Vector3.forward * time);
        transform.position = transform.position;
    }

    [PunRPC]
    public void SetPlayerID(int id) {
        playerID = id;
    }

    [PunRPC]
    public void SetPlayerColor() {
        GetComponentInChildren<MeshRenderer>().material.color = GlobalPlayerColors.instance.GetPlayerColor(playerID);
    }

    void SetPlayerName() {
        gameObject.name = "myPlayer";
        Camera.main.GetComponent<NetworkCamera>().AssignPlayer(this.gameObject.transform);
        playerName.text = PhotonNetwork.NickName;
    }

    private void Update() {
        if (canControl) {
            float rotationParam = transform.rotation.eulerAngles.y;
            rotationParam = Mathf.RoundToInt(rotationParam);
            WallCreator.MoveDirection moveDirection = WallCreator.MoveDirection.Up;

            if (rotationParam == 0 || rotationParam == 360 || rotationParam == -360) {
                moveDirection = WallCreator.MoveDirection.Up;
            }else if(rotationParam == 90 || rotationParam == -270) {
                moveDirection = WallCreator.MoveDirection.Right;
            }
            else if (rotationParam == 180 || rotationParam == -180) {
                moveDirection = WallCreator.MoveDirection.Down;
            }
            else if (rotationParam == -90 || rotationParam == 270) {
                moveDirection = WallCreator.MoveDirection.Left;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                GetComponent<WallCreator>().SpawnWall(moveDirection);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                transform.Rotate(new Vector3(0, 90, 0), Space.Self);
                GetComponent<WallCreator>().SpawnWall(moveDirection);
            }
        }
    }
}
