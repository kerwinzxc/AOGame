﻿using AO;
using cfg.Map;
using System;
using System.Numerics;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Gate)]
	public class EnterSceneRequestHandler : AMActorRpcHandler<Scene, EnterSceneRequest, EnterSceneResponse>
	{
		protected override async ETTask Run(Scene scene, EnterSceneRequest request, EnterSceneResponse response)
		{
			Log.Console("EnterSceneRequestHandler");

            var mapScene = scene;
            var unitComp = mapScene.GetComponent<SceneUnitComponent>();
            var newAvatar = mapScene.AddChildWithId<Avatar>(IdGenerater.Instance.GenerateUnitId(mapScene.DomainZone()));
            newAvatar.Cache();

            newAvatar.AddComponent<AvatarClient, long>(request.GateSessionId);

            newAvatar.ClientCall.M2C_OnEnterMap(new M2C_OnEnterMap() { MapName = mapScene.Type, Scene = mapScene.CreateUnitInfo() });
            unitComp.Add(newAvatar);

            var unitInfo = newAvatar.CreateUnitInfo();
            var notifyComps = newAvatar.GetNotifySelfComponents();
            foreach (var comp in notifyComps)
            {
                var compBytes = MongoHelper.Serialize(comp);
                unitInfo.ComponentInfos.Add(new ComponentInfo() { ComponentName = $"{comp.GetType().FullName}", ComponentBytes = compBytes });
            }
            newAvatar.ClientCall.M2C_CreateMyUnit(new M2C_CreateMyUnit() { Unit = unitInfo });

            response.UnitId = newAvatar.Id;
            response.UnitInstanceId = newAvatar.InstanceId;

            await ETTask.CompletedTask;
		}
	}
}