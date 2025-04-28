using UnityEngine;

// Responsible for acting as an intermediary between our player input and the input the server receives.

public class ServerMovement_Goblin : MonoBehaviour
{
    [SerializeField] Vector2 rawMoveDirection;

    public void SetRawMoveDirection(Vector2 direction)
    {
        rawMoveDirection = direction;
    }

    public Vector2 GetRawMoveDirection()
    {
        return rawMoveDirection;
    }
}
