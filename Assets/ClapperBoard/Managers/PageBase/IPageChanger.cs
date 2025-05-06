using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ClapperBoard.Managers.PageBase {
    /// <summary>
    ///     Sceneの遷移処理を外部から操作するインターフェース。
    /// </summary>
    public interface IPageChanger {
        Transform root { get; }
        // State自体に渡す、Sceneの遷移処理を行うインターフェース。
        // これをStateクラス自体に渡すことで、初期化時を除いて循環依存ではない設計を実現できる

        // ここでのTSceneは具象的なクラスを扱うのではなく、
        // ヘッダーがあるだとか、ISceneChangerが取り扱うScene全てが備えている性質を指定する
        public UniTask ProcessPage(IPageBluePrint nextBlueprint, CancellationToken ct = default);
        public UniTask BackPage(CancellationToken ct = default);
    }
}