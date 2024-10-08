using UnityEngine;

public class BeetleAI : Destructable
{
    public float moveForwardDistance;  // �������� �� ��� Z ����� (������)
    public float moveDownDistance;     // �������� �� ��� Y ����
    public float moveBackwardDistance; // �������� �� ��� Z ����� (�����)
    public float moveUpDistance;       // �������� �� ��� Y �����

    public float speed;
    public int damage;
    public float knockbackForce;

    public GameObject playerPrefab;  // ������ �� ������ ������

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private int currentDirection = 0;

    void Start()
    {
        startPosition = transform.position;
        SetNextTarget();
    }

    void Update()
    {
        // ��������� ��������� ����
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // ���������, ������ �� ��� ����
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            SetNextTarget();
        }

        // ������������ ���� � ����������� ��������, ��������� �������� �� ��� X � Y
        Vector3 directionOfMovement = (targetPosition - startPosition).normalized;
        directionOfMovement.y = 0; // ���������� ������������ ������������ ��� �������������� �������� �� ��� X � Y
        if (directionOfMovement != Vector3.zero)
        {
            transform.forward = directionOfMovement;
        }
    }

    void SetNextTarget()
    {
        switch (currentDirection)
        {
            case 0: // �������� ����� �� ��� Z (�����)
                targetPosition = startPosition + new Vector3(0, 0, -moveBackwardDistance);
                break;
            case 1: // �������� ���� �� ��� Y
                targetPosition = startPosition + new Vector3(0, -moveDownDistance, -moveBackwardDistance);
                break;
            case 2: // �������� ����� �� ��� Z (������)
                targetPosition = startPosition + new Vector3(0, -moveDownDistance, 0);
                break;
            case 3: // �������� ����� �� ��� Y
                targetPosition = startPosition + new Vector3(0, 0, 0);
                break;
        }

        currentDirection = (currentDirection + 1) % 4;

        if (currentDirection == 0)
        {
            startPosition = targetPosition;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;

        if (collidedObject == playerPrefab)
        {
            // �������� ��������� Destructable � ������
            Destructable playerDestructable = collidedObject.GetComponent<Destructable>();
            if (playerDestructable != null)
            {
                // ������� ���� ������
                playerDestructable.ApplyDamage(damage);

                // ������������ ����������� ������������
                Vector3 directionToPlayer = collidedObject.transform.position - transform.position;
                Vector3 beetleForward = transform.forward;

                // ���������, ��������� �� ����� � ����������� �������� ����
                if (IsPlayerInFront(beetleForward, directionToPlayer))
                {
                    // ����������� ������ � ������� �������� ����
                    Rigidbody playerRb = collidedObject.GetComponent<Rigidbody>();
                    if (playerRb != null)
                    {
                        playerRb.AddForce(beetleForward * knockbackForce, ForceMode.Impulse);
                    }
                }
            }
        }
    }

    bool IsPlayerInFront(Vector3 beetleForward, Vector3 directionToPlayer)
    {
        // ���������, ��������� �� ����� ����� ����� �� ����������� ��� ��������
        float angle = Vector3.Angle(beetleForward, directionToPlayer);
        return angle < 45f;  // ����� ������ ���������� ����� �����
    }
}
