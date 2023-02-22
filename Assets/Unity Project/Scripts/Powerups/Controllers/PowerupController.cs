using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    private Powerup m_CurrentPowerup;
    public Powerup CurrentPowerup { get => m_CurrentPowerup; }

    private Character m_Character;
    private InputManager m_InputManager;

    private FirePowerupAbility m_FirePowerupAbility;

    private void Start()
    {

        // Get InputManager
        m_Character = GetComponent<Character>();
        m_InputManager = m_Character.LinkedInputManager;

        m_FirePowerupAbility = GetComponent<FirePowerupAbility>();
    }

    // + + + + | Functions | + + + + 

    /// <summary>
    /// Tries to add a Powerup, returns true if the player doesn't already have one.
    /// </summary>
    /// <returns></returns>
    public bool TryAddPowerup(Powerup newPowerup)
    {
        if (m_CurrentPowerup == Powerup.NONE)
        {
            m_CurrentPowerup = newPowerup;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Sets the current powerup to Powerup.NONE.
    /// </summary>
    private void ClearPowerup()
    {
        m_CurrentPowerup = Powerup.NONE;
    }

    public void UsePowerup()
    {
        switch(m_CurrentPowerup)
        {
            case Powerup.NONE:
                return;
            case Powerup.FIRESPELL:
                //
                m_FirePowerupAbility.HandleStartAbility();
                return;
        }
    }
}
