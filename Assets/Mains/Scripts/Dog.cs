using UnityEngine;

public class Dog : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bee"))
        {
            LevelManager.OnDogBit?.Invoke();
        }
    }
}