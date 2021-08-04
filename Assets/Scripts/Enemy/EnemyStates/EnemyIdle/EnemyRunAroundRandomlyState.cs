using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 The silliest enemy AI. The enemy moves randomly.

 Note: if fixed RNG mode is activated in the game settings, we might have to mix things up.
 
 */
public class EnemyRunAroundRandomlyState : EnemyIdle
{

    //Constants
    const float maxSpeed = 5;
    const float pollDelay = 0.2f; //Delay between calls to the coroutine
    const float decisionProb = 0.5f; //likelihhood of making a decision during any frame
    const float acceleration = 0.5f;
    const float maxRotation = 15*Mathf.PI/180f;
    const float animationTimeScaleMultiplierMin = 0.2f;
    const float animationTimeScaleMultiplierMax = 1.3f; //the animation time scale should vary depending on enemy speed / maximum speed. This multiplier speeds up or slows down the base animation speed.
    
    //Runtime variables
    Rigidbody2D rb;
    public EnemyRunAroundRandomlyState(GameObject t, GameStateMachine s) : base(t, s)
    {    
        rb = esm().GetComponent<Rigidbody2D>();
    }


    //Checks if the velocity is less than or greater than 0
    protected void CalculateOrientation() {
        if (rb.velocity.x < 0)
        {
            esm().SetOrientation(EnemyStateMachine.Orientation.LEFT);
        }
        else if (rb.velocity.x > 0)
        {
            esm().SetOrientation(EnemyStateMachine.Orientation.RIGHT);
        }
    }

    //Helper functions representing the decisions that can be made by the AI:
    void Accelerate()
    {
        if (rb.velocity.magnitude > 0)
        {
            rb.velocity *= 1 + acceleration;
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized*maxSpeed;
            }
        }
        else
        {
            rb.velocity = Random.insideUnitCircle.normalized*(acceleration); //sets the thing going in a random direction
            CalculateOrientation();
            
            
        }
    }  
    void Turn()
    {
        float angle = Random.Range(-maxRotation, maxRotation);
        rb.velocity = new Vector2(rb.velocity.x * Mathf.Cos(angle) - rb.velocity.y * Mathf.Sin(angle), rb.velocity.x * Mathf.Sin(angle) + rb.velocity.y * Mathf.Cos(angle));
        CalculateOrientation();
    }
    void Decelerate()
    {
        rb.velocity *= 1 - acceleration;

    }
    
    //This function selects an action based on a random integer.
    void MakeDecision()
    {
        int slots = 12; 
        /*
         
        The chosen decision is given by a random number, modulo slots.
        Each possible action is given by one or more of the cases.
        
        You can skew the distribution by having multiple cases fall through to the same action.
         
         */
        switch (Random.Range(0,100) % slots)
        {
            default:
                Accelerate();
                break;
            case 1:
            case 5:
            case 6: 
                Turn();
                break;
            case 2:
                Decelerate();
                break;
        }
    }



    //Overrides:

    public override void OnEnter()
    {
        //Debug.Log("In RunAroundRandomly OnEnter");
        base.OnEnter();
        _sm.StartCoroutine(DecisionChecker());
    }
    public override void OnExit()
    {
        _sm.StopCoroutine(DecisionChecker());
        base.OnExit();

    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        esm().SetAnimatorTimeScale(Mathf.Lerp(animationTimeScaleMultiplierMin,animationTimeScaleMultiplierMax,rb.velocity.magnitude/maxSpeed)); //updates animation speed
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    //Coroutine performing AI checks.
    IEnumerator DecisionChecker()
    {
        for (; ; )
        {
           // Debug.Log("Checking for a decision");
            //If the decision threshold is met, make a decision
            if (Random.value < decisionProb)
            {
                //Debug.Log("Making a decision");
                MakeDecision();
            }

            yield return new WaitForSeconds(pollDelay);
        }
    }


}
