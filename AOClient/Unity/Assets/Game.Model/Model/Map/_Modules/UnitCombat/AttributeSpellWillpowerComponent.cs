namespace AO
{
    using ET;

    public partial class AttributeSpellWillpowerComponent : Entity, IAwake, IUnitAttribute, IUnitDBComponent
    {
        /// <summary>
        /// ʩ����������ֵ
        /// </summary>
        [NotifyAOI, PropertyChanged]
        public int AttributeValue { get; set; }

        /// <summary>
        /// ���õ�ʩ������
        /// </summary>
        [NotifyAOI, PropertyChanged]
        public int AvailableValue { get; set; }
    }
}