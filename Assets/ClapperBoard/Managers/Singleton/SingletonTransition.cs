using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Singleton;
using ClapperBoard.Managers.Transition;

namespace ClapperBoard.Managers.Singleton {
    // シングルトンに取得できるトランジション
    public class SingletonTransition : SingletonMonoBehaviour<SingletonTransition>, ITransition {
        [SerializeField] private GameObject _viewRoot;
        [SerializeField] private Slider _progressSlider;

        private void Awake() {
            DontDestroyOnLoad(this);
            _viewRoot.SetActive(false);
        }

        public UniTask Enter() {
            _viewRoot.SetActive(true);
            return UniTask.CompletedTask;
        }

        public UniTask Complete() {
            _viewRoot.SetActive(false);
            return UniTask.CompletedTask;
        }

        public void Report(float progress) {
            _progressSlider.value = progress;
        }
    }
}