using System;
using System.Threading;
using ClapperBoard.Managers.Transition;
using Cysharp.Threading.Tasks;
namespace ClapperBoard.Managers.SceneBase {
    /// <summary>
    /// Sceneを生成するためのタスクを示すインターフェース
    /// </summary>
    public interface ISceneBluePrint {
        public UniTask<TSceneBaseType> Instantiate<TSceneBaseType>(
            ISceneChanger sceneChanger,
            CancellationToken ct = default) where TSceneBaseType : IScene;
        public ITransition transition { get; set; }
    }
}