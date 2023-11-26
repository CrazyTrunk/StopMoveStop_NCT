using System;
using UnityEngine;

public class Character : MonoBehaviour, ICombatant
{
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private float animSpeed = 1.5f;
    [SerializeField] private float animPlayTime = 1f;
    protected string CurrentAnim;

    [Header("Combat")]
    [SerializeField] private ThrowWeapon throwWeapon;
    [SerializeField] public HandWeapon Weapon;
    [SerializeField] private float speed;
    [SerializeField] private float range;

    [Header("Sphere Detection")]
    [SerializeField] private DetectedCircle detectedCircle;
    [SerializeField] private RadicalTrigger radicalTrigger;
    [SerializeField] private CharacterSphere characterSphere;
    protected CapsuleCollider capsuleCollider;
    public const float RangeIncreasePerTenLevels = 2f;
    private float baseRange = 5f;
    private float maxRangeIncrese = 10f;

    [Header("UI Canvas Info and noti")]
    [SerializeField] private LevelDisplay levelDisplayInfo;


    [Header("Basic CharacterInfo")]
    [SerializeField] private Transform characterModel;
    public int Level = 0;
    public const int MaxLevelIncreseGap = 3;
    public const int MaxLevel = 55;
    public Vector3 baseScale = new Vector3(1, 1, 1);
    public float maxScale = 2f;
    public float scaleIncrement = 0.1f;
    private Vector3 originalColliderSize;
    private Vector3 originalColliderCenter;


    [Header("Boolean")]
    private bool isMoving;
    private bool hasEnemyInSight;
    private bool isAttacking;


    public Animator Animator { get => animator; set => animator = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public float Speed { get => speed; set => speed = value; }
    public bool HasEnemyInSight { get => hasEnemyInSight; set => hasEnemyInSight = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsDead { get; set; }
    public float AnimSpeed { get => animSpeed; set => animSpeed = value; }
    public float AnimPlayTime { get => animPlayTime; set => animPlayTime = value; }
    public ICombatant LastAttacker { get; set; }
    //Event
    public event Action<ICombatant> OnCombatantKilled;
    public event Action OnLevelUp;

    private void Awake()
    {
        Animator.speed = animSpeed;
        capsuleCollider = GetComponent<CapsuleCollider>();
        originalColliderSize = new Vector3(capsuleCollider.radius, capsuleCollider.height, capsuleCollider.radius);
        originalColliderCenter = capsuleCollider.center;
    }
    public void ChangeAnim(string animName)
    {
        if (CurrentAnim != animName)
        {
            Animator.ResetTrigger(animName);
            CurrentAnim = animName;
            Animator.SetTrigger(CurrentAnim);
        }
    }
    public void LookAtTarget(Transform target)
    {
        transform.LookAt(target.position);
    }
    public void ThrowWeapon()
    {
        float scaleMultiplier = 1 + (Level / (float)MaxLevel * (maxScale - 1));;

        throwWeapon.Throw(range, this, scaleMultiplier);
    }
    public void HideWeaponOnHand()
    {
        Weapon.HideWeapon();
    }
    public void ShowWeaponOnHand()
    {
        Weapon.ShowWeapon();
    }

    public void Detect()
    {
        detectedCircle?.Show();
    }

    public void Undetect()
    {
        detectedCircle?.Hide();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void ResetState()
    {
        Level = 0;
        IsDead = false;
        isMoving = false;
        isAttacking = false;
        hasEnemyInSight = false;
        capsuleCollider.enabled = true;
        radicalTrigger.OnInit();
    }
    public void InitLevelBot(int level)
    {
        Level = level;
        float currentRange = CalculateRange();
        ScaleModel(level);
        AdjustCollider();
        characterSphere.UpdateTriggerSize(currentRange);
        levelDisplayInfo.UpdateUILevelPlayer(Level);
    }
    public void LevelUp(int enemyLevel)
    {
        int level = CalculateLevel(enemyLevel);
        float currentRange = CalculateRange();
        ScaleModel(level);
        AdjustCollider();
        characterSphere.UpdateTriggerSize(currentRange);
        levelDisplayInfo.UpdateUILevelPlayer(Level);
    }
    private int CalculateLevel(int enemyLevel)
    {
        int levelIncrease = Mathf.Max(1, Mathf.Min(enemyLevel - Level, MaxLevelIncreseGap));
        Level += levelIncrease;
        if (this is Player player)
        {
            //trigger Text Ui
            Debug.Log("Level Up");
            player.ShowFloatingText(levelIncrease);
        }
        Level = Mathf.Clamp(Level, 0, MaxLevel);
        OnLevelUp?.Invoke();
        return Level;
    }
    private float CalculateRange()
    {
        float increment = (Level / 10) * RangeIncreasePerTenLevels;
        return range = Mathf.Min(baseRange + increment, maxRangeIncrese);

    }
    private void ScaleModel(int level)
    {
        float scaleMultiplier = 1 + (level / (float)MaxLevel * (maxScale - 1));
        characterModel.localScale = baseScale * scaleMultiplier;
    }
    private void AdjustCollider()
    {
        float scale = characterModel.localScale.y;

        // Điều chỉnh collider dựa trên tỉ lệ scale
        capsuleCollider.radius = originalColliderSize.x * scale;
        capsuleCollider.height = originalColliderSize.y * scale;

        // Điều chỉnh center của collider
        // Bạn có thể cần điều chỉnh giá trị này dựa trên vị trí cụ thể của model của bạn
        capsuleCollider.center = new Vector3(originalColliderCenter.x, originalColliderCenter.y * scale, originalColliderCenter.z);
    }
    protected void InvokeOnDeath()
    {
        OnCombatantKilled?.Invoke(this);
    }
}
