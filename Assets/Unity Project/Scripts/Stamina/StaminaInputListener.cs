using MoreMountains.CorgiEngine;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class StaminaInputListener : MonoBehaviour
{
    private Dictionary<CharacterAbility, float> m_AbilityCosts = new Dictionary<CharacterAbility, float>();

    // The Character script living on this
    [SerializeField] private Character m_CorgiCharacter;

    [SerializeField] private InputManager m_IM;
    private StaminaController m_StaminaController;

    private CharacterRoll m_RollAbility;

    private void Start()
    {
        m_CorgiCharacter = GetComponent<Character>();
        m_StaminaController = GetComponent<StaminaController>();
        m_IM = m_CorgiCharacter.LinkedInputManager; // TODO: May need to wait a sec?

        //

        CacheAbilities();
    }

    // + + + + | Functions | + + + +

    /// <summary>
    /// Collects and stores relevant CharacterAbilities.
    /// </summary>
    private void CacheAbilities()
    {
        m_RollAbility = GetComponent<CharacterRoll>();
        m_AbilityCosts.Add(m_RollAbility, 35f); // TODO: Const values?
        //m_IM.RollButton.ButtonDownMethod += HandleRollDown; // TODO: UNDO TO BIND DELEGATE
        Debug.Log("CharacterRoll added successfully!");
    }

    // + + + + | Input Handling | + + + +

    private void HandleRollDown() => HandleAbilityDown(m_RollAbility);

    private void HandleAbilityDown(CharacterAbility charAbility)
    {
        Debug.Log($"Handling Ability Down for {charAbility.name}| {charAbility.GetType()}");

        // If the Ability has a stamina cost in the dictionary,
        if (m_AbilityCosts.TryGetValue(charAbility, out float abilityStaminaCost))
        {
            // Then, check if the ability can be run.
            if (m_StaminaController.CanRemoveStaminaAmount(abilityStaminaCost))
            {
                charAbility.PermitAbility(true);
                m_StaminaController.RemoveStaminaAmount(abilityStaminaCost);
            }
            else
            {
                charAbility.PermitAbility(false);
            }
        }
    }
}