using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MeshCreator : MonoBehaviour
{

    private float increment;
    public Transform pos;

    public List<Vector3> point = new List<Vector3>();
    public List<int> triangles = new List<int>();
    private PhotonView myView;

   /* private void OnDrawGizmos() {
        foreach(Vector3 v in point) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(-v+transform.position, 0.1f);
        }
    }*/

    void Start() { 

        myView = GetComponent<PhotonView>();
        transform.parent = null;
        
    }

    public void AddCurrentPos() {
        if (myView.IsMine) {
            myView.RPC("UpdateVertices", RpcTarget.All);
        }
    }

    [PunRPC]
    void UpdateVertices() {
        
        point.Add(new Vector3(pos.position.x - transform.position.x, 1, pos.position.z - transform.position.z));
        point.Add(new Vector3(pos.position.x - transform.position.x, 0, pos.position.z - transform.position.z));

        myView.RPC("UpdateTrail", RpcTarget.All);
    }

    [PunRPC]
    void UpdateTrail() {
        triangles.Clear();
        Vector3[] vertices = point.ToArray();
        
        for(int i = 0; i < (vertices.Length/2)-1; i++) {
            Vector2Int A = new Vector2Int(i*2, (i*2)+1);
            Vector2Int B = new Vector2Int(A.x + 2, A.y + 2);

            //First Face
            int one = A.y;
            int two = A.x;
            int three = B.x;

            int four = A.y;
            int five = B.x;
            int six = B.y;

            triangles.Add(one);
            triangles.Add(two);
            triangles.Add(three);
            triangles.Add(four);
            triangles.Add(five);
            triangles.Add(six);

            //Back Face
            int Bone = B.x;
            int Btwo = A.x;
            int Bthree = A.y;

            int Bfour = B.y;
            int Bfive = B.x;
            int Bsix = A.y;

            triangles.Add(Bone);
            triangles.Add(Btwo);
            triangles.Add(Bthree);
            triangles.Add(Bfour);
            triangles.Add(Bfive);
            triangles.Add(Bsix);
            
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
    }
}
