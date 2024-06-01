using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BuffSystem
{
    [Serializable]
    public class Freeze : Buff
    {
        public Freeze()
        {
            name = "冰冻";
            duration = 1;
        }

        public override void OnStack(BuffController buffController, Buff otherBuff)
        {
            base.OnStack(buffController, otherBuff);
            duration = Mathf.Max(duration, otherBuff.duration);
        }

        public override void OnEnable(BuffController buffController)
        {
            base.OnEnable(buffController);
            buffController.entity.AddTimeScaleRequest(0.01f);

            var material = ABMgr.GetInstance().LoadRes<Material>("material", "Ice");
            AddMaterial(buffController.gameObject, material);

        }

        public override void OnDisable(BuffController buffController)
        {
            base.OnDisable(buffController);
            buffController.entity.RemoveTimeScaleRequest(0.01f);

            RemoveMaterial(buffController.gameObject);
        }


        void AddMaterial(GameObject root, Material material)
        {
            var renderers = root.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = new Material[renderer.materials.Length + 1];
                Array.Copy(renderer.materials, materials, renderer.materials.Length);
                materials[materials.Length - 1] = material;
                renderer.materials = materials;
            }
        }

        void RemoveMaterial(GameObject root)
        {
            var renderers = root.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = new Material[renderer.materials.Length - 1];
                Array.Copy(renderer.materials, materials, renderer.materials.Length - 1);
                renderer.materials = materials;
            }
        }
    }
}

