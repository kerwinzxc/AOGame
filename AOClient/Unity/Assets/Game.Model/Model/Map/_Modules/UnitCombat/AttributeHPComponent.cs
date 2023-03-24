namespace AO
{
    using ET;

    public partial class AttributeHPComponent : Entity, IAwake, IUnitAttribute, IUnitDBComponent
    {
        /// <summary>
        /// ����������
        /// </summary>
        [NotifyAOI, PropertyChanged]
        public int AttributeValue { get; set; }

        /// <summary>
        /// ���õ�����ֵ
        /// </summary>
        [NotifyAOI, PropertyChanged]
        public int AvailableValue { get; set; }
    }
}