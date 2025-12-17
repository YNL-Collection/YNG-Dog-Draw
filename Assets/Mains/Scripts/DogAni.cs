using UnityEngine;

public class DogAni : MonoBehaviour
{
    [Header("Cấu hình hiệu ứng")]
    public float speed = 3f;        // Tốc độ phập phồng
    public float intensity = 0.1f;  // Độ mạnh của hiệu ứng

    private Vector3 initialScale;

    void Start()
    {
        // Lưu lại kích thước ban đầu của con chó
        initialScale = transform.localScale;
    }

    void Update()
    {
        float time = Time.time * speed;

        // SỬA LỖI: Chuyển X, Y thành x, y (viết thường)
        float scaleX = Mathf.Sin(time) * intensity;
        float scaleY = Mathf.Sin(time + Mathf.PI) * intensity;

        // Áp dụng vào scale
        transform.localScale = new Vector3(
            initialScale.x + scaleX, // Đã sửa thành x thường
            initialScale.y + scaleY, // Đã sửa thành y thường
            initialScale.z           // Đã sửa thành z thường
        );
    }
}