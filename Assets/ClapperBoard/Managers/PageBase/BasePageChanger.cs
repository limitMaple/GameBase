using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ClapperBoard.Managers.PageBase {
    /// <summary>
    ///     Scene遷移を行うクラス
    /// </summary>
    /// <typeparam name="TScene">
    ///     扱うSceneの型。Headerなどの情報はここに派生を入れる
    ///     扱うSceneは数多くの型を持つが、ジェネリクスの型は全てに共通する型にする
    /// </typeparam>
    public class BasePageChanger<TScene> : IPageChanger where TScene : IPage {
#region Constructor

        public BasePageChanger(Transform root) {
            this.root = root;
        }

#endregion

#region PrivateMethod

        private async UniTask LoadCurrentHistory(CancellationToken ct = default) {
            ct.ThrowIfCancellationRequested();
            var currentBluePrint = _sceneHistory.Peek();
            if (currentBluePrint == null) return;
            _isProcessing = true;
            if (currentScene != null) {
                // 現在のSceneのキャンセル処理
                _currentSceneCts?.Cancel();
                _currentSceneCts?.Dispose();
                _currentSceneCts = new CancellationTokenSource();
                // 出ていく前に外部からやっておくタスクを実行
                onExitScene?.Invoke(currentScene);
                await currentScene.Exit();
            }

            currentScene = await currentBluePrint.Instantiate<TScene>(this, ct);
            // 実行前に外部からやっておくタスクを実行
            onRunScene?.Invoke(currentScene);
            currentScene.Run(_currentSceneCts.Token).Forget();
            _isProcessing = false;
        }

#endregion

#region PrivateField

        /// Sceneの履歴を保持するクラス
        private readonly Stack<IPageBluePrint> _sceneHistory = new();

        // 今のscene
        public TScene currentScene　{ get; private set; }

        // 各種実行状況
        private bool _isProcessing;
        private CancellationTokenSource _currentSceneCts = new();

        // シーン遷移の購読
        public Action<TScene> onRunScene;
        public Action<TScene> onExitScene;

#endregion
        
#region ISceneChangeController

        public Transform root { get; set; }

        public async UniTask ProcessPage(IPageBluePrint nextPagePrint, CancellationToken ct = default) {
            _sceneHistory.Push(nextPagePrint);
            await LoadCurrentHistory(ct);
        }

        public async UniTask BackPage(CancellationToken ct = default) {
            _sceneHistory.Pop();
            await LoadCurrentHistory(ct);
        }

#endregion
    }
}