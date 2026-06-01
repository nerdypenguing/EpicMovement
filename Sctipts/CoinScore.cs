using UnityEngine;
using TMPro;

public class CoinScore : MonoBehaviour
{

    public float score;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText.text = score.ToString();
        scoreText.text = "Score: " + score;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            score++;
            scoreText.text = "Score: " + score;
            Destroy(other.gameObject);
        }
    }
}