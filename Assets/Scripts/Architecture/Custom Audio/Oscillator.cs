using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Oscillator for audio synthesis
/*
 Based on Dano Kablamo's tutorial here: https://www.youtube.com/watch?v=GqHFGMy_51c

 
 
 */
public class Oscillator : MonoBehaviour
{
    public double frequency = 440.0;
    protected double increment;
    protected double phase;
    protected double sampling_frequency = 48000.0;

    public float gain;
    public float volume=0.1f;

    //Reference to other Controllers
    public EnvelopeController amplitudeController;


    //Returns a sine wave by default. Override in children functions
    protected virtual float GetWaveValue(float p)
    {
        return Mathf.Sin(p);       
    }


    protected void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0 * Mathf.PI / sampling_frequency;
    
        for (int i= 0; i<data.Length; i+= channels)
        {
            phase += increment;
            
            
            data[i] = (float)(gain*volume * amplitudeController.GetEnvelope()*GetWaveValue((float)phase));
            if (channels > 1)
            {
                data[i + 1] = data[i];
            }

            if (phase > (Mathf.PI * 2))
            {
                phase = 0.0;
            }

        }

        
    
    
    
    }

}
