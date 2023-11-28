using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask _doorLayer;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private Transform _playerHand;
    [SerializeField] private Vector3 _throwForce;
    [SerializeField] private Transform _interactableInventory;
    
    private Camera _camera;
    private RaycastHit _itemInHandInfo;
    private InteractableItem _item;
    private bool _isItemInHand;
    
    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var doorInfo, 2f, _doorLayer))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var door = doorInfo.collider.GetComponent<Door>();
                door.SwitchDoorState();
            }
        }
        
        if (Physics.Raycast(ray, out var hitInfo, 2f, _interactableLayer))
        {
            _item = hitInfo.collider.GetComponent<InteractableItem>();
            _item.SetFocus();

            if (Input.GetKeyDown(KeyCode.E))
            {
                TakeItemInHand(hitInfo);
            }
        }
        else
        {
            if (_item != null)
            {
                _item.RemoveFocus();
                _item = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_isItemInHand)
            {
                ThrowItem();   
            }
        }
    }

    private void TakeItemInHand(RaycastHit hitInfo)
    {
        if (_isItemInHand)
        {
            DropItem();
        }
        
        _isItemInHand = true;
        _itemInHandInfo = hitInfo;
        
        var rigidbody = _itemInHandInfo.rigidbody;
        rigidbody.isKinematic = true;
                
        var itemInHand = _itemInHandInfo.transform;
        itemInHand.SetParent(_playerHand);
        itemInHand.SetPositionAndRotation(_playerHand.position, _playerHand.rotation);
    }

    private void DropItem()
    {
            _itemInHandInfo.rigidbody.isKinematic = false;
            _itemInHandInfo.transform.SetParent(_interactableInventory, true);
            _isItemInHand = false;
    }

    private void ThrowItem()
    {
        DropItem();
        
        _itemInHandInfo.rigidbody.AddRelativeForce(_throwForce);
    }
}
