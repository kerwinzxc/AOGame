namespace AO
{
    using ET;

    public partial class AttributeHPComponent : Entity, IAwake
    {
        /// <summary>
        /// ����������
        /// </summary>
        [NotifyAOI, PropertyChanged]
        public int Attribute_HP { get; set; }

        /// <summary>
        /// ���õ�����ֵ
        /// </summary>
        [NotifyAOI, PropertyChanged]
        public int Available_HP { get; set; }
        public int HP => Available_HP;
    }
}