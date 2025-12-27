using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField] private GameObject _normal;
    [SerializeField] private GameObject _cry;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bee"))
        {
            LevelManager.Instance.Loss();
            _normal.SetActive(false);
            _cry.SetActive(true);
        }
    }
}