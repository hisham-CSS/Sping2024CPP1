using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public abstract class Enemy : MonoBehaviour
{
    //private - private to the class that has created it. Only a property of that class.  Even child classes cannot see this variable.
    //public - it's a party and every is invited. This is a public property that anybody who has access to the class or object that has this class attached, can retrive and access.
    //protected - private but accessable to child classes
    protected SpriteRenderer sr;
    protected Animator anim;

    protected int health;
    [SerializeField] protected int maxHealth;

    //void functions = functions that have no return type and require no return values
    //variable functions = functions that will return a value of the type that they are
    public virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (maxHealth <= 0) maxHealth = 10;

        health = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            anim.SetTrigger("Death");

            if (transform.parent != null)
                Destroy(transform.parent.gameObject, 2);
            else
                Destroy(gameObject, 2);
        }
    }
}
