using System.Threading;
using Cysharp.Threading.Tasks;

namespace ClapperBoard.Managers.PageBase {
    /// <summary>
    ///     Sceneそのものを実装するインターフェース。
    /// </summary>
    public interface IPage {
        // 表示前の読み込みタスク
        // ISceneContextはダウンキャストを前提に扱う
        // 初期化時に、遷移用のマネージャーを受け取る

        // 生成時に行う処理
        public UniTask Init(IPageContext context, IPageChanger pageChanger);

        // 実行時に行う処理
        public UniTask Run(CancellationToken ct);

        // 退出時に行う処理
        public UniTask Exit();

        // Sceneの各種コンポーネント取得用のインターフェース
        public T GetSceneComponent<T>();
    }
}