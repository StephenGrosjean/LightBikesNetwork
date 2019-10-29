using UnityEngine;
using Photon.Pun;

public class PlayerDeath : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject model;

    private WallCreator wallCreator;
    private PlayerController playerController;
    private bool dead = false;

    private void Awake() {
        wallCreator = GetComponent<WallCreator>();
        playerController = GetComponent<PlayerController>();
    }

    //Detect collisions 
    private void OnCollisionEnter(Collision collision) {
        if (photonView.IsMine) {
            if (collision.collider.tag == "Wall" && collision.gameObject != wallCreator.GetCurrentWall()) {
                Death();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (photonView.IsMine) {
            if (other.tag == "Wall" && other.gameObject != wallCreator.GetCurrentWall()) {
                Death();
            }
        }
    }

    public void Death() {
        if (!dead && photonView.IsMine) {
            SoundManager.instance.PlayEffect(SoundManager.Effect.DESTROY);
            dead = true;
            playerController.canControl = false;

            //Create initData for the instantiated object
            object[] initData = new object[1];
            initData[0] = playerController.GetPlayerID();

            //Instantiate the deathObject with the init data
            PhotonNetwork.Instantiate("DeathObj", transform.position, transform.rotation, 0, initData);


            GameRoomController.instance.RemovePlayer(photonView.OwnerActorNr); // Remove the player from the GameRoomController
            GameRoomController.instance.SetEndGameText(); //Set the EndGameText
            photonView.RPC("DisableModel", RpcTarget.All); //Disable the 3d model
            
        }
    }

    //RPC to disable 3D Model on all clients
    [PunRPC]
    private void DisableModel() {
        model.SetActive(false);
    }
}
