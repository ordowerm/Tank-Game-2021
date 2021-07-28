using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardSynthesizerScript : MonoBehaviour
{
    public Oscillator target;
    public KeyCode[] keys;
    MusicNotes notes = new MusicNotes();
    int minimumNoteId = 51; //default note is C3

    public List<float> GetFrequenciesDown()
    {
        List<float> freqs = new List<float>();
        for (int i= 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                freqs.Add(notes.GetNote(minimumNoteId + i));
            }
        }
        return freqs;
    }
    public List<float> GetFrequenciesUp()
    {
        List<float> freqs = new List<float>();
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyUp(keys[i]))
            {
                freqs.Add(notes.GetNote(minimumNoteId + i));
            }
        }
        return freqs;
    }
    
    
    public void PlayNotes(List<float> freqs)
    {
        foreach (float f in freqs)
        {
            target.frequency = f;
            target.amplitudeController.TriggerEnvelope();
        }
    }


    //Sets default keys if keys[] has length == 0
    protected void SetDefaultKeys()
    {
        keys = new KeyCode[13]
        {
            KeyCode.A,
            KeyCode.W,
            KeyCode.S,
            KeyCode.E,
            KeyCode.D,
            KeyCode.F,
            KeyCode.T,
            KeyCode.G,
            KeyCode.Y,
            KeyCode.H,
            KeyCode.U,
            KeyCode.J,
            KeyCode.K

        };
    }


    // Update is called once per frame
    void Update()
    {
        if (GetFrequenciesUp().Count > 0)
        {
            target.amplitudeController.TriggerReleaseEnvelope();

        }
        PlayNotes(GetFrequenciesDown());
      
    }


    private void Awake()
    {
        SetDefaultKeys();  
    }
}
