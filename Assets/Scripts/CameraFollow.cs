using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 posOffset;
    public Vector3 rotOffset;

    private void LateUpdate()
    {
        if (player == null)
            return;

        transform.position = player.position + posOffset;
        transform.eulerAngles = rotOffset;
    }

    //[InspectorButton("Set Camera Position")]
    //void SetCameraPos()
    //{
    //    if (player == null)
    //        return;

    //    transform.position = player.position + posOffset;
    //    transform.eulerAngles = rotOffset;
    //}
}
