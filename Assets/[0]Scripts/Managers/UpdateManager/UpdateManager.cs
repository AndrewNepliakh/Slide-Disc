using System;
using Interfaces;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "UpdateManager", menuName = "Managers/UpdateManager")]
    public class UpdateManager : BaseInjectable, IAwake
    {
        public void OnAwake()
        {
            GameObject.Find("[EnterPoint]").GetComponent<UpdateManagerMonoBehaviour>().SetUp(this);
        }
        public void Update()
        {
            foreach (var entity in MonoEntity.AllUpdates)
            {
                entity.LocaUpdate();
            }
        }
    }
}
