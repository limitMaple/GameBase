using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ClapperBoard.Managers.PageBase {
    public class PageChangerStarter : MonoBehaviour {
        [Header("生成GameObjectのルート")] [SerializeField]
        private Transform _root;

        [Header("初期シーン")] [SerializeField] private AssetReferenceGameObject _initScene;

        [SerializeReference] [SubclassSelector]
        private IPageContext _initPageContext;

        private BasePageChanger<IPage> _pageChanger;

        private void Start() {
            DontDestroyOnLoad(this);
            _pageChanger = new BasePageChanger<IPage>(_root);
            _pageChanger.ProcessPage(
                new PrefabPageBluePrint(
                    _initScene, _initPageContext, null
                )).Forget();
        }
    }
}