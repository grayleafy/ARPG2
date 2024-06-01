using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BuffSystem
{
    [Serializable]
    public class BreakIce : Buff
    {
        public string breakIceAbName = "particle";
        public string breakIceParticle = "破冰";

        public Vector3 offsetPosition;
        public Vector3 localScale = Vector3.one;

        public HitConfig hitConfig;



        public override void OnEnable(BuffController buffController)
        {
            base.OnEnable(buffController);

            Debug.Log("破冰");

            //检测有没有冻结
            Buff freeze = null;
            foreach (Buff buff in buffController.buffs)
            {
                if (buff is Freeze)
                {
                    freeze = buff;
                }
            }

            //如果已经被冻结
            if (freeze != null)
            {
                //移除冻结buff
                buffController.RemoveBuff(freeze);


                //一次攻击事件
                HitInfo hitInfo = new HitInfo();
                hitInfo.target = buffController.entity as IHittable;
                hitInfo.hitGameObject = buffController.gameObject;
                hitInfo.hitPosition = buffController.transform.position;
                hitInfo.hitImpact = Vector3.zero;
                hitInfo.hitConfig = hitConfig;

                //镜头抖动
                CameraMgr.GetInstance().AddShakeRequest(hitInfo.hitConfig.shakeRequest);

                //协程顿帧,顿帧结束后调用受击者的受击事件
                if (hitConfig.FreezeFrameDuration > 0)
                {
                    (hitInfo.target as Entity).AddTimeScaleRequest(0.01f);
                    hitInfo.hitConfig.hitSource.AddTimeScaleRequest(0.01f);
                    //一段时间后结束顿帧，并且执行受击效果
                    RuntimeTimer.GetInstance().WaitForInvoke(hitConfig.FreezeFrameDuration, () =>
                    {
                        (hitInfo.target as Entity).RemoveTimeScaleRequest(0.01f);
                        hitInfo.hitConfig.hitSource.RemoveTimeScaleRequest(0.01f);
                        hitInfo.target.OnHit(hitInfo);
                    });
                }
                else
                {
                    hitInfo.target.OnHit(hitInfo);
                }

                LoadParticle(buffController);
            }

            duration = -1;
        }

        //加载破冰粒子效果
        void LoadParticle(BuffController buffController)
        {
            PoolMgr.GetInstance().GetObj(breakIceAbName, breakIceParticle, (obj) =>
            {
                obj.transform.SetParent(buffController.transform);
                obj.transform.localPosition = offsetPosition;
                obj.transform.localScale = localScale;
            });
        }
    }


}

