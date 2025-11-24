using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    void Update()
    {
        // Leer input (WASD o flechas)
        float h = Input.GetAxisRaw("Horizontal"); // A/D - Izq/Der
        float v = Input.GetAxisRaw("Vertical");   // W/S - Arriba/Abajo

        // Vector de movimiento en plano XZ
        Vector3 dir = new Vector3(h, 0f, v).normalized;

        // Aplicar movimiento por transform
        transform.position += dir * speed * Time.deltaTime;

        // Si querés que rote en dirección del movimiento:
        if (dir.sqrMagnitude > 0.001f)
        {
            transform.forward = dir; // hace que mire hacia donde va
        }
    }
}
