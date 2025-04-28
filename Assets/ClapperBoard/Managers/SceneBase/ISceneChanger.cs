using System.Threading;
using Cysharp.Threading.Tasks;
namespace ClapperBoard.Managers.SceneBase {
    /// <summary>
    /// Sceneの遷移処理を外部から操作するインターフェース。
    /// </summary>
    public interface ISceneChanger {
        // State自体に渡す、Sceneの遷移処理を行うインターフェース。
        // これをStateクラス自体に渡すことで、初期化時を除いて循環依存ではない設計を実現できる
        
        // ここでのTSceneは具象的なクラスを扱うのではなく、
        // ヘッダーがあるだとか、ISceneChangerが取り扱うScene全てが備えている性質を指定する
        public UniTask ProcessScene(ISceneBluePrint nextBlueprint,CancellationToken ct = default);
        public UniTask BackScene(CancellationToken ct = default);
        public void SetVisible(bool isVisible);
    }
}