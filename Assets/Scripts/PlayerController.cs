using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;


public class PlayerController : MonoBehaviourPun, IPunObservable, IPunInstantiateMagicCallback {
    [SerializeField] private float trailUpdateTime;
    [SerializeField] private int playerID;
    [SerializeField] private float speed = 1;
    [SerializeField] private float timing;
    
    [SerializeField] private Transform sectionSpawnPoint;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private GameObject endGameDummy;
    [SerializeField] private MeshRenderer bikeRenderer;


    private PhotonView view;
    public bool canControl = true;

    private float lag;
    private Rigidbody m_Body;
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private bool boost;

    public int GetPlayerID() {
        return playerID;
    }

    void Start()
    {
        canControl = view.IsMine;
        view.RPC("SetRigidbody", RpcTarget.All);
    }

    public void OnEnable() {
        PhotonNetwork.AddCallbackTarget(this);
        endGameDummy = GameObject.Find("GameRoomController").GetComponent<GameRoomController>().GetDummy();
    }

    public void OnDisable() {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        view = GetComponent<PhotonView>();
        if (view.IsMine) {
            view.RPC("SetPlayerID", RpcTarget.All, info.Sender.ActorNumber);
            view.RPC("SetPlayerColor", RpcTarget.All);
            SetPlayerName();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (m_Body != null) {
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
    }

    void FixedUpdate()
    {
        if (endGameDummy != null) {
            if (endGameDummy.activeSelf) {
                canControl = false;
            }
        }
        if (canControl) {
            MovePlayer(Time.deltaTime);
        }

        //Movement
        if (!view.IsMine) {
            m_Body.position = Vector3.MoveTowards(m_Body.position, networkPosition, Time.fixedDeltaTime);
            m_Body.rotation = Quaternion.RotateTowards(m_Body.rotation, networkRotation, Time.fixedDeltaTime * 100.0f);
        }
    }

    void MovePlayer(float time) {
        transform.Translate(speed * Vector3.forward * time);
        transform.position = transform.position;
    }

    void SetPlayerName() {
        gameObject.name = "myPlayer"; //set local gameobject name for the camera
        Camera.main.GetComponent<NetworkCamera>().AssignPlayer(this.gameObject.transform);
        playerName.text = PhotonNetwork.NickName;
    }

    private void Update() {
        if (canControl) {
            float rotationParam = transform.rotation.eulerAngles.y;
            rotationParam = Mathf.RoundToInt(rotationParam);
            WallCreator.MoveDirection moveDirection = WallCreator.MoveDirection.Up;

            //Rotation table for the wall orientation
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


            //Boost
            if (Input.GetKeyDown(KeyCode.B)) {
                if (!boost) {
                    boost = true;
                    StartCoroutine("Boost");
                }
            }
        }
    }

    IEnumerator Boost() {
        TextEvent.instance.AddMessage(PhotonNetwork.NickName + " Boosted!", TextEvent.Colors.YELLOW);
        photonView.RPC("BoostStartRPC", RpcTarget.All);
        yield return new WaitForSeconds(.5f);
        photonView.RPC("BoostStopRPC", RpcTarget.All);
        yield return new WaitForSeconds(5);
        boost = false;
    }


    //RPCs
    
    [PunRPC]
    void BoostStartRPC() {
        speed += 12;
    }
    [PunRPC]
    void BoostStopRPC() {
        speed -= 12;
    }

    [PunRPC]
    void SetRigidbody() {
        m_Body = GetComponent<Rigidbody>();
    }

    [PunRPC]
    public void SetPlayerID(int id) {
        playerID = id;
    }

    [PunRPC]
    public void SetPlayerColor() {
        bikeRenderer.material.color = GlobalPlayerColors.instance.GetPlayerColor(playerID);
    }

}
