namespace AO
{
    using ET;
    using Sirenix.OdinInspector;

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
        [NotifyAOI]
        public CollisionShape CollisionShape { get; set; }
        [NotifyAOI]
        public float Radius { get; set; }
    }
}