using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

//just a puiblic class so all scripts can access dialogue list
public class Dialogue
{
    [SerializeField] List<string> lines;

    public List<string> Lines
    {
        get
        {
            return lines;
        }
    }
}
