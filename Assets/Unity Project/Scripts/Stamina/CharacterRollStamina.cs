using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRollStamina : CharacterRoll
{
    [Header("Stamina")]
    public float AbilityCost = 35f;
    private StaminaController StaminaController;

    private void Awake()
    {
        StaminaController = GetComponent<StaminaController>();
    }

    // + + + + | Overrides | + + + +

    public override void StartRoll()
    {
        if (!RollAuthorized())
        {
            return;
        }

        if (!RollConditions())
        {
            return;
        }

        if (!StaminaController.CanRemoveStaminaAmount(AbilityCost))
        {
            return;
        }

        InitiateRoll();
        StaminaController.RemoveStaminaAmount(AbilityCost);
        Debug.Log("Roll started!");
    }
}
