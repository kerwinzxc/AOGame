﻿namespace AO
{
    using AO;
    using ET;
    using ET.Server;
    using System.Collections.Generic;

    public static partial class PlayerOuterRequests
    {
        public static partial async ETTask C2G_EnterMap(Player player, C2G_EnterMap request, G2C_EnterMap response)
        {
            response.MyId = player.Id;
            if (player.UnitId == 0)
            {
                CreateNewAvatar(player).Coroutine();
            }
            await ETTask.CompletedTask;
        }

        public static async ETTask CreateNewAvatar(Player player)
        {
            var sceneComp = AOGame.MapApp.GetComponent<MapSceneComponent>();
            var map1Scene = sceneComp.GetScene("Map1");
            var unitComp = map1Scene.GetComponent<SceneUnitComponent>();

            var newAvatar = map1Scene.AddChildWithId<Avatar>(IdGenerater.Instance.GenerateUnitId(1));

            unitComp.Add(newAvatar);
            player.UnitId = newAvatar.Id;
            var session = ETRoot.Instance.Get(player.GetComponent<GateSessionIdComponent>().GateSessionId);
			session.GetComponent<SessionPlayerComponent>().MessageType2EntityId.Add(typeof(IMapMessage), newAvatar.Id);
			session.GetComponent<SessionPlayerComponent>().MessageType2ActorId.Add(typeof(IMapMessage), newAvatar.InstanceId);
            newAvatar.AddComponent<AvatarCall, long>(session.InstanceId);

            await TimerComponent.Instance.WaitAsync(100);

            var unitInfo = newAvatar.CreateUnitInfo();
            var notifyComps = newAvatar.GetNotifySelfComponents();
            foreach (var comp in notifyComps)
            {
                var compBytes = MongoHelper.Serialize(comp);
                unitInfo.ComponentInfos.Add(new ComponentInfo() { ComponentName = $"{comp.GetType().FullName}", ComponentBytes = compBytes });
            }
            newAvatar.ClientCall.M2C_CreateMyUnit(new M2C_CreateMyUnit() { Unit = unitInfo });

            await TimerComponent.Instance.WaitAsync(1000);

            var msg = new M2C_CreateUnits() { Units = new List<UnitInfo>() };
            foreach (IMapUnit item in unitComp.GetAll())
            {
                if (item == newAvatar) continue;
                unitInfo = item.CreateUnitInfo();
                msg.Units.Add(unitInfo);
            }
            newAvatar.ClientCall.M2C_CreateUnits(msg);

            newAvatar.GetComponent<UnitLevelComponent>().Level = 100;
        }
    }
}