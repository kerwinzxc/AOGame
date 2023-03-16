using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using EGamePlay.Combat;

namespace EGamePlay.Combat
{
    public class EffectAssignAbility : Entity, IActionAbility
    {
        public CombatEntity OwnerEntity { get { return GetParent<CombatEntity>(); } set { } }
        public bool Enable { get; set; }


        public bool TryMakeAction(out EffectAssignAction action)
        {
            if (Enable == false)
            {
                action = null;
            }
            else
            {
                action = OwnerEntity.AddChild<EffectAssignAction>();
                action.ActionAbility = this;
                action.Creator = OwnerEntity;
            }
            return Enable;
        }
    }

    /// <summary>
    /// Ч��Ӧ���ж�
    /// </summary>
    public class EffectAssignAction : Entity, IActionExecution
    {
        /// �������Ч��Ӧ���ж���Դ����
        public Entity SourceAbility { get; set; }
        /// Ŀ���ж�
        public IActionExecution TargetAction { get; set; }
        public AbilityEffect AbilityEffect { get; set; }
        public AbilityItem AbilityItem { get; set; }
        public Effect EffectConfig => AbilityEffect.EffectConfig;
        /// �ж�����
        public Entity ActionAbility { get; set; }
        /// Ч��Ӧ���ж�Դ
        public EffectAssignAction SourceAssignAction { get { return null; } set { } }
        /// �ж�ʵ��
        public CombatEntity Creator { get; set; }
        /// Ŀ�����
        public CombatEntity Target { get; set; }
        public Entity AssignTarget { get; set; }


        /// ǰ�ô���
        private void PreProcess()
        {
            if (AssignTarget is CombatEntity combatEntity)
            {
                Target = combatEntity;
            }
        }

        public void AssignEffect()
        {
            //Log.Debug($"ApplyEffectAssign {EffectConfig}");
            PreProcess();

            foreach (var item in AbilityEffect.Components.Values)
            {
                if (item is IEffectTriggerSystem effectTriggerSystem)
                {
                    effectTriggerSystem.OnTriggerApplyEffect(this);
                }
            }

            PostProcess();

            FinishAction();
        }

        public void FillDatasToAction(IActionExecution action)
        {
            action.SourceAssignAction = this;
            action.Target = Target;
        }

        /// ���ô���
        private void PostProcess()
        {
            Creator.TriggerActionPoint(ActionPointType.AssignEffect, this);
            Target.TriggerActionPoint(ActionPointType.ReceiveEffect, this);
        }

        public void FinishAction()
        {
            Entity.Destroy(this);
        }
    }
}