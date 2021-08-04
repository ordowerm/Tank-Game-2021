using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 This script contains multiple oscillators to allow for polyphony.

 
 
 */
public class PolyphonicOscillator : MonoBehaviour
{
    public int maxNotes;
    public EnvelopeController.EnvelopeParameters envelopeParameters;
    public GameObject oscillatorPrefab; //a prefab containing an oscillator and an envelopecontroller
    protected List<Oscillator> oscillators; //list of all the oscillators
    protected Queue<Oscillator> freeOscillators; //queue structure containing all oscillators that currently are unused
    protected float pitchBend; //amount by which to bend the pitch -- implement once everything else has been addressed

    //Spawns GameObjects that contain Oscillators and envelopeControllers
    public void InitializeOscillators()
    {
       oscillators = new List<Oscillator>();
       freeOscillators = new Queue<Oscillator>();
       for (int i= 0; i < maxNotes; i++)
        {
            GameObject newOscillatorObject = Instantiate(oscillatorPrefab);
            newOscillatorObject.transform.parent = transform;
            newOscillatorObject.name = "Oscillator " + (i+1);
       
            try
            {
                newOscillatorObject.GetComponent<Oscillator>().amplitudeController.parameters = envelopeParameters; ;
                oscillators.Add(newOscillatorObject.GetComponent<Oscillator>());
                freeOscillators.Enqueue(newOscillatorObject.GetComponent<Oscillator>());
            }
            catch (MissingComponentException e)
            {
                Debug.LogError("Oscillator prefab doesn't contain an Oscillator component: " + e);
            }

        }
    }


    //Helper function for identifying which oscillator is playing, based on a given base_frequency.
    //Returns null if none are playing the given base_frequency.
    //Runtime O(n), where n is number of oscillators.
    protected Oscillator FrequencyIsPlaying(float freq)
    {
        //iterate through all oscillators to see if the note is already being played:
        Oscillator playing = null;
        for (int i = 0; i < oscillators.Count; i++)
        {
            if (
                freq == oscillators[i].base_frequency &&
                    !(oscillators[i].amplitudeController.eState == EnvelopeController.EnvelopeState.OFF ||
                    oscillators[i].amplitudeController.eState == EnvelopeController.EnvelopeState.RELEASE)
               )
            {
                playing = oscillators[i];
            }
        }
        return playing;
    }
    public virtual void PlayFrequency(float freq)
    {
        //if no oscillators match that base_frequency,
        if (!FrequencyIsPlaying(freq))
        {
            if (freeOscillators.Count > 0)
            {
                Oscillator currentOscillator = freeOscillators.Dequeue();
                currentOscillator.base_frequency = freq;
                currentOscillator.amplitudeController.TriggerEnvelope();
            }
        }
    }
    public void FreeFrequency(float freq)
    {
        Oscillator osc = FrequencyIsPlaying(freq);
        if (osc)
        {
            osc.amplitudeController.TriggerReleaseEnvelope();
            freeOscillators.Enqueue(osc);
        }
    }


    protected void Awake()
    {
        InitializeOscillators();
    }
}
