using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script providing the ability to get a Powerup from a source.
/// </summary>
public class PowerupSource : MonoBehaviour
{
    [SerializeField]
    private Powerup m_Powerup;
    public Powerup Powerup { get =>  m_Powerup; }
}
