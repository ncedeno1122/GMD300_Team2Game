using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandleWeaponStamina : CharacterHandleWeapon
{
    [Header("Stamina")]
    public float AbilityCost = 30f;
    private StaminaController StaminaController;

    private void Awake()
    {
        StaminaController = GetComponent<StaminaController>();
    }

    // + + + + | Overrides | + + + +

    /// <summary>
    /// Causes the character to start shooting, costing stamina.
    /// </summary>
    public override void ShootStart()
    {
        // if the Shoot action is enabled in the permissions, we continue, if not we do nothing.  If the player is dead we do nothing.
        if (!AbilityAuthorized
             || (CurrentWeapon == null)
             || ((_condition.CurrentState != CharacterStates.CharacterConditions.Normal) && (_condition.CurrentState != CharacterStates.CharacterConditions.ControlledMovement)))
        {
            return;
        }

        if (!CanShootFromLadders && (_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing))
        {
            return;
        }

        if (!StaminaController.CanRemoveStaminaAmount(AbilityCost))
        {
            return;
        }

        //  if we've decided to buffer input, and if the weapon is in use right now
        if (BufferInput && (CurrentWeapon.WeaponState.CurrentState != Weapon.WeaponStates.WeaponIdle))
        {
            // if we're not already buffering, or if each new input extends the buffer, we turn our buffering state to true
            if (!_buffering || NewInputExtendsBuffer)
            {
                _buffering = true;
                _bufferEndsAt = Time.time + MaximumBufferDuration;
            }
        }

        PlayAbilityStartFeedbacks();
        MMCharacterEvent.Trigger(_character, MMCharacterEventTypes.HandleWeapon, MMCharacterEvent.Moments.Start);
        StaminaController.RemoveStaminaAmount(AbilityCost);
        CurrentWeapon.WeaponInputStart();
    }
}
