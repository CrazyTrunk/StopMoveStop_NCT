using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour, ICombatant
{
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private float animSpeed = 1.5f;
    [SerializeField] private float animPlayTime = 1f;
    protected string CurrentAnim;

    [Header("Combat")]
    [SerializeField] private Transform spawnBulletPoint;
    [SerializeField] private Transform spawnWeaponPoint;

    private Weapon weapon;
    private GameObject weaponPrefab;
    [SerializeField] private float speed;
    [SerializeField] private float range;

    [Header("Sphere Detection")]
    [SerializeField] private DetectedCircle detectedCircle;
    [SerializeField] private RadicalTrigger radicalTrigger;
    [SerializeField] private CharacterInfo characterInfo;
    [SerializeField] private CharacterSphere characterSphere;
    [SerializeField] private CapsuleCollider capsuleColliderCharacter;
    public const float RangeIncreasePerTenLevels = 2f;
    private float baseRange = 5f;
    private float baseSpeed = 5f;
    [SerializeField] private float maxSpeed = 7f;
    private readonly float maxRangeIncrese = 10f;

    [Header("UI Canvas Info and noti")]
    [SerializeField] private LevelDisplay levelDisplayInfo;


    [Header("Basic CharacterInfo")]
    [SerializeField] private Transform characterModel;
    public int level = 0;
    private int previousLevel = 0;
    public const int MaxLevelIncreseGap = 3;
    public const int MaxLevel = 55;
    public Vector3 baseScale = new(1f, 1f, 1f);
    public float maxScale = 2f;
    public float scaleIncrement = 0.1f;
    private float scaleMultiple = 1f;
    private Vector3 originalColliderSize;
    private Vector3 originalColliderCenter;

    [Header("Boolean")]
    private bool isMoving;
    private bool hasEnemyInSight;
    private bool isAttacking;
    private bool isDead;

    [SerializeField] private WeaponData weaponDataSO;


    public Animator Animator { get => animator; set => animator = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public float Speed { get => speed; set => speed = value; }
    public bool HasEnemyInSight { get => hasEnemyInSight; set => hasEnemyInSight = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public float AnimSpeed { get => animSpeed; set => animSpeed = value; }
    public float AnimPlayTime { get => animPlayTime; set => animPlayTime = value; }
    public Weapon Weapon { get => weapon; set => weapon = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public float Range { get => range; set => range = value; }
    public float ScaleMultiple { get => scaleMultiple; set => scaleMultiple = value; }
    public Transform CharacterModel { get => characterModel; set => characterModel = value; }
    public float BaseSpeed { get => baseSpeed; set => baseSpeed = value; }
    public float BaseRange { get => baseRange; set => baseRange = value; }
    public CharacterSphere CharacterSphere { get => characterSphere; set => characterSphere = value; }
    public WeaponData WeaponDataSO { get => weaponDataSO; set => weaponDataSO = value; }

    public event Action<ICombatant> OnCombatantKilled;
    public void OnInit()
    {
        Animator.speed = animSpeed;
        originalColliderSize = new Vector3(capsuleColliderCharacter.radius, capsuleColliderCharacter.height, capsuleColliderCharacter.radius);
        originalColliderCenter = capsuleColliderCharacter.center;
        ResetState();
        if (this is Enemy enemy)
        {
            ChangeWeapon(WeaponType.HAMMER);
        }
        else if (this is Player player)
        {
            ChangeWeapon(GameManager.Instance.GetPlayerData().equippedWeapon);
        }
        EquipWeapon(Weapon);
        CharacterSphere.UpdateTriggerSize(this.range);
    }
    private void OnEnable()
    {

    }
    private void HandleStateChange(GameState state)
    {
        if (GameManager.Instance.IsState(GameState.Playing))
        {
            characterInfo.gameObject.SetActive(true);
        }
        else
        {
            characterInfo.gameObject.SetActive(false);
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        ApplyWeaponBonuses(weapon.bonusSpeed, weapon.bonusRange);
    }
    public void ApplyWeaponBonuses(float bonusSpeed, float bonusRange)
    {
        this.Speed = BaseSpeed;
        this.Range = BaseRange;

        this.Speed += bonusSpeed / 10;
        this.Range += bonusRange / 10;
    }
    public void ChangeAnim(string animName)
    {
        if (!string.IsNullOrEmpty(animName) && CurrentAnim != animName)
        {
            if (!string.IsNullOrEmpty(CurrentAnim))
                Animator.ResetTrigger(CurrentAnim);
            CurrentAnim = animName;
            Animator.SetTrigger(CurrentAnim);
        }
    }
    public void ChangeWeapon(WeaponType weaponType)
    {
        if (weaponPrefab != null)
        {
            Destroy(weaponPrefab);
        }
        weaponPrefab = Instantiate(WeaponDataSO.GetWeaponByType(weaponType).gameObject, spawnWeaponPoint);
        weapon = weaponPrefab.GetComponent<Weapon>();
    }
    public void ChangeWeapon(Weapon currentWeapon)
    {
        if (weaponPrefab != null)
        {
            Destroy(weaponPrefab);
        }
        weaponPrefab = Instantiate(currentWeapon.gameObject, spawnWeaponPoint);
        weapon = currentWeapon;
    }
    public void LookAtTarget(Transform target)
    {
        transform.LookAt(target.position);
    }
    #region Weapon - Bullet
    public void ThrowWeapon()
    {
        weapon.ThrowWeapon(spawnBulletPoint, this, OnHitVictim);
    }
    public void HideWeaponOnHand()
    {
        weapon.HideWeapon();
    }
    public void ShowWeaponOnHand()
    {
        weapon.ShowWeapon();
    }
    #endregion
    #region Circle UnderFeet (interface)
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
    #endregion


    public void ResetState()
    {
        level = 0;
        isDead = false;
        isMoving = false;
        isAttacking = false;
        hasEnemyInSight = false;
        capsuleColliderCharacter.enabled = true;
        radicalTrigger.OnInit();
        Undetect();
    }
    #region Level Init and other calculate related
    public void InitLevelBot(int level)
    {
        this.level = level;
        float currentRange = CalculateRange();
        ScaleModel(level);
        AdjustCollider();
        CharacterSphere.UpdateTriggerSize(currentRange);
        levelDisplayInfo.UpdateUILevelPlayer(this.level);
        Speed = Mathf.Min(Speed + (level * 0.1f), maxSpeed);
    }
    public void LevelUp(int enemyLevel)
    {
        int level = CalculateLevel(enemyLevel);
        float currentRange = CalculateRange();
        ScaleModel(level);
        if (this is Player player)
        {
            if (level != 0 && level >= previousLevel + 10 && level < MaxLevel)
            {
                Camera.main.GetComponent<CameraFollow>().UpdateCameraHeight();
                previousLevel = level;
            }
        }
        AdjustCollider();
        CharacterSphere.UpdateTriggerSize(currentRange);
        levelDisplayInfo.UpdateUILevelPlayer(this.level);
        Speed = Mathf.Min(Speed + 0.1f, maxSpeed);
    }

    private int CalculateLevel(int enemyLevel)
    {
        int levelIncrease = Mathf.Max(1, Mathf.Min(enemyLevel - level, MaxLevelIncreseGap));
        level += levelIncrease;
        if (this is Player player)
        {
            player.GainCoin(levelIncrease);
            player.ShowFloatingText(levelIncrease);
        }
        level = Mathf.Clamp(level, 0, MaxLevel);
        return level;
    }
    private float CalculateRange()
    {
        float increment = (level / 10) * RangeIncreasePerTenLevels;
        Range = Mathf.Min(Range + increment, maxRangeIncrese);
        return Range;
    }
    private void ScaleModel(int levelReceive)
    {
        ScaleMultiple = 1 + (levelReceive / (float)MaxLevel * (maxScale - 1));
        CharacterModel.localScale = baseScale * ScaleMultiple;
    }
    private void AdjustCollider()
    {
        float scale = CharacterModel.localScale.y;
        //chat GPT
        // Điều chỉnh collider dựa trên tỉ lệ scale
        capsuleColliderCharacter.radius = originalColliderSize.x * scale;
        capsuleColliderCharacter.height = originalColliderSize.y * scale;

        // Điều chỉnh center của collider
        // Bạn có thể cần điều chỉnh giá trị này dựa trên vị trí cụ thể của model của bạn
        capsuleColliderCharacter.center = new Vector3(originalColliderCenter.x, originalColliderCenter.y * scale, originalColliderCenter.z);
    }
    #endregion
    #region Event
    protected virtual void OnHitVictim(Character attacker, Character victim)
    {
        victim.PlayDead();
        if (victim is Player player)
        {
            LoseMenu.Show();
            LoseMenu.Instance.OnInit(LevelManager.Instance.TotalBotsToKill, attacker.name, player.CoinGained);
            GameManager.Instance.ChangeState(GameState.GameOver);
            GameManager.Instance.UpdatePlayerData(player.PlayerData);
            GameManager.Instance.SaveToJson(player.PlayerData, FilePathGame.CHARACTER_PATH);

        }
        attacker.LevelUp(victim.level);
    }

    private void PlayDead()
    {
        isDead = true;
        OnCombatantKilled?.Invoke(this);
        StartCoroutine(RespawnCoroutine());
    }
    private IEnumerator RespawnCoroutine()
    {
        ChangeAnim(Anim.DIE);
        // Đợi animation "die" chạy xong
        yield return new WaitForSeconds(AnimPlayTime / AnimSpeed);

        // Ẩn nhân vật (hoặc làm nhân vật không hoạt động) khi nó chết
        yield return new WaitForSeconds(1f);
        if (GameManager.Instance.IsState(GameState.Playing))
        {
            LevelManager.Instance.BotKilled(this);
        }
    }
    #endregion

}
