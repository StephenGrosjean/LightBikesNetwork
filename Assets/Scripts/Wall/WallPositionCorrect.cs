using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPositionCorrect : MonoBehaviour
{
    public void SetWallPosition(Vector3 pos1, Vector3 pos2, WallCreator.MoveDirection direction) {
        float scale = (Vector3.Distance(pos1, pos2));
        Vector3 midPoint = middlePoint(pos1, pos2);

        if (direction == WallCreator.MoveDirection.Left || direction == WallCreator.MoveDirection.Right) {
            transform.position = new Vector3(midPoint.x, midPoint.y, midPoint.z);
            transform.localScale = new Vector3(scale, transform.localScale.y, transform.localScale.z);
        }
        else if (direction == WallCreator.MoveDirection.Up || direction == WallCreator.MoveDirection.Down) {
            transform.position = new Vector3(midPoint.x, midPoint.y, midPoint.z);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, scale);
        }
    }

    Vector3 middlePoint(Vector3 pos1, Vector3 pos2) {
        return pos2 + (pos1 - pos2) / 2;
    }
}
