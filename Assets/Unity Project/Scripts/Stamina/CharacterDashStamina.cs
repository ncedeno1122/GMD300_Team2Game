using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDashStamina : CharacterDash
{
    [Header("Stamina")]
    public float AbilityCost = 40f;
    private StaminaController StaminaController;

    private void Awake()
    {
        StaminaController = GetComponent<StaminaController>();
    }

    // + + + + | Overrides | + + + +

    /// <summary>
    /// Causes the character to dash or dive (depending on the vertical movement at the start of the dash), costing Stamina
    /// </summary>
    public override void StartDash()
    {
        if (!DashAuthorized())
        {
            return;
        }

        if (!DashConditions())
        {
            return;
        }

        if (!StaminaController.CanRemoveStaminaAmount(AbilityCost))
        {
            return;
        }

        StaminaController.RemoveStaminaAmount(AbilityCost);
        InitiateDash();
    }
}
