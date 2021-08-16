using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace RollABall.Scripts
{
    public class CollectableRotator : MonoBehaviour
    {
        private const string PLAYER_TAG = "Player";

        private static Subject<Unit> onPickUpCollectedSubject;
        public static IObservable<Unit> OnPickUpCollected => onPickUpCollectedSubject = new Subject<Unit>();


        // Start is called before the first frame update
        private void Start()
        {
            Observable.EveryUpdate()
                .Subscribe(OnUpdateListener)
                .AddTo(this);
            
            this.OnTriggerEnterAsObservable()
                .Subscribe(OnTriggerEnterLister)
                .AddTo(this);
        }

        private void OnUpdateListener(long obj)
        {
            transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        }

        private void OnTriggerEnterLister(Collider other)
        {
            if (!other.gameObject.CompareTag(PLAYER_TAG)) return;
            gameObject.SetActive(false);
            onPickUpCollectedSubject.OnNext(Unit.Default);
        }
    }
}