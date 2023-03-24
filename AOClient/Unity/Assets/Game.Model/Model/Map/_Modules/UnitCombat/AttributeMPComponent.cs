namespace AO
{
    using ET;

    public partial class AttributeMPComponent : Entity, IAwake, IUnitAttribute, IUnitDBComponent
    {
        /// <summary>
        /// ����ħ��ֵ
        /// </summary>
        [NotifyAOI, PropertyChanged]
        public int AttributeValue { get; set; }

        /// <summary>
        /// ���õ�ħ��ֵ
        /// </summary>
        [NotifyAOI, PropertyChanged]
        public int AvailableValue { get; set; }
    }
}