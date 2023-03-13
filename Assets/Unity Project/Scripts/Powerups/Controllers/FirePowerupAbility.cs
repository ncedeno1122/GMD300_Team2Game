using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// TODO_DESCRIPTION
/// </summary>
[AddComponentMenu("Corgi Engine/Character/Abilities/FirePowerupAbility")]
public class FirePowerupAbility : CharacterAbility
{
    /// This method is only used to display a helpbox text
    /// at the beginning of the ability's inspector
    public override string HelpBoxText() { return "The Fire Powerup."; }

    private bool _IsAbilityInProgress = false;

    [Header("Animation Parameters")]
    protected const string _todoParameterName = "TODO";
    protected int _todoAnimationParameter;

    public float Anim_InitialDelay = 0.15f; // The time to start the ability
    public float Anim_CastTime = 1f; // The time that the ability should last for
    public float Anim_RecoveryTime = 0.35f; // The time that the player returns from the animation

    [Header("Hitbox Parameters")]
    public Vector2 HitBoxDimensions_Start = new Vector2(0.5f, 0.5f);
    public Vector2 HitboxDimensions_End = new Vector2(5f, 3f);

    /// the layers that will be damaged by this object
    [Tooltip("the layers that will be damaged by this object")]
    public LayerMask TargetLayerMask;

    // DamageArea
    [SerializeField, SerializeReference]
    private GameObject _damageArea;
    private BoxCollider2D _boxCollider2D;
    private DamageOnTouch _damageOnTouch;
    private Collider2D _damageAreaCollider;

    private bool _hitEventSent = false;
    private bool _hitDamageableEventSent = false;
    private bool _hitNonDamageableEventSent = false;
    private bool _killEventSent = false;
    private bool _eventsRegistered = false;
    private Coroutine _meleeWeaponAttack;

    public TypedDamage FireDamageReference; // TODO: This feels ugly...

    [SerializeField]
    private float _minDamageCaused = 25f;
    public float MinDamageCaused { get => _minDamageCaused; }
    [SerializeField]
    private float _maxDamageCaused = 25f;
    public float MaxDamageCaused { get => _maxDamageCaused; }

    // Particles
    public ParticleSystem Particle;

    /// <summary>
    /// Here you should initialize our parameters
    /// </summary>
    protected override void Initialization()
    {
        base.Initialization();

        // Create DamageArea
        if (_damageArea == null)
        {
            CreateDamageArea();
            DisableDamageArea();
        }

        // TODO: Set Owner like in MeleeWeapon?
    }

    /// <summary>
    /// Every frame, we check if we're crouched and if we still should be
    /// </summary>
    public override void ProcessAbility()
    {
        base.ProcessAbility();
    }

    /// <summary>
    /// Called at the start of the ability's cycle, this is where you'll check for input
    /// </summary>
    protected override void HandleInput()
    {
        // here as an example we check if we're pressing down
        // on our main stick/direction pad/keyboard
        if (_inputManager.PrimaryMovement.y < -_inputManager.Threshold.y)
        {
            DoSomething();   
        }
    }

    /// <summary>
    /// If we're pressing down, we check for a few conditions to see if we can perform our action
    /// </summary>
    protected virtual void DoSomething()
    {
        // if the ability is not permitted
        if (!AbilityPermitted
            // or if we're not in our normal stance
            || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal)
            // or if we're grounded
            || (!_controller.State.IsGrounded)
            // or if we're gripping
            || (_movement.CurrentState == CharacterStates.MovementStates.Gripping))
        {
            // we do nothing and exit
            return;
        }

        // if we're still here, we display a text log in the console
        MMDebug.DebugLogTime("Starting FirePowerupAbility!");
        HandleStartAbility();
    }

    /// <summary>
    /// Adds required animator parameters to the animator parameters list if they exist
    /// </summary>
    protected override void InitializeAnimatorParameters()
    {
        RegisterAnimatorParameter(_todoParameterName, AnimatorControllerParameterType.Bool, out _todoAnimationParameter);
    }

    /// <summary>
    /// At the end of the ability's cycle,
    /// we send our current crouching and crawling states to the animator
    /// </summary>
    public override void UpdateAnimator()
    {
        MMAnimatorExtensions.UpdateAnimatorBool(_animator, _todoAnimationParameter, (_movement.CurrentState == CharacterStates.MovementStates.Crouching), _character._animatorParameters);
    }

    /// <summary>
    /// Creates the damage area.
    /// </summary>
    protected virtual void CreateDamageArea()
    {
        //MMDebug.DebugLogTime("Creating Damage Area!");

        // TODO: Cache this somewhere? Could be expensive... Hmm.
        _damageArea = new GameObject();
        _damageArea.name = this.name + "DamageArea";
        _damageArea.transform.position = this.transform.position;
        _damageArea.transform.rotation = this.transform.rotation;
        _damageArea.transform.SetParent(this.transform);

        // Box Collider 2D
        _boxCollider2D = _damageArea.AddComponent<BoxCollider2D>();
        _boxCollider2D.offset = Vector2.zero;
        _boxCollider2D.size = HitBoxDimensions_Start;
        _damageAreaCollider = _boxCollider2D;
        _damageAreaCollider.isTrigger = true;

        // Add Rigidbody2D
        Rigidbody2D rigidBody = _damageArea.AddComponent<Rigidbody2D>();
        rigidBody.isKinematic = true;

        // Add DamageOnTouch
        _damageOnTouch = _damageArea.AddComponent<DamageOnTouch>();
        _damageOnTouch.TargetLayerMask = TargetLayerMask;
        _damageOnTouch.MinDamageCaused = MinDamageCaused;
        _maxDamageCaused = (_maxDamageCaused <= _minDamageCaused) ? _minDamageCaused : _maxDamageCaused;
        _damageOnTouch.MaxDamageCaused = MaxDamageCaused;
        _damageOnTouch.DamageCausedKnockbackType = DamageOnTouch.KnockbackStyles.NoKnockback;
        _damageOnTouch.DamageCausedKnockbackForce = Vector2.zero;
        _damageOnTouch.InvincibilityDuration = 0.1f;
        _damageOnTouch.TypedDamages = new List<TypedDamage>()
        {
            new TypedDamage() // Add Fire Typed Damage
            {
                AssociatedDamageType = FireDamageReference.AssociatedDamageType,
                MinDamageCaused = 25f,
                MaxDamageCaused = 25f
            }
        };  

    }

    // + + + + | Functions | + + + + 

    public void HandleStartAbility()
    {
        if (_IsAbilityInProgress) return;
        StartCoroutine(PerformAbility());
    }

    protected IEnumerator PerformAbility()
    {
        _IsAbilityInProgress = true;


        _character.MovementState.ChangeState(CharacterStates.MovementStates.Idle);
        _character.ConditionState.ChangeState(CharacterStates.CharacterConditions.ControlledMovement);

       // Halt the character
        CharacterHorizontalMovement chm = _character.FindAbility<CharacterHorizontalMovement>();
        if (chm)
        {
            chm.ReadInput = false;
            chm.ResetHorizontalSpeed();
            chm.MovementForbidden = true;
            chm.WalkSpeed = 0f;
        }
        else
        {
            MMDebug.DebugLogTime($"{GetType().Name} - Couldn't find CharacterHorizontalMovement?");
        }

        // Wait for initial delay
        yield return new WaitForSeconds(Anim_InitialDelay);
        EnableDamageArea();
        Particle.Play();

        // Work With hitboxes
        float deltaTimeHelper = 0f;
        while(deltaTimeHelper < Anim_CastTime)
        {
            deltaTimeHelper += Time.deltaTime;

            // Grow the DamageArea 
            _boxCollider2D.size = Vector2.Lerp(HitBoxDimensions_Start, HitboxDimensions_End, deltaTimeHelper / Anim_CastTime);

            yield return new WaitForEndOfFrame();
        }

        DisableDamageArea();

        // Wait for recovery time
        yield return new WaitForSeconds(Anim_RecoveryTime);
        _IsAbilityInProgress = false;
        _character.MovementState.ChangeState(CharacterStates.MovementStates.Idle);
        _character.ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);

        if (chm)
        {
            chm.ReadInput = true;
            chm.ResetHorizontalSpeed();
            chm.MovementForbidden = false;
            chm.WalkSpeed = 6f;
        }
    }

    /// <summary>
    /// Enables the damage area.
    /// </summary>
    protected virtual void EnableDamageArea()
    {
        _hitEventSent = false;
        _hitDamageableEventSent = false;
        _hitNonDamageableEventSent = false;
        _killEventSent = false;
        _damageAreaCollider.enabled = true;
    }

    /// <summary>
    /// Disables the damage area.
    /// </summary>
    protected virtual void DisableDamageArea()
    {
        _damageAreaCollider.enabled = false;
    }
}
