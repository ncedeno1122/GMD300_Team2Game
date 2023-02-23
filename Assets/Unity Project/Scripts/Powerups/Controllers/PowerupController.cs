using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    protected string _logClassPrefix { get => this.GetType().Name + " -"; }

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
            MMDebug.DebugLogTime($"{_logClassPrefix} Added new powerup {newPowerup}!");
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

    /// <summary>
    /// Handles the user-input request for using a Powerup. Uses one if we have one, tries to find one if not.
    /// </summary>
    public void UsePowerup()
    {
        MMDebug.DebugLogTime($"{_logClassPrefix} Using Powerup - {m_CurrentPowerup}");

        switch(m_CurrentPowerup)
        {
            case Powerup.NONE:
                TryFindPowerupSource();
                return; // Find PowerupSource and RETURN so we don't clear what we get.
            case Powerup.FIRESPELL:
                m_FirePowerupAbility.HandleStartAbility();
                break;
        }

        ClearPowerup();
    }

    /// <summary>
    /// Handles the attempt to find a PowerupSource, setting
    /// </summary>
    /// <returns></returns>
    private bool TryFindPowerupSource()
    {
        PowerupSource attemptedPUpSrc = CastForPowerupSource();

        if (attemptedPUpSrc != null)
        {
            MMDebug.DebugLogTime($"{_logClassPrefix} Found & Set our current powerup to {attemptedPUpSrc.Powerup}!");
            m_CurrentPowerup = attemptedPUpSrc.Powerup;
            return true;
        }
        else
        {
            MMDebug.DebugLogTime($"{_logClassPrefix} Couldn't find a Powerup Source, couldn't set current powerup...");
            return false;
        }
    }

    /// <summary>
    /// A function that attempts to find a PowerupSource on the Background Layer.
    /// </summary>
    /// <returns></returns>
    private PowerupSource CastForPowerupSource()
    {
        // TODO: use a physics2d cast to get a powerup source and return it
        LayerMask bgLayer = LayerMask.GetMask("Background");
        List<Collider2D> overlapResults = new(); // TODO: Parameterize & Pool?
        Physics2D.OverlapBox(transform.position, Vector2.one, 0f, new ContactFilter2D() { layerMask = bgLayer, useLayerMask = true, useTriggers = true }, overlapResults);

        // Iterate
        PowerupSource detectedPupSrc = null;
        foreach (Collider2D c2d in overlapResults)
        {
            if (c2d.GetComponent<PowerupSource>() is PowerupSource foundSrc)
            {
                detectedPupSrc = foundSrc;
                break; // Only return the first found one!
            }
        }

        // Process & return result
        if (detectedPupSrc)
        {
            MMDebug.DebugLogTime($"{_logClassPrefix} Found {detectedPupSrc.gameObject.name}'s collider AND PowerupSource!");
            return detectedPupSrc;
        }
        else
        {
            MMDebug.DebugLogTime($"{_logClassPrefix} Found nothing...");
            return null;
        }
    }

    // + + + + | Gizmos | + + + +

    public void OnDrawGizmos()
    {
        switch(m_CurrentPowerup)
        {
            case Powerup.NONE:
                Gizmos.color = Color.white - new Color(0f, 0f, 0f, 0.5f);
                break;
            case Powerup.FIRESPELL:
                Gizmos.color = Color.red - new Color(0f, 0f, 0f, 0.5f);
                break;
        }
        Gizmos.DrawSphere(transform.position + Vector3.up, 0.25f);
    }
}
