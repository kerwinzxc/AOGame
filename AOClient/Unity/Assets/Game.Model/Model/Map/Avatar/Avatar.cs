namespace AO
{
    using ET;
    using Unity.Mathematics;

    public partial class Avatar : Entity, IMapUnit, IAwake
    {
        public string? Name { get; set; }

        public int ConfigId { get; set; }

        private float3 position;
        public float3 Position
        {
            get
            {
                return position;
            }
            set
            {
                float3 oldPos = this.position;
                this.position = value;
                AOGame.Publish(new EventType.ChangePosition() { Unit = this, OldPos = oldPos });
            }
        }

#if !UNITY
        public AvatarCall.ClientCall ClientCall => GetComponent<AvatarCall>().Client;
#else
        /// <summary>
        /// ���������
        /// </summary>
        public static Avatar Main { get; set; }
        ///// <summary>
        ///// ����������
        ///// </summary>
        //public static Scene CurrentScene { get; set; }
#endif
    }
}