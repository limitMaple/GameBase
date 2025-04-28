using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace ClapperBoard.Managers.Transition {
    // トランジションの実装例
    public class ExampleTransition : MonoBehaviour,ITransition {
        readonly Slider m_ProgressSlider;
        public UniTask Enter ()
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public UniTask Complete()
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }

        public void Report(float progress)
        {
            m_ProgressSlider.value = progress;
        }
    }
}