namespace AO
{
    using ET;
    using Sirenix.OdinInspector;
    using System.Collections.Generic;

    [LabelText("��ײ����״")]
    public enum CollisionShape
    {
        [LabelText("Բ��")]
        Sphere,
        [LabelText("����")]
        Box,
        [LabelText("����")]
        Sector,
        [LabelText("�Զ���")]
        Custom,
    }

    public partial class UnitCollisionComponent : Entity, IAwake, IUpdate
    {
        public HashSet<long> StayUnits { get; set; } = new HashSet<long>();
        [NotifyAOI]
        public CollisionShape CollisionShape { get; set; } = CollisionShape.Sphere;
        [NotifyAOI]
        public float Radius { get; set; } = 1f;
    }
}