using UnityEngine;

public class enemyscript : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;

    private bool isChasing = true;


    void Update()
    {
        if (player == null || !isChasing) return;

        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        transform.position += direction * speed * Time.deltaTime;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //isChasing = false;
            speed = 0;
            Debug.Log("Enemy touched player — stopping!");

        }
    }

}
