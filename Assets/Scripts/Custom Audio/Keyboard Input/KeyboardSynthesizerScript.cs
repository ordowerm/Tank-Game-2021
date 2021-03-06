using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardSynthesizerScript : MonoBehaviour
{
    public PolyphonicOscillator target;
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
            target.PlayFrequency(f);
        }
    }
    public void StopNotes(List<float> freqs)
    {
        foreach (float f in freqs)
        {
            target.FreeFrequency(f);
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
       PlayNotes(GetFrequenciesDown());
       StopNotes(GetFrequenciesUp());
      
    }


    private void Awake()
    {
        SetDefaultKeys();  
    }
}
