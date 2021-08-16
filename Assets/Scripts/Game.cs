using UniRx;
using UnityEngine;

namespace RollABall.Scripts
{
    public class Game : MonoBehaviour
    {
        #region Private Fields

        /// <summary>
        /// Serializable fields
        /// </summary>
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        #endregion





        #region Public Fields
        
        public ReactiveProperty<int> Count = new ReactiveProperty<int>();
        
        #endregion





        #region Monobehaviour Events

        // Start is called before the first frame update
        private void Start()
        {
            MessageBroker.Default.Receive<ItemCollected>()
                .Subscribe(_ => ++Count.Value)
                .AddTo(_disposable);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
        
        #endregion
    }
}