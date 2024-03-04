using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private float trapDamage = 20f;
    [SerializeField]
    private float hell = 100f;
    [SerializeField]
    private float healing = 10f;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Trap"))
        {
            TakeDamage(trapDamage);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        if (collision.gameObject.CompareTag("hell"))
        {
            TakeDamage(hell);
            if (currentHealth <= 0)
            {
                Die();
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cherry"))
        {

            Healing();
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
    public void Healing()
    {

        currentHealth += healing;
        healthBar.SetHealth(currentHealth);
    }

    private void Die()
    {
        anim.SetTrigger("death");
        rb.bodyType = RigidbodyType2D.Static;
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene("KhanhScene");
    }
}
