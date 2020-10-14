using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMover : MonoBehaviour
{
    private CharacterController _myController;
    private Vector3 _motionVector;
   // private Vector3 _motionFactor;

    private IMovementComponent[] _movementComps;

    // Start is called before the first frame update
    private void Awake() 
    {
        _myController = GetComponent<CharacterController>();
        ResetCharacter(transform);
        _movementComps = GetComponents<IMovementComponent>();
        
    }
        
    
    // Update is called once per frame
    private void Update()
    {
        _motionVector = Vector3.zero;
        for (int i = 0; i < _movementComps.Length; i++)
        {
            _motionVector += _movementComps[i].MovementVector;
            _motionVector.x *= _movementComps[i].FactorVector.x;
            _motionVector.y *= _movementComps[i].FactorVector.y;
            _motionVector.z *= _movementComps[i].FactorVector.z;
        }

        //print(_movementComps.Length);
       // print(_motionVector);
        _myController.Move(_motionVector * Time.deltaTime);
        

    }

    /// <summary>
    /// Reset the character into a new position without breaking the character
    /// controller collisions
    /// </summary>
    /// <param name="newLocation"> Where to teleport the player to.</param>
    public void ResetCharacter(Transform newLocation, bool resetMovement = true)
    {
        if(resetMovement) _motionVector = Vector3.zero;
        _myController.enabled = false;
        transform.position = newLocation.position;
        transform.rotation = newLocation.rotation;
        _myController.enabled = true;

    }
}
