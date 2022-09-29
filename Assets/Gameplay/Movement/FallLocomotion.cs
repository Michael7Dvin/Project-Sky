using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FallLocomotion : MonoBehaviour
{
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        _characterController.Move(Physics.gravity * Time.deltaTime);

    }
}
