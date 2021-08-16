using UniRx;
using UnityEngine;

namespace RollABall.Scripts
{
    public class GameController : MonoBehaviour
    {
        public ReactiveProperty<int> Count = new ReactiveProperty<int>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        // Start is called before the first frame update
        private void Start()
        {
            CollectableRotator.OnPickUpCollected
                .Subscribe(i => ++Count.Value)
                .AddTo(_disposable);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}