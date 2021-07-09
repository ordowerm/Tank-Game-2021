using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 Interface for use with observer pattern
 */
public interface ITextDisplayListener
{
    public void NotifyTextRenderComplete(TextDisplayer td);
}
