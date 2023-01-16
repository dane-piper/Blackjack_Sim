using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attributes : MonoBehaviour
{
    public Card cards;
    public int suite;
    public int face;

    public void initialize()
    {
        suite = (int)cards.Suite;
        face = cards.Value;

    }
}
