/*
 * Interface used to make sure all visualisation managers implement the same behaviors - such as the ChosenTrial variable.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I3DVisualization
{
    public Trial ChosenTrial
    { get; }
}
