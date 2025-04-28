using System;
using System.Threading;
using ClapperBoard.Managers.Transition;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace ClapperBoard.Managers.SceneBase {
    /// <summary>
    /// Sceneを生成するタスクの基礎的な実装。
    /// Stackにまとめておいて、必要になったらここから生成する
    /// </summary>
    public class PrefabSceneBluePrint : ISceneBluePrint{
        private readonly AssetReferenceGameObject _assetReference;

        /// Sceneを初期化するために必要な文脈
        public ISceneContext context { get; set; }
        public ITransition transition { get; set; }
        public Func<UniTask> onBeforeInit { get; set; }

        public PrefabSceneBluePrint(AssetReferenceGameObject assetReference, ISceneContext context) {
            _assetReference = assetReference;
            this.context = context;
        }

        public PrefabSceneBluePrint(string assetPath, ISceneContext context) {
            _assetReference = new AssetReferenceGameObject(assetPath);
            this.context = context;
        }

        // Progressを通して読み込み情報を受け取る
        public async UniTask<TSceneBaseType> Instantiate<TSceneBaseType>(
            ISceneChanger sceneChanger,
            CancellationToken ct = default) where TSceneBaseType : IScene {

            if (transition != null) {
                await transition.Enter();
            }
            if (onBeforeInit != null) {
                await onBeforeInit.Invoke();
            }
            
            var handle = Addressables.LoadAssetAsync<GameObject>(_assetReference);
            GameObject obj = await handle.Task;
            if (transition != null) {
                await transition.Complete();
            }
            TSceneBaseType scene = obj.GetComponent<TSceneBaseType>();
            await scene.Init(context, sceneChanger);
            return scene;
        }
    }
}