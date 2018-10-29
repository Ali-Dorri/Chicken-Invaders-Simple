using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulates motion of an object on a surface with friction. The gameObject it is attached to, should have rigidBody2D
/// to work correctly.
/// </summary>
public class EntityMovePhysics : MonoBehaviour, IPausable
{
    //
    //Fields
    //

    [SerializeField] float acceleration = 0;
    [SerializeField] float friction = 0;
    [SerializeField] float maxSpeed = 0;
    new Rigidbody2D rigidbody;
    Vector2 currenVelocity = new Vector2(0, 0);

    //constants
    const float DEFAULT_ACCELERATION = 9;
    const float DEFAULT_FRICTION = 4;
    const float DEFAULT_MAX_SPEED = 7;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public Rigidbody2D RigidBody
    {
        get
        {
            return rigidbody;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        if(acceleration <= 0)
        {
            acceleration = DEFAULT_ACCELERATION;
        }
        if(friction <= 0)
        {
            friction = DEFAULT_FRICTION;
        }
        if (maxSpeed <= 0)
        {
            maxSpeed = DEFAULT_MAX_SPEED;
        }

        rigidbody = GetComponent<Rigidbody2D>();
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void Move(Vector2 direction)
    {
        //find result velocity
        float frameAccelerate = acceleration * Time.deltaTime;
        Vector2 deltaVelocity = direction.normalized * frameAccelerate;
        Vector2 velocity = rigidbody.velocity + deltaVelocity;

        //move body by affecting friction
        AddFriction(velocity);
    }

    private void AddFriction(Vector2 velocity)
    {
        //find friction
        Vector2 frictionVector = -velocity.normalized * friction * Time.deltaTime;

        //
        //affect friction
        //

        Vector2 newVelocity = velocity + frictionVector;

        //if friction can change velocity.x sign
        if (newVelocity.x * velocity.x < 0)
        {
            newVelocity.x = 0;
        }

        //if friction can change velocity.y sign
        if (newVelocity.y * velocity.y < 0)
        {
            newVelocity.y = 0;
        }

        //newVelocity can be at max maxSpeed
        if (newVelocity.magnitude > maxSpeed)
        {
            newVelocity = newVelocity.normalized * maxSpeed;
        }

        rigidbody.velocity = newVelocity;
    }

    public void PauseOrResume(bool isPaused)
    {
        if (isPaused)
        {
            currenVelocity = rigidbody.velocity;
            rigidbody.velocity = new Vector2(0, 0);
        }
        else
        {
            rigidbody.velocity = currenVelocity;
        }
    }
}
