using System.Threading;
using ClapperBoard.Managers.SceneBase;
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace ClapperBoard.Managers {
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseSceneMonoBehaviour : MonoBehaviour,IScene {
        protected ISceneChanger sceneChanger;
        #region IScene
        public async UniTask Init(ISceneContext context, ISceneChanger newSceneChanger) {
            sceneChanger = newSceneChanger;
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


        protected virtual UniTask OnInit(ISceneContext context) {
            return UniTask.CompletedTask;
        }
        protected virtual UniTask OnStart(CancellationToken ct) {
            return UniTask.CompletedTask;
        }
        protected virtual UniTask OnExit() {
            return UniTask.CompletedTask;
        }

        // Sceneの各種コンポーネント取得用のインターフェース
        public T GetSceneComponent<T>() {
            return gameObject.GetComponent<T>();
        }
    }
}