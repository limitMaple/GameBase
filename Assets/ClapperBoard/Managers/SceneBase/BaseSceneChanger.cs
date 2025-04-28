using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ClapperBoard.Managers.Transition;
using UnityEngine.AddressableAssets;

namespace ClapperBoard.Managers.SceneBase {
    /// <summary>
    /// Scene遷移を行うクラス
    /// </summary>
    /// 扱うSceneの型。Headerなどの情報はここに派生を入れる
    /// 扱うSceneは数多くの型を持つが、ジェネリクスの型は全てに共通する型にする
    /// </typeparam>
    public class BaseSceneChanger: MonoBehaviour, ISceneChanger {

    #region Data
    [Serializable]
    public class InitContext : ISceneContext {
        
    }
    #endregion

    #region SerializeField
        [SerializeField] private AssetReference _initialScene;
    #endregion

    #region PrivateField
        /// Sceneの履歴を保持するクラス
        private Stack<ISceneBluePrint> _sceneHistory = new Stack<ISceneBluePrint>();
        // 今のscene
        public IScene currentScene　{ get; private set; }
        
        // 各種実行状況
        private bool _isProcessing;
        private CancellationTokenSource _currentSceneCts;
        
        // シーン遷移の購読
        public Action<IScene> onRunScene;
        public Action<IScene> onExitScene;

        // デフォルトのトランジション設定
        public ITransition defaultSceneLoadTransition = null;
    #endregion

        private async void Start() {
            // アセット読み込み
            var sceneHandle = Addressables.LoadAssetAsync<IScene>(_initialScene);
            var scene = await sceneHandle.Task;

            _sceneHistory.Push(new PrefabSceneBluePrint(_initialScene.ToString(),new InitContext()));
            LoadCurrentHistory(default).Forget();
        }

    #region ISceneChangeController

        public async UniTask ProcessScene(ISceneBluePrint nextScenePrint, CancellationToken ct = default) {
            _sceneHistory.Push(nextScenePrint);
            await LoadCurrentHistory(ct);
        }
        public async UniTask BackScene(CancellationToken ct = default) {
            _sceneHistory.Pop();
            await LoadCurrentHistory(ct);
        }
        public void SetVisible(bool isVisible) {
            // 現在のsceneの非表示
            currentScene.GetSceneComponent<GameObject>()?.SetActive(false);
        }

    #endregion

    #region PrivateMethod
        private async UniTask LoadCurrentHistory(CancellationToken ct = default) {
            ct.ThrowIfCancellationRequested();
            var currentBluePrint = _sceneHistory.Peek();
            if(currentBluePrint == null) return;
            _isProcessing = true;
            if (currentScene != null) {
                // 現在のSceneのキャンセル処理
                _currentSceneCts?.Cancel();
                _currentSceneCts?.Dispose();
                _currentSceneCts = CancellationTokenSource.CreateLinkedTokenSource(
                    this.GetCancellationTokenOnDestroy()
                );
                // 出ていく前に外部からやっておくタスクを実行
                onExitScene?.Invoke(currentScene);
                await currentScene.Exit();
            }

            // トランジションがないなら設定
            if (currentBluePrint.transition == null) {
                currentBluePrint.transition = defaultSceneLoadTransition;
            }
            currentScene = await currentBluePrint.Instantiate<IScene>(this,ct);
            // 実行前に外部からやっておくタスクを実行
            onRunScene?.Invoke(currentScene);
            currentScene.Run(_currentSceneCts.Token).Forget();
            _isProcessing = false;
        }
    #endregion

    }
}