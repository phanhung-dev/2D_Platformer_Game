using System.Collections.Generic;
using UnityEngine;

public class DectectionZone : MonoBehaviour
{
    Collider2D col;
    public List<Collider2D> detectedColliders = new List<Collider2D>();

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player")) không cần so sánh bởi vì đã cài đặt tương tác chỉ với player
        //{
            detectedColliders.Add(collision);
        //}

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player"))
        //{
            detectedColliders.Remove(collision);
        //}
    }


}
