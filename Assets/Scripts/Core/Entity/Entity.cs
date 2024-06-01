using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour
{
    [Header("阵营")]
    public TeamType teamType = TeamType.Monster;


    public void SetTimeScale(float t)
    {
        AddTimeScaleRequest(t);
    }

    //TODO: 用request的方式实现可靠性
    #region 时间速率
    private float _timeScale = 1.0f;
    /// <summary>
    /// 时间速率,设置为0将会使刚体不可逆地失去所有速度
    /// </summary>
    public virtual float TimeScale
    {
        get { return _timeScale; }
        protected set
        {
            if (value < 0.0f)
            {
                throw new System.Exception("时间速率不可小于0");
            }
            float lastTimeScale = _timeScale;
            _timeScale = value;
            //设置子物体
            Entity[] childenEntity = gameObject.GetComponentsInChildren<Entity>();
            for (int i = 0; i < childenEntity.Length; i++)
            {
                if (childenEntity[i] != this)
                {
                    childenEntity[i].TimeScale = value;
                }
            }
            //设置组件的速度
            if (AnimatorComponent != null)
            {
                AnimatorComponent.speed = value;
            }
            if (RigidbodyComponent != null)
            {
                RigidbodyComponent.velocity = value / lastTimeScale * RigidbodyComponent.velocity;
                RigidbodyComponent.angularVelocity = value / lastTimeScale * RigidbodyComponent.angularVelocity;
                RigidbodyComponent.angularDrag = value / lastTimeScale * RigidbodyComponent.angularDrag;
                RigidbodyComponent.drag = value / lastTimeScale * RigidbodyComponent.drag;
            }
        }
    }
    private RequestValue<float> timeScaleRequest = new RequestValue<float>((a, b) => a * b, 1);
    public void AddTimeScaleRequest(float timeScale)
    {
        timeScaleRequest.AddRequest(timeScale);
        TimeScale = timeScaleRequest.GetResult();
    }
    public void RemoveTimeScaleRequest(float timeScale)
    {
        timeScaleRequest.RemoveRequest(timeScale);
        TimeScale = timeScaleRequest.GetResult();
    }


    public void ModifyTimeScale(float timeScale)
    {
        TimeScale = timeScale;
    }

    #endregion


    #region 组件
    private bool hasAnimator = true;
    private Animator _animator;
    /// <summary>
    /// 动画器
    /// </summary>
    public Animator AnimatorComponent
    {
        get
        {
            if (hasAnimator == true && _animator == null)
            {
                _animator = GetComponent<Animator>();
                hasAnimator = _animator != null;
                if (hasAnimator)
                {
                    _animator.applyRootMotion = false;
                }
            }
            if (hasAnimator == false) return null;
            return _animator;
        }
    }

    private bool hasRigidbody = true;
    private Rigidbody _rigidbody = null;
    /// <summary>
    /// 刚体
    /// </summary>
    public Rigidbody RigidbodyComponent
    {
        get
        {
            if (hasRigidbody == true && _rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
                hasRigidbody = _rigidbody != null;
            }
            if (hasRigidbody == false) return null;
            return _rigidbody;
        }
    }
    #endregion


    #region Unity消息

    private void Reset()
    {
        //子物体的碰撞体的层 应设置为
        //gameObject.layer = LayerMask.NameToLayer("Entity");
    }
    void Start()
    {
        EntityStart();
    }


    void Update()
    {
        EntityUpdate(Time.deltaTime * TimeScale);
    }

    private void FixedUpdate()
    {
        EntityFixUpdate(Time.fixedDeltaTime * TimeScale);
    }

    private void OnEnable()
    {
        EntityOnEnable();
    }

    private void OnDisable()
    {
        EntityOnDisable();
    }

    #endregion


    #region Entity消息
    public virtual void EntityStart()
    {

    }

    public virtual void EntityUpdate(float dt)
    {

    }

    public virtual void EntityFixUpdate(float dt)
    {
        if (RigidbodyComponent != null)
        {
            RootMotionFixedUpdate();
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
    public virtual void EntityOnEnable()
    {

    }

    public virtual void EntityOnDisable()
    {

    }


    #endregion


    #region 动画相关



    Dictionary<string, float> clipLenthDic = new();
    /// <summary>
    /// 获取一个动画片段的长度，名称是剪辑的名称，不是状态的名称
    /// </summary>
    /// <param name="clipName"></param>
    /// <returns></returns>
    public float GetClipLength(string clipName)
    {
        if (clipLenthDic.ContainsKey(clipName))
        {
            return clipLenthDic[clipName];
        }
        if (AnimatorComponent == null) { return 0; }
        var clips = AnimatorComponent.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i].name == clipName)
            {
                return clipLenthDic[clipName] = clips[i].length;
            }
        }
        return 0;
    }

    #endregion








    #region 根运动相关
    [Header("根运动相关")]
    public bool applyRootMotionPosition = false;
    public bool applyRootMotionRotation = false;
    /// <summary>
    /// 被其它系统接管？
    /// </summary>
    public bool isTakenOver = false;
    public Vector3 rootMotionFixedDeltaPosition = Vector3.zero;
    public Quaternion rootMotionFixedDeltaRotation = Quaternion.identity;

    private void OnAnimatorMove()
    {
        RootMotionUpdate();
    }

    void RootMotionUpdate()
    {
        if (AnimatorComponent != null)
        {
            if (applyRootMotionPosition)
            {
                rootMotionFixedDeltaPosition += AnimatorComponent.deltaPosition;
            }
            if (applyRootMotionRotation)
            {
                rootMotionFixedDeltaRotation = AnimatorComponent.deltaRotation * rootMotionFixedDeltaRotation;
            }
        }
    }

    //本质是缺少优先级判定
    void RootMotionFixedUpdate()
    {
        //清空速度并应用根运动
        if (isTakenOver == false)
        {
            if (applyRootMotionPosition)
            {
                //AddForceByTaregtPosition(RigidbodyComponent.position + rootMotionFixedDeltaPosition);
                //RigidbodyComponent.velocity = Vector3.zero;
                AddForceSmooothlyByTargetVelocity(Vector3.zero, 10);
                RigidbodyComponent.MovePosition(RigidbodyComponent.position + rootMotionFixedDeltaPosition);
            }
            if (applyRootMotionRotation)
            {
                //AddTorqueByTargetRotation(rootMotionFixedDeltaRotation * RigidbodyComponent.rotation);
                AddTorqueByTargetAngularVelocity(Vector3.Lerp(RigidbodyComponent.angularVelocity, Vector3.zero, 0.2f));
                RigidbodyComponent.MoveRotation(rootMotionFixedDeltaRotation * RigidbodyComponent.rotation);
            }

            rootMotionFixedDeltaPosition = Vector3.zero;
            rootMotionFixedDeltaRotation = Quaternion.identity;
        }


    }

    //Vector3 fixRootDeltaPosition = Vector3.zero;
    //Quaternion fixRootDeltaRotation = Quaternion.identity;

    //void RootDeltaFixUpdate(float dt)
    //{
    //    Vector3 nextPosition = RigidbodyComponent.position + fixRootDeltaPosition;
    //    RigidbodyComponent.MovePosition(nextPosition);
    //    fixRootDeltaPosition = Vector3.zero;
    //    Quaternion nextRotation = fixRootDeltaRotation * RigidbodyComponent.rotation;
    //    RigidbodyComponent.MoveRotation(nextRotation);
    //    fixRootDeltaRotation = Quaternion.identity;
    //}


    ///// <summary>
    ///// 使用一次根运动位移，在update中调用
    ///// </summary>
    //public void AddRootMotionPositionDelta()
    //{
    //    if (hasAnimator)
    //    {
    //        fixRootDeltaPosition += AnimatorComponent.deltaPosition;
    //    }
    //}

    ///// <summary>
    ///// 使用一次根运动选择，在update中调用
    ///// </summary>
    //public void AddRootMotionRotateDelta()
    //{
    //    if (hasAnimator)
    //    {
    //        fixRootDeltaRotation = AnimatorComponent.deltaRotation * fixRootDeltaRotation;
    //    }
    //}

    #endregion


    #region 运动相关
    /// <summary>
    /// 通过施加力的方式，使得下一帧fixUpdate后到达目标位置,误差有点大？
    /// </summary>
    /// <param name="targetPosition"></param>
    public void AddForceByTaregtPosition(Vector3 targetPosition)
    {
        Vector3 deltaPosition = targetPosition - RigidbodyComponent.position;
        Vector3 velocity = deltaPosition / Time.fixedDeltaTime;
        AddForceByTargetVelocity(velocity / TimeScale);
    }
    /// <summary>
    /// 通过施加力的方式，使得下一次fixUpdate后到目标速度 (目标速度会经过timeScale变换)
    /// </summary>
    /// <param name="targetVelocity"></param>
    public void AddForceByTargetVelocity(Vector3 targetVelocity)
    {
        Vector3 realTargetVelocity = targetVelocity * TimeScale;
        Vector3 deltaVelocity = realTargetVelocity - RigidbodyComponent.velocity;
        RigidbodyComponent.AddForce(deltaVelocity, ForceMode.VelocityChange);
    }
    /// <summary>
    /// 通过添加力的方式，平滑过渡到目标速度
    /// </summary>
    /// <param name="targetVelocity"></param>
    /// <param name="smooth"></param>
    /// <returns></returns>
    public Vector3 AddForceSmooothlyByTargetVelocity(Vector3 targetVelocity, float smooth = 1)
    {
        Vector3 smoothTargetVelocity = Vector3.Lerp(RigidbodyComponent.velocity, targetVelocity * TimeScale, smooth * TimeScale * Time.fixedDeltaTime);
        AddForceByTargetVelocity(smoothTargetVelocity / TimeScale);
        return smoothTargetVelocity;
    }
    /// <summary>
    /// 施加力，用法同刚体的Addforce，但是考虑的entity的时间尺度
    /// </summary>
    /// <param name="force"></param>
    /// <param name="forceMode"></param>
    public void AddForce(Vector3 force, ForceMode forceMode)
    {
        RigidbodyComponent.AddForce(force * TimeScale, forceMode);
    }
    /// <summary>
    /// 通过目标角速度设添加力矩
    /// </summary>
    /// <param name="targetAngularVelocity"></param>
    public void AddTorqueByTargetAngularVelocity(Vector3 targetAngularVelocity)
    {
        Vector3 deltaAngularVelocity = targetAngularVelocity - RigidbodyComponent.angularVelocity;
        RigidbodyComponent.AddTorque(deltaAngularVelocity, ForceMode.VelocityChange);
    }

    /// <summary>
    /// 设置旋转，使得下一次fixUpdate后到targetRotation
    /// </summary>
    /// <param name="targetRotation"></param>
    public void AddTorqueByTargetRotation(Quaternion targetRotation)
    {
        Quaternion deltaQuaternion;
        if (IsObtuse(targetRotation, RigidbodyComponent.rotation))
        {
            deltaQuaternion = Invert(targetRotation) * Quaternion.Inverse(RigidbodyComponent.rotation);
        }
        else
        {
            deltaQuaternion = targetRotation * Quaternion.Inverse(RigidbodyComponent.rotation);
        }
        Vector3 targetAngularVelocity = deltaQuaternion.eulerAngles * Mathf.PI / 180.0f;
        targetAngularVelocity.x = targetAngularVelocity.z = 0;
        while (targetAngularVelocity.y > Mathf.PI) targetAngularVelocity.y -= Mathf.PI * 2.0f;
        while (targetAngularVelocity.y < -Mathf.PI) targetAngularVelocity.y += Mathf.PI * 2.0f;
        targetAngularVelocity /= Time.fixedDeltaTime;
        Vector3 deltaAngularVelocity = targetAngularVelocity - RigidbodyComponent.angularVelocity;
        RigidbodyComponent.AddTorque(deltaAngularVelocity, ForceMode.VelocityChange);
    }
    /// <summary>
    /// 通过施加力矩，平滑转到目标旋转,smooth不需要乘以dt
    /// </summary>
    /// <param name="targetRotation"></param>
    /// <param name="smooth"></param>
    /// <returns></returns>
    public Quaternion AddTorqueSmoothlyByTargetRotation(Quaternion targetRotation, float smooth = 1)
    {
        //Debug.Log("rotation = " + targetRotation + ", smooth = " + smooth);
        Quaternion smoothTargetRotation;
        if (IsObtuse(RigidbodyComponent.rotation, targetRotation))
        {
            smoothTargetRotation = Quaternion.Lerp(RigidbodyComponent.rotation, Invert(targetRotation), smooth * TimeScale * Time.fixedDeltaTime);
        }
        else
        {
            smoothTargetRotation = Quaternion.Lerp(RigidbodyComponent.rotation, targetRotation, smooth * TimeScale * Time.fixedDeltaTime);
        }
        AddTorqueByTargetRotation(smoothTargetRotation);
        return smoothTargetRotation;
    }
    //两个四元数是否为钝角
    bool IsObtuse(Quaternion a, Quaternion b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w < 0;
    }

    //返回一个取反的四元数
    Quaternion Invert(Quaternion quaternion)
    {
        return new Quaternion(-quaternion.x, -quaternion.y, -quaternion.z, -quaternion.w);
    }

    /// <summary>
    /// 设置维持目标速度的静摩擦力
    /// </summary>
    /// <param name="friction"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void SetStaticFriction(float friction)
    {
        throw new System.NotImplementedException();
    }
    #endregion


    #region 位置、速度和旋转相关,废弃
    //private bool hasTargetVelocity = false;
    //private Vector3 targetVelocity = Vector3.zero;
    //private bool hasTargetQuaternion = false;
    //private Quaternion targetQuaternion = Quaternion.identity;


    ////通过设置加速度来改变速度
    //void TranformFixUpdate(float dt)
    //{
    //    if (hasTargetVelocity)
    //    {
    //        hasTargetVelocity = false;
    //        //加入时间速率的真正目标速度
    //        Vector3 realTargetVelocity = targetVelocity * TimeScale;
    //        Vector3 deltaVelocity = realTargetVelocity - RigidbodyComponent.velocity;
    //        RigidbodyComponent.AddForce(deltaVelocity, ForceMode.VelocityChange);
    //    }
    //    if (hasTargetQuaternion)
    //    {
    //        hasTargetQuaternion = false;
    //        Quaternion deltaQuaternion;
    //        if (IsObtuse(targetQuaternion, RigidbodyComponent.rotation))
    //        {
    //            deltaQuaternion = Invert(targetQuaternion) * Quaternion.Inverse(RigidbodyComponent.rotation);
    //        }
    //        else
    //        {
    //            deltaQuaternion = targetQuaternion * Quaternion.Inverse(RigidbodyComponent.rotation);
    //        }
    //        Vector3 targetAngularVelocity = deltaQuaternion.eulerAngles * Mathf.PI / 180.0f;
    //        targetAngularVelocity.x = targetAngularVelocity.z = 0;
    //        while (targetAngularVelocity.y > Mathf.PI) targetAngularVelocity.y -= Mathf.PI * 2.0f;
    //        while (targetAngularVelocity.y < -Mathf.PI) targetAngularVelocity.y += Mathf.PI * 2.0f;
    //        targetAngularVelocity /= dt;
    //        Vector3 deltaAngularVelocity = targetAngularVelocity - RigidbodyComponent.angularVelocity;
    //        RigidbodyComponent.AddTorque(deltaAngularVelocity, ForceMode.VelocityChange);
    //    }
    //}


    //public void SetTargetPosition(Vector3 position)
    //{
    //    RigidbodyComponent.MovePosition(position);
    //}


    ///// <summary>
    ///// 设置目标速度
    ///// </summary>
    ///// <param name="velocity"></param>
    //public void SetTargetVelocity(Vector3 velocity)
    //{
    //    hasTargetVelocity = true;
    //    targetVelocity = velocity;
    //}

    ///// <summary>
    ///// 取消目标速度
    ///// </summary>
    //public void CancleTargetVelocity()
    //{
    //    hasTargetVelocity = false;
    //    targetVelocity = Vector3.zero;
    //}

    ///// <summary>
    ///// 平滑设置目标速度，建议在fixUpdate中调用，否则不稳定
    ///// </summary>
    ///// <param name="velocity"></param>
    ///// <param name="smooth"></param>
    //public Vector3 SetTargetVelocitySmoothly(Vector3 velocity, float smooth = 1)
    //{
    //    hasTargetVelocity = true;
    //    return targetVelocity = Vector3.Lerp(targetVelocity, velocity, smooth * TimeScale * Time.fixedDeltaTime);
    //}

    //public void SetTargetQuaternion(Quaternion quaternion)
    //{
    //    hasTargetQuaternion = true;
    //    targetQuaternion = quaternion;
    //}

    //public Quaternion SetTargetQuaternionSmoothly(Quaternion quaternion, float smooth = 1)
    //{
    //    hasTargetQuaternion = true;
    //    if (IsObtuse(targetQuaternion, quaternion))
    //    {
    //        return targetQuaternion = Quaternion.Lerp(targetQuaternion, Invert(quaternion), smooth * TimeScale * Time.fixedDeltaTime);
    //    }
    //    else
    //    {
    //        return targetQuaternion = Quaternion.Lerp(targetQuaternion, quaternion, smooth * TimeScale * Time.fixedDeltaTime);
    //    }
    //}


    #endregion
}
