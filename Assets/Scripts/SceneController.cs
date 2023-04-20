using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneController : MonoBehaviour
{
    public const int gridRows = 2;
    public const int gridColumns = 4;
    public const float offsetX = 4f;
    public const float offsetY = 5f;

    [SerializeField] private MainCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh scoreLabel;
    [SerializeField] private TextMesh totalAttemptsLabel;

    private int _score = 0;
    private int _totalAttempts = 0;

    [SerializeField] private TextMesh timeLabel;
    private float _startTime;
    private bool _isGameFinished = false;


    private MainCard _firstRevealedCard;
    private MainCard _secondRevealedCard;


    public bool canReveal
    {
        get
        {
            return _secondRevealedCard == null;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        _startTime = Time.time;

        SetCardStartLayout();

    }

    private void SetCardStartLayout()
    {
        Vector3 startPosition = originalCard.transform.position;

        int[] cardNumbers = { 0, 0, 1, 1, 2, 2, 3, 3 };
        cardNumbers = ShuffleCards(cardNumbers);

        MainCard card;

        for (int i = 0; i < gridColumns; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                if (i == 0 && j == 0)
                    card = originalCard;
                else
                    card = Instantiate(originalCard);

                int index = j * gridColumns + i;
                int id = cardNumbers[index];
                card.ChangeSprite(id, images[id]);

                float xPos = startPosition.x + (i * offsetX);
                float yPos = startPosition.y + (j * offsetY);
                card.transform.position = new Vector3(xPos, yPos, startPosition.z);
            }
        }
    }

    private int[] ShuffleCards(int[] cardNumbers)
    {
        int[] shuffledCarNumbers = cardNumbers.Clone() as int[];

        for (int i = 0; i < shuffledCarNumbers.Length; i++)
        {
            int temp = shuffledCarNumbers[i];
            int r = UnityEngine.Random.Range(0, shuffledCarNumbers.Length);
            shuffledCarNumbers[i] = shuffledCarNumbers[r];
            shuffledCarNumbers[r] = temp;
        }

        return shuffledCarNumbers;
    }

    public void RevealCard(MainCard card)
    {
        if (_secondRevealedCard != null) return;

        if (_firstRevealedCard == null)
        {
            _firstRevealedCard = card;
        }
        else
        {
            _secondRevealedCard = card;
            StartCoroutine(CheckCardMatchCoroutine());
        }
    }

    private IEnumerator CheckCardMatchCoroutine()
    {

        if (_firstRevealedCard.Id == _secondRevealedCard.Id)
        {
            _score++;
            scoreLabel.text = "Score: " + _score;
        }
        else
        {
            yield return new WaitForSeconds(1f);

            _firstRevealedCard.Unreveal();
            _secondRevealedCard.Unreveal();
        }

        _totalAttempts++;
        totalAttemptsLabel.text = "Attempts: " + _totalAttempts;

        _firstRevealedCard = null;
        _secondRevealedCard = null;

        int cardsRemaining = gridRows * gridColumns - _score * 2;
        if (cardsRemaining == 0)
        {
            _isGameFinished = true;
        }

    }

    private void UpdateTimer()
    {
        if (_isGameFinished) return;

        float t = Time.time - _startTime;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");

        timeLabel.text = "Time: " + minutes + ":" + seconds;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        UpdateTimer();
    }
}
