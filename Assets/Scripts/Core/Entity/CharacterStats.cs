using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CharacterStats  : MonoBehaviour
{

    #region 初始属性
    [Header("初始属性")]
    //最大生命值
    [SerializeField]
    private float originMaxHealth = 100;
    public float OriginMaxHealth
    {
        get { return originMaxHealth; }
        set
        {
            originMaxHealth = value;
            ReCalculatePanelAttribute();
        }
    }
    //生命值
    [SerializeField]
    private float originHealth = 100;
    public float OriginHealth
    {
        get => originHealth;
        set
        {
            originHealth = Mathf.Max(value, 0);
            ReCalculatePanelAttribute();
        }
    }
    //移动速度
    [SerializeField]
    private float originMoveSpeed = 6;
    public float OriginMoveSpeed
    {
        get { return originMoveSpeed; }
        set
        {
            originMoveSpeed = value;
            ReCalculatePanelAttribute();
        }
    }
    //攻击速度
    [SerializeField]
    private float originAttackSpeed;
    public float OriginAttackSpeed
    {
        get { return originAttackSpeed; }
        set
        {
            originAttackSpeed = value;
            ReCalculatePanelAttribute();
        }
    }
    //攻击力
    [SerializeField]
    private float originAttackPower;
    public float OriginAttackPower
    {
        get { return originAttackPower; }
        set
        {
            originAttackPower = value;
            ReCalculatePanelAttribute();
        }
    }
    //防御力
    [SerializeField]
    private float originDefensePower;
    public float OriginDefensePower
    {
        get => originDefensePower;
        set
        {
            originDefensePower = value;
            ReCalculatePanelAttribute() ;
        }
    }
    //暴击率
    [SerializeField]
    private float originCritRate;
    public float OriginCritRate
    {
        get => originCritRate;
        set
        {
            originCritRate = value;
            ReCalculatePanelAttribute();
        }
    }
    //暴击伤害
    [SerializeField]
    private float originCritMultiplier;
    public float OriginCritMultiplier
    {
        get => originCritMultiplier;
        set
        {
            originCritMultiplier = value;
            ReCalculatePanelAttribute();
        }
    }
    #endregion

    #region 属性加成
    [Header("增量")]
    //最大生命值 固定值加成
    private RequestValue<float> maxHealthModifierFixed = new RequestValue<float>((a, b) => a + b, 0);
    //最大生命值 百分比加成
    private RequestValue<float> maxHealthModifierPercentage = new RequestValue<float>((a, b) => a + b, 0);
    //移动速度 固定值加成
    private RequestValue<float> moveSpeedModifierFixed = new RequestValue<float>((a, b) => a + b, 0);
    //移动速度 百分比加成
    private RequestValue<float> moveSpeedModifierPercentage = new RequestValue<float>((a, b) => a + b, 0);
    //攻击速度 固定值加成
    private RequestValue<float> attackSpeedModifierFixed = new RequestValue<float>((a, b) => a + b, 0);
    //攻击速度 百分比加成
    private RequestValue<float> attackSpeedModifierPercentage = new RequestValue<float>((a, b) => a + b, 0);
    //攻击力 固定值加成
    private RequestValue<float> attackPowerModifierFixed = new RequestValue<float>((a, b) => a + b, 0);
    //攻击力 百分比加成
    private RequestValue<float> attackPowerModifierPercentage = new RequestValue<float>((a, b) => a + b, 0);
    //防御力 固定值加成
    private RequestValue<float> defensePowerModifierFixed = new RequestValue<float>((a, b) => a + b, 0);
    //防御力 百分比加成
    private RequestValue<float> defensePowerModifierPercentage = new RequestValue<float>((a, b) => a + b, 0);
    //暴击率 固定值加成
    private RequestValue<float> critRateModifierFixed = new RequestValue<float>((a, b) => a + b, 0);
    //暴击率 百分比加成
    private RequestValue<float> critRateModifierPercentage = new RequestValue<float>((a, b) => a + b, 0);
    //暴击伤害 固定值加成
    private RequestValue<float> critMultiplierModifierFixed = new RequestValue<float>((a, b) => a + b, 0);
    //暴击伤害 百分比加成
    private RequestValue<float> critMultiplierModifierPercentage = new RequestValue<float>((a, b) => a + b, 0);
    #endregion

    #region 面板属性
    [Header("面板属性")]
    //头像
    [SerializeField]
    private Texture2D characterPortrait;
    public Texture2D CharacterPortrait
    {
        get
        {
            return characterPortrait;
        }
        set
        {
            characterPortrait = value;
            isDirty = true;
        }
    }
    //最大生命值
    [SerializeField]
    private float maxHealth;
    public float MaxHealth
    {
        get { return maxHealth; }
        private set
        {
            maxHealth = Mathf.Max(0, value);
            isDirty = true;
        }
    }
    //生命值
    [SerializeField]
    private float health;
    public float Health
    {
        get => health;
        private set
        {
            health = Mathf.Max(value, 0);
            isDirty = true;
        }
    }
    //移动速度
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        private set
        {
            moveSpeed = Mathf.Max(0, value);
            isDirty = true;
        }
    }
    //攻击速度
    [SerializeField]
    private float attackSpeed;
    public float AttackSpeed
    {
        get { return attackSpeed; }
        private set
        {
            attackSpeed = Mathf.Max(0, value);
            isDirty = true;
        }
    }
    //攻击力
    [SerializeField]
    private float attackPower;
    public float AttackPower
    {
        get { return attackPower; }
        private set
        {
            attackPower = Mathf.Max(0, value);
            isDirty = true;
        }
    }
    //防御力
    [SerializeField]
    private float defensePower;
    public float DefensePower
    {
        get => defensePower;
        private set
        {
            defensePower = Mathf.Max(0, value);
            isDirty = true;
        }
    }
    //暴击率
    [SerializeField]
    private float critRate;
    public float CritRate
    {
        get => critRate;
        private set
        {
            critRate = Mathf.Max(0, value);
            isDirty = true;
        }
    }
    //暴击伤害
    [SerializeField]
    private float critMultiplier;
    public float CritMultiplier
    {
        get => critMultiplier;
        private set
        {
            critMultiplier = Mathf.Max(0, value);
            isDirty = true;
        }
    }
    #endregion

    #region 生命周期
    private bool isDirty = true;
    public UnityEvent<CharacterStats> updateEvent = new();
    public UnityEvent<CharacterStats> enableEvent = new();
    public UnityEvent<CharacterStats> disableEvent = new();

    private void Awake()
    {
        fixedModifiers = new Dictionary<ModifiableAttributeType, RequestValue<float>>
        {
            {ModifiableAttributeType.MaxHealth, maxHealthModifierFixed },
            {ModifiableAttributeType.MoveSpeed, moveSpeedModifierFixed },
            {ModifiableAttributeType.AttackSpeed, attackSpeedModifierFixed },
            {ModifiableAttributeType.AttackPower, attackPowerModifierFixed },
            {ModifiableAttributeType.DefensePower, defensePowerModifierFixed },
            {ModifiableAttributeType.CritRate, critRateModifierFixed },
            {ModifiableAttributeType.CritMultiplier, critMultiplierModifierFixed }
        };
        percentageModifiers = new Dictionary<ModifiableAttributeType, RequestValue<float>>
        {
            {ModifiableAttributeType.MaxHealth, maxHealthModifierPercentage },
            {ModifiableAttributeType.MoveSpeed, moveSpeedModifierPercentage },
            {ModifiableAttributeType.AttackSpeed, attackSpeedModifierPercentage },
            {ModifiableAttributeType.AttackPower, attackPowerModifierPercentage },
            {ModifiableAttributeType.DefensePower, defensePowerModifierPercentage },
            {ModifiableAttributeType.CritRate, critRateModifierPercentage },
            {ModifiableAttributeType.CritMultiplier, critMultiplierModifierPercentage }
        };   
    }

    private void OnEnable()
    {
        enableEvent.Invoke(this);
        ReCalculatePanelAttribute();
    }

    private void OnDisable()
    {
        disableEvent.Invoke(this);
    }

    private void Update()
    {
        if (isDirty)
        {
            updateEvent.Invoke(this);
            isDirty = false;
        }
    }
    #endregion



    #region 外部修改属性的方法
    /// <summary>
    /// 属性类型
    /// </summary>
    public enum ModifiableAttributeType
    {
        MaxHealth,
        MoveSpeed,
        AttackSpeed,
        AttackPower,
        DefensePower,
        CritRate,
        CritMultiplier,
    }
    private Dictionary<ModifiableAttributeType, RequestValue<float>> fixedModifiers;
    private Dictionary<ModifiableAttributeType, RequestValue<float>> percentageModifiers;
    /// <summary>
    /// 从外部修改某个属性
    /// </summary>
    public void ModifyAttribute(ModifiableAttributeType attributeType, bool fixedOrPercentage, float deltaValue, bool addOrRemove)
    {
        RequestValue<float> modifier;
        if (fixedOrPercentage)
        {
            modifier = fixedModifiers[attributeType];
        }
        else
        {
            modifier = percentageModifiers[attributeType];
        }

        if (addOrRemove)
        {
            modifier.AddRequest(deltaValue);
        }
        else
        {
            modifier.RemoveRequest(deltaValue);
        }


        ReCalculatePanelAttribute();
    }

    #endregion


    //重新计算面板属性
    private void ReCalculatePanelAttribute()
    {
        //最大生命值和当前生命值
        float damageHealth = MaxHealth - Health;
        MaxHealth = OriginMaxHealth * Mathf.Max(0, (1 + maxHealthModifierPercentage.GetResult())) + maxHealthModifierFixed.GetResult();
        Health = MaxHealth - damageHealth;

        MoveSpeed = OriginMoveSpeed * Mathf.Max(0, (1 + moveSpeedModifierPercentage.GetResult())) + moveSpeedModifierFixed.GetResult();
        AttackSpeed = OriginAttackSpeed * Mathf.Max(0, (1 + attackSpeedModifierPercentage.GetResult())) + attackSpeedModifierFixed.GetResult();
        AttackPower = OriginAttackPower * Mathf.Max(0, (1 + attackPowerModifierPercentage.GetResult())) + attackPowerModifierFixed.GetResult();
        DefensePower = OriginDefensePower * Mathf.Max(0, (1 + defensePowerModifierPercentage.GetResult())) + defensePowerModifierFixed.GetResult();
        CritRate = OriginCritRate * Mathf.Max(0, (1 + critRateModifierPercentage.GetResult())) + critRateModifierFixed.GetResult();
        CritMultiplier = OriginCritMultiplier * Mathf.Max(0, (1 + critMultiplierModifierPercentage.GetResult())) + critMultiplierModifierFixed.GetResult();

        isDirty = true;
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(HitInfo hitInfo)
    {
        float basicDamage = hitInfo.GetBasicSourceDamageValue();
        Health = Health - basicDamage;
    }



}
