using UnityEngine;

public class RockMovement : MonoBehaviour
{
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
}
