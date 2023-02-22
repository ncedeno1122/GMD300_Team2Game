using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    [SerializeField]
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

        TryAddPowerup(Powerup.FIRESPELL);

        // Delegates
        m_InputManager.SecondaryShootButton.ButtonDownMethod += UsePowerup;
    }

    private void OnEnable()
    {
        if (!m_InputManager) return;
        m_InputManager.SecondaryShootButton.ButtonDownMethod += UsePowerup;
    }

    private void OnDisable()
    {
        if (!m_InputManager) return;
        m_InputManager.SecondaryShootButton.ButtonDownMethod -= UsePowerup;
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
            MMDebug.DebugLogTime($"Added new powerup {newPowerup}!");
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
        MMDebug.DebugLogTime($"Using Powerup - {m_CurrentPowerup}");

        switch(m_CurrentPowerup)
        {
            case Powerup.NONE:
                return; // Returns and DOESN'T clear the powerup.
            case Powerup.FIRESPELL:
                m_FirePowerupAbility.HandleStartAbility();
                break;
        }

        ClearPowerup();
    }
}
