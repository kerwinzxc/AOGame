namespace AO
{
    using ET;
    using UnityEngine;

    [Event(SceneType.Process)]
    public class CreateMyUnit_CreateUnit : AEvent<EventType.CreateMyUnit>
    {
        protected override async ETTask Run(Scene scene, EventType.CreateMyUnit args)
        {
            Log.Debug("CreateMyUnit_CreateUnit Run");

            var unitInfo = args.Unit;
            var unitType = (UnitType)unitInfo.Type;
            if (unitType == UnitType.Player)
            {
                AOGame.TryGet(out MapSceneComponent sceneComp);
                //var sceneComp = AOGame.ClientApp.GetComponent<MapSceneComponent>();
                var map1Scene = sceneComp.GetScene("map1");
                //var map1Scene = MapSceneComponentSystem.GetScene(sceneComp, "map1");
                var unitComp = map1Scene.GetComponent<SceneUnitComponent>();

                var avatar = map1Scene.AddChildWithId<Avatar>(unitInfo.UnitId);
                SceneUnitComponentSystem.Add(unitComp, avatar);

                var asset = await AssetUtils.LoadAssetAsync("Hero.prefab").Task;
                Log.Debug($"CreateMyUnit_CreateUnit LoadAssetAsync {asset.Object}");
                var prefab = asset.GameObject;
                var obj = GameObject.Instantiate(prefab);
                obj.transform.position = new Vector3(unitInfo.Position.x, unitInfo.Position.y, unitInfo.Position.z);
                obj.transform.forward = new Vector3(unitInfo.Forward.x, unitInfo.Forward.y, unitInfo.Forward.z);
                avatar.AddComponent<UnitViewComponent>().UnitObj = obj;
                avatar.AddComponent<UnitMoveComponent>();
                avatar.AddComponent<AvatarControlComponent>();
            }

            await ETTask.CompletedTask;
        }
    }
}