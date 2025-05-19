using System.Threading;
using ClapperBoard.Managers.PageBase;
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace ClapperBoard.Managers {
    /// <summary>
    /// </summary>
    public abstract class BasePageMonoBehaviour : MonoBehaviour, IPage {
        protected IPageChanger pageChanger;

        // Sceneの各種コンポーネント取得用のインターフェース
        public T GetSceneComponent<T>() {
            return gameObject.GetComponent<T>();
        }

        protected virtual UniTask OnInit(IPageContext context) {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnStart(CancellationToken ct) {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnExit() {
            return UniTask.CompletedTask;
        }

#region IScene

        public async UniTask Init(IPageContext context, IPageChanger newPageChanger) {
            pageChanger = newPageChanger;
            await OnInit(context);
        }

        public async UniTask Run(CancellationToken ct) {
            await OnStart(ct);
        }

        public async UniTask Exit() {
            await OnExit();
            Destroy(gameObject);
        }

#endregion
    }
}