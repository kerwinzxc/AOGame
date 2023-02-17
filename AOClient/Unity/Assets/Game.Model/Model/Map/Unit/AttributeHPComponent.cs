namespace AO
{
    using ET;

    public partial class AttributeHPComponent : Entity, IAwake
    {
        /// <summary>
        /// ����������
        /// </summary>
        [NotifyAOI]
        public int Attribute_HP { get; set; }

        /// <summary>
        /// ���õ�����ֵ
        /// </summary>
        [NotifyAOI]
        public int Available_HP { get; set; }
        public int HP => Available_HP;
    }
}