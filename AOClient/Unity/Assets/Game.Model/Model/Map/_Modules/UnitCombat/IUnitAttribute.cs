namespace AO
{
    using ET;

    public interface IUnitAttribute
    {
        /// <summary>
        /// ����ֵ����ɫ�ĵȼ�����ֵ��ֻ�ܵȼ�Ӱ��
        /// </summary>
        public int AttributeValue { get; set; }

        /// <summary>
        /// ����ֵ������ʵ�ʿ��õ�ֵ����װ����ս����buff��Ӱ��
        /// </summary>
        public int AvailableValue { get; set; }
    }
}