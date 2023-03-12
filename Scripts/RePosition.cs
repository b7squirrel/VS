using UnityEngine;

public class RePosition : MonoBehaviour
{
    Collider2D col;
    
    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = Player.instance.transform.position;
        Vector3 myPos = transform.position;
        float diffx = Mathf.Abs(playerPos.x - myPos.x);
        float diffy = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = Player.instance.InputVec;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffx > diffy)
                {
                    transform.Translate(Vector3.right * dirX * 80);
                }
                else if (diffx < diffy)
                {
                    transform.Translate(Vector3.up * dirY * 80);
                }
                break;

            case "Enemy":
                if (col.enabled)
                {
                    Enemy enemy = GetComponent<Enemy>();
                    if(enemy != null) enemy.Timer = 8f; // 너무 멀리 떨어져 버린 후 8초가 지나면 스폰 포인트로 강제 이동시키기 위해서
                    transform.position += new Vector3(playerDir.x * 40f, playerDir.y * 50f, transform.position.z);
                }
                break;
        }
    }
}
