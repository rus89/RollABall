using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RollABall.Scripts
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameController _gameController;
        [SerializeField] private TextMeshProUGUI _textMeshProUgui;
        [SerializeField] private GameObject _endGameMenuGameObject;
        [SerializeField] private TextMeshProUGUI _gameOverText;
        [SerializeField] private Button _restartButton;

        private void Start()
        {
            _gameController.Count
                .Subscribe(OnCountChanged)
                .AddTo(this);
            
            _gameController.Count
                .Where(i => i == 12)
                .Subscribe(OnWinTextListener)
                .AddTo(this);
            
            _restartButton
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    _endGameMenuGameObject.SetActive(false);
                })
                .AddTo(this);
        }

        private void OnWinTextListener(int count)
        {
            _gameOverText.text = "YOU WIN";

            Observable.Timer(TimeSpan.FromSeconds(2f))
                .Subscribe(_ =>
                {
                    _endGameMenuGameObject.SetActive(true);
                })
                .AddTo(this);
        }

        private void OnCountChanged(int count)
        {
            _textMeshProUgui.text = "Count: " + count;
        }
    }
}