using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask _doorLayer;
    [SerializeField] private LayerMask _interactableLayer;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, 2f, _doorLayer))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var door = hitInfo.collider.GetComponent<Door>();
                door.SwitchDoorState();
            }
        }
    }
}
