using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName =  "Volume", menuName = "Gry/Volume")]
public class Volume : ScriptableObject
{
    [SerializeField] private float volume = 0;

    public float getVolume()
    {
        return volume;
    }

    public void setVolume(float vol)
    {
        volume = vol;
    }
}
