using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class CameraBlocker : MonoBehaviour
{
    [SerializeField] private AliveEntity _aliveEntity;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _distance;

    private void LateUpdate()
    {
        Vector3 playerPos = _aliveEntity.transform.position;
        _camera.transform.position = new Vector3(playerPos.x, playerPos.y + _distance, playerPos.z );
    }
}
