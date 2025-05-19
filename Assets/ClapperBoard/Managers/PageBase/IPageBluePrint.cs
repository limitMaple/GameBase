using System.Threading;
using Cysharp.Threading.Tasks;

namespace ClapperBoard.Managers.PageBase {
    /// <summary>
    ///     Sceneを生成するためのタスクを示すインターフェース
    /// </summary>
    public interface IPageBluePrint {
        public UniTask<TSceneBaseType> Instantiate<TSceneBaseType>(
            IPageChanger pageChanger,
            CancellationToken ct = default) where TSceneBaseType : IPage;
    }
}