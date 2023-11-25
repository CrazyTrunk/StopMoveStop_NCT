using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Character : MonoBehaviour, ICombatant
{
    [SerializeField] private Animator animator;
    [SerializeField] private ThrowWeapon throwWeapon;
    [SerializeField] public HandWeapon Weapon;
    [SerializeField] private float speed;
    [SerializeField] private DetectedCircle detectedCircle;
    [SerializeField] private RadicalTrigger radicalTrigger;
    [SerializeField] private PlayerSphere playerSphere;
    [SerializeField] private float animSpeed = 1.5f;
    [SerializeField] private float animPlayTime = 1f;
    CapsuleCollider capsuleCollider;
    [Header("Scale")]
    [SerializeField] private Transform characterModel;
    public int Level = 0;
    public const int MaxLevelIncreseGap = 3;
    public const int MaxLevel = 55;
    public Vector3 baseScale = new Vector3(1, 1, 1);
    public float maxScale = 2f;
    public float scaleIncrement = 0.1f;
    private float respawnTime = 1f;
    private Vector3 originalColliderSize;
    private Vector3 originalColliderCenter;


    [Header("Scale Sphere")]
    public const float LevelIncreasePerTenLevels = 2f;
    [SerializeField] private float range;
    private float baseRange = 5f;
    private float maxRangeIncrese = 10f;

    protected string CurrentAnim;
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
        throwWeapon.Throw(range, this);
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
        if (detectedCircle != null)
            detectedCircle.Show();
    }

    public void Undetect()
    {
        if (detectedCircle != null)
            detectedCircle.Hide();
    }

    public Transform GetTransform()
    {
        return transform;
    }
    public virtual void OnDeath()
    {
        IsDead = true;
        isMoving = false;
        isAttacking = false;
        hasEnemyInSight = false;
        capsuleCollider.enabled = false;
        Undetect();
        ChangeAnim(Anim.DIE);
        OnCombatantKilled?.Invoke(this);

        StartCoroutine(RespawnCoroutine());

    }
    private IEnumerator RespawnCoroutine()
    {

        // Đợi animation "die" chạy xong
        yield return new WaitForSeconds(AnimPlayTime / animSpeed);

        // Ẩn nhân vật (hoặc làm nhân vật không hoạt động) khi nó chết
        yield return new WaitForSeconds(respawnTime);
        LevelManager.Instance.BotKilled(this);

    }

    public void ResetState()
    {
        IsDead = false;
        isMoving = false;
        isAttacking = false;
        hasEnemyInSight = false;
        capsuleCollider.enabled = true;
        radicalTrigger.OnInit();
    }
    public void LevelUp(int enemyLevel)
    {
        int level = CalculateLevel(enemyLevel);
        float currentRange = CalculateRange();
        ScaleModel(level);
        AdjustCollider();
        playerSphere.UpdateTriggerSize(currentRange);
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
        float increment = (Level / 10) * LevelIncreasePerTenLevels;
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
}
