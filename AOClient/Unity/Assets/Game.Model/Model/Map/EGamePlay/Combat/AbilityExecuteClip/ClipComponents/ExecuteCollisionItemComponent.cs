﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 
    /// </summary>
    public class ExecuteCollisionItemComponent : Component
    {
        public CollisionExecuteData CollisionExecuteData { get; set; }


        public override void Awake()
        {
            Entity.OnEvent(nameof(ExecuteClip.TriggerEffect), OnTriggerExecutionEffect);
            Entity.OnEvent(nameof(ExecuteClip.EndEffect), OnTriggerEnd);
        }

        public void OnTriggerExecutionEffect(Entity entity)
        {
            //Log.Debug("ExecutionSpawnCollisionComponent OnTriggerExecutionEffect");
            Entity.GetParent<SkillExecution>().SpawnCollisionItem(GetEntity<ExecuteClip>().ExecutionEffectConfig);
        }

        public void OnTriggerEnd(Entity entity)
        {
            //Log.Debug("ExecutionAnimationComponent OnTriggerExecutionEffect");
            //Entity.GetParent<SkillExecution>().OwnerEntity.Publish(AnimationClip);
        }
    }
}