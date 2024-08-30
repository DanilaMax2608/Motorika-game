using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // ������������ ���������� ��������
    private float currentHealth;

    public float moveSpeed = 5f; // �������� ������������ ������
    public float jumpForce = 10f; // ���� ������ ������
    private bool isGrounded = true; // ���� ��� ��������, ��������� �� ����� �� �����

    private Rigidbody rb; // ������ �� Rigidbody ������

    void Start()
    {
        currentHealth = maxHealth; // ������������� ��������
        rb = GetComponent<Rigidbody>(); // �������� ��������� Rigidbody
    }

    void Update()
    {
        // ���������� ��������� ������
        Move();

        // ���������, ���� ����� �������� ������ � ��������� �� �����, �� �� �������
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal"); // �������� ��� ��� ��������������� �������� (A/D ��� �������)
        Vector3 movement = new Vector3(moveInput * moveSpeed, rb.velocity.y, 0); // ������� ������ �� ��� X
        rb.velocity = movement; // ��������� �������� � Rigidbody
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // ������ �������� �� Y ��� ������
        isGrounded = false; // �������, ��� ����� � ������
    }

    void OnCollisionEnter(Collision collision)
    {
        // ���� ����� �������� �����, �� �� ����� ����� �������
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // ��������� �������� �� �������� �����
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");
        // ����� ����� �������� ������ ������ ������ (��������, ���������� ������)
    }
}
