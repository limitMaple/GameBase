using System;
using System.Threading;
using Cysharp.Threading.Tasks;
namespace ClapperBoard.Managers.Transition {
    /// <summary>
    /// トランジションを示すインターフェース
    /// ダウンロード状況とかを取得し、反映する
    /// </summary>
    public interface ITransition:IProgress<float> {
        public UniTask Enter();
        public UniTask Complete();
    }
}