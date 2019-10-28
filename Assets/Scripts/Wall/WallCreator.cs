using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WallCreator : MonoBehaviourPun, IPunObservable {

    [SerializeField] private List<Vector3> points = new List<Vector3>();
    [SerializeField] private GameObject currentWall;
    [SerializeField] private Vector3 wallOffset;

    private GameObject tempWall;
    private Vector3 movement;

    private Vector3 networkPosition, networkLocalScale;
    private Quaternion networkRotation;

    public enum MoveDirection {
        Left,
        Right,
        Up,
        Down
    }

    public MoveDirection lastDirection;


    public GameObject GetCurrentWall() {
        return currentWall;
    }
    private void OnDrawGizmos() {
        foreach(Vector3 v in points) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(v, 0.5f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (currentWall != null) {
            if (stream.IsWriting) {
                stream.SendNext(this.currentWall.transform.position);
                stream.SendNext(this.currentWall.transform.rotation);
                stream.SendNext(this.currentWall.transform.localScale);
            }
            else {
                networkPosition = (Vector3)stream.ReceiveNext();
                networkRotation = (Quaternion)stream.ReceiveNext();
                networkLocalScale = (Vector3)stream.ReceiveNext();

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTimestamp));
                networkPosition += (GetComponent<Rigidbody>().velocity * lag);
            }
        }
    }

    void FixedUpdate() {
        if (currentWall != null && photonView.IsMine) {
            UpdateWall();
        }
    }

    public void SpawnWall(MoveDirection direction) {
        points.Add(transform.position);
        lastDirection = direction;
        object[] initData = new object[1];
        initData[0] = GetComponent<PlayerController>().GetPlayerID();

        PhotonNetwork.Instantiate("WallCorner", transform.position+wallOffset, Quaternion.identity, 0, initData);

        tempWall = PhotonNetwork.Instantiate("Wall", transform.position + wallOffset, Quaternion.identity, 0, initData);
        tempWall.name = "TempWall";

        if (currentWall != null) {
            currentWall.GetComponent<WallPositionCorrect>().SetWallPosition(points[points.Count - 1] + wallOffset, points[points.Count - 2] + wallOffset, direction);
        }

        photonView.RPC("AsignWall", RpcTarget.All);
    }

    void UpdateWall() {
        if (photonView.IsMine) {
            float scale = (Vector3.Distance(points[points.Count - 1] + wallOffset, transform.position + wallOffset));
            Vector3 midPoint = middlePoint(points[points.Count - 1] + wallOffset, transform.position + wallOffset);

            if (lastDirection == MoveDirection.Up || lastDirection == MoveDirection.Down) {
                currentWall.transform.position = new Vector3(midPoint.x, midPoint.y, midPoint.z);
                currentWall.transform.localScale = new Vector3(scale, currentWall.transform.localScale.y, currentWall.transform.localScale.z);
            }
            else if (lastDirection == MoveDirection.Left || lastDirection == MoveDirection.Right) {
                currentWall.transform.position = new Vector3(midPoint.x, midPoint.y, midPoint.z);
                currentWall.transform.localScale = new Vector3(currentWall.transform.localScale.x, currentWall.transform.localScale.y, scale);
            }
        }
    }

    [PunRPC]
    void AsignWall() {
        if (photonView.IsMine) {
            currentWall = GameObject.Find("TempWall");
            currentWall.name = "Wall" + Time.time;
        }
    }

    Vector3 middlePoint(Vector3 pos1, Vector3 pos2) {
        return pos2 + (pos1 - pos2) / 2;
    }
}
