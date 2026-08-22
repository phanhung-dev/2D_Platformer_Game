using System;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    [SerializeField] private bool hasKnockback = false;
    [SerializeField] private Vector2 knockbackForce = new Vector2(5f, 3f);

    [SerializeField] private Transform parentTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == transform.root.gameObject) return;

        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            if (hasKnockback && parentTransform != null)
            {
                float faceDirection = Mathf.Sign(parentTransform.localScale.x);
                Vector2 appliedKnockback = new Vector2(knockbackForce.x * faceDirection, knockbackForce.y);

                damageable.Hit(damage, appliedKnockback);
            } else
            {
                damageable.Hit(damage);
            }
        }
        
    }
}
