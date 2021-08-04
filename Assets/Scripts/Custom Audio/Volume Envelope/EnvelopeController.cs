using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Controls normal ADSR envelope parameters for a synthesizer.
 In its default implementation, it assumes linear ADSR curves.
 
 */
public class EnvelopeController : MonoBehaviour
{
    public enum EnvelopeState { OFF, ATTACK, DECAY, SUSTAIN, RELEASE}
    public EnvelopeState eState;
   
    protected float timer = 0;
    protected float returnLevel = 0;
    protected float releaseStartLevel = 0;

    [System.Serializable]
    public class EnvelopeParameters
    {
        public float attackTime;
        public float decayTime;
        public float sustainLevel;
        public float releaseTime;
    }

    public EnvelopeParameters parameters;

    //begins the ADSR cycle
    public void TriggerEnvelope()
    {
        timer = 0;
        eState = EnvelopeState.ATTACK;
        returnLevel = 0;
    }
    //signals release state
    public void TriggerReleaseEnvelope()
    {
        eState = EnvelopeState.RELEASE;
        timer = parameters.releaseTime-(returnLevel/parameters.sustainLevel)*parameters.releaseTime;
    }
    
    //Obtains return value between 0 and 1, representing the value for the envelope
    public float GetEnvelope()
    {
        return returnLevel;
    }

    //Helper functions for each state
    protected virtual void GetAttackLevel()
    {
        if (parameters.attackTime > 0)
        {
            returnLevel = Mathf.Lerp(0, 1, timer / parameters.attackTime);
            if (timer >= parameters.attackTime)
            {
                timer = 0;
                eState = EnvelopeState.DECAY;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            returnLevel = 1;
            eState = EnvelopeState.DECAY;
        }
        
    }
    protected virtual void GetDecayLevel()
    {
        if (parameters.decayTime > 0)
        {
            returnLevel = Mathf.Lerp(1, parameters.sustainLevel, timer / parameters.decayTime);
            if (timer >= parameters.decayTime)
            {
                timer = 0;
                eState = EnvelopeState.SUSTAIN;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            returnLevel = parameters.sustainLevel;
            eState = EnvelopeState.SUSTAIN;
        }
       
    }
    protected virtual void GetSustainLevel() {
        if (Mathf.Abs(parameters.sustainLevel) > 1) { parameters.sustainLevel = 1; } //Protection from bad user input
        returnLevel = parameters.sustainLevel;
    }
    protected virtual void GetReleaseLevel()
    {
        if (parameters.releaseTime > 0)
        {

            returnLevel = Mathf.Lerp(parameters.sustainLevel, 0, timer / parameters.releaseTime);
            if (timer >= parameters.releaseTime)
            {
                timer = 0;
                returnLevel = 0;
                eState = EnvelopeState.OFF;
            }
            else
            {
                timer += Time.deltaTime;
            }

        }
        else
        {
            returnLevel = 0;
            eState = EnvelopeState.OFF;
        }
    }

    protected void Update()
    {
        switch (eState)
        {
            case EnvelopeState.ATTACK:
                GetAttackLevel();
                break;
            case EnvelopeState.DECAY:
                GetDecayLevel();
                break;
            case EnvelopeState.RELEASE:
                GetReleaseLevel();
                break;
            case EnvelopeState.SUSTAIN:
                GetSustainLevel();
                break;
        }
    }
}
