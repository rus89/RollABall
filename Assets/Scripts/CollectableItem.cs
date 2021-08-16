using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace RollABall.Scripts
{
    public sealed class ItemCollected {}
    
    public sealed class CollectableItem : MonoBehaviour
    {
        #region Private Fields

        /// <summary>
        /// Non-Serializable fields
        /// </summary>
        private readonly Vector3 _rotationDirection = new Vector3(15, 30, 45);

        #endregion





        #region Monobehaviour Events
        
        // Start is called before the first frame update
        private void Start()
        {
            this.UpdateAsObservable()
                .Subscribe(_ => OnUpdateListener())
                .AddTo(this);
            
            this.OnTriggerEnterAsObservable()
                .Subscribe(OnTriggerEnterLister)
                .AddTo(this);
        }

        #endregion




        #region Event Listeners

        private void OnUpdateListener()
        {
            transform.Rotate(_rotationDirection * Time.deltaTime);
        }

        private void OnTriggerEnterLister(Collider otherCollider)
        {
            if (!otherCollider.CompareTag(Constants.Tags.PLAYER)) return;
            gameObject.SetActive(false);
            MessageBroker.Default.Publish(new ItemCollected());
        }

        #endregion
    }
}