using System;
using System.Threading;
using ClapperBoard.Managers.Transition;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace ClapperBoard.Managers.PageBase {
    /// <summary>
    ///     Sceneを生成するタスクの基礎的な実装。
    ///     Stackにまとめておいて、必要になったらここから生成する
    /// </summary>
    public class PrefabPageBluePrint : IPageBluePrint {
        private readonly AssetReferenceGameObject _assetReference;

        /// Sceneを初期化するために必要な文脈
        private readonly IPageContext _context;
        
        private readonly ITransition _transition;

        public PrefabPageBluePrint(AssetReferenceGameObject assetReference, IPageContext context,
            ITransition transition) {
            _assetReference = assetReference;
            _context = context;
            _transition = transition;
        }

        public PrefabPageBluePrint(string assetPath, IPageContext context, ITransition transition) {
            _assetReference = new AssetReferenceGameObject(assetPath);
            _context = context;
            _transition = transition;
        }

        public Func<UniTask> onBeforeInit { get; set; }

        // Progressを通して読み込み情報を受け取る
        public async UniTask<TSceneBaseType> Instantiate<TSceneBaseType>(
            IPageChanger pageChanger,
            CancellationToken ct = default) where TSceneBaseType : IPage {
            if (_transition != null) await _transition.Enter();
            if (onBeforeInit != null) await onBeforeInit.Invoke();

            var handle = Addressables.LoadAssetAsync<GameObject>(_assetReference);
            var obj = Object.Instantiate(await handle.Task, pageChanger.root);
            if (_transition != null) await _transition.Complete();
            var scene = obj.GetComponent<TSceneBaseType>();
            await scene.Init(_context, pageChanger);
            return scene;
        }
    }
}