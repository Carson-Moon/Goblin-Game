using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class ImpulseController : MonoBehaviour
{
    private List<ImpulseForce> impulseForces = new();


    [SerializeField] Vector3 direction;
    [SerializeField] float force;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            AddImpulse(direction, force);
        }
    }

    public Vector3 ApplyImpulseForces(KinematicCharacterMotor motor)
    {
        if(impulseForces.Count == 0)
            return Vector3.zero;

        Vector3 velocityToAdd = Vector3.zero;

        foreach(var impulse in impulseForces)
            velocityToAdd += impulse.Direction.normalized * impulse.Strength;

        impulseForces.Clear();

        motor.ForceUnground(time: 0);

        return velocityToAdd;
    }

    public void AddImpulse(Vector3 direction, float strength)
    {
        impulseForces.Add(new()
        {
            Direction = direction,
            Strength = strength
        });
    }
}
