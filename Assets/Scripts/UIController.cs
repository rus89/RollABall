using System;
using System.Linq;
using TMPro;
using UniRx;
using Unity.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RollABall.Scripts
{
    public class UIController : MonoBehaviour
    {
        #region Private Fields

        /// <summary>
        /// Serializable fields
        /// </summary>
        [SerializeField] private Game _game;
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private TextMeshProUGUI _gameOverText;
        [SerializeField] private GameObject _endGameMenu;
        [SerializeField] private Button _restartButton;
        
        #endregion





        #region Monobehaviour Events

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
        private void OnValidate()
        {
            _game = GameObject.Find("GameController").GetComponent<Game>();
            var allUIGameObjects = gameObject.Descendants();
            _countText = allUIGameObjects.First(x => x.name.Contains("CountText")).GetComponent<TextMeshProUGUI>();
            _gameOverText = allUIGameObjects.First(x => x.name.Contains("GameOverText")).GetComponent<TextMeshProUGUI>();
            _endGameMenu = allUIGameObjects.First(x => x.name.Contains("EndGameMenu"));
            _restartButton = allUIGameObjects.First(x => x.name.Contains("RestartButton")).GetComponent<Button>();
        }
#endif

        private void Start()
        {
            _game.Count
                .Subscribe(OnCountChanged)
                .AddTo(this);
            
            _game.Count
                .Where(i => i == 12)
                .Subscribe(OnWinTextListener)
                .AddTo(this);
            
            _restartButton
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    _endGameMenu.SetActive(false);
                })
                .AddTo(this);
        }
        
        #endregion





        #region Event Listeners

        private void OnWinTextListener(int count)
        {
            _gameOverText.text = "YOU WON";

            Observable.Timer(TimeSpan.FromSeconds(2f))
                .Subscribe(_ =>
                {
                    _endGameMenu.SetActive(true);
                })
                .AddTo(this);
        }

        private void OnCountChanged(int count)
        {
            _countText.text = "Count: " + count;
        }
        
        #endregion
    }
}
