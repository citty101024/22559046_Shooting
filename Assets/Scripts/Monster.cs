using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float spd = 5.0f;
    Vector3 direct = Vector3.left;
    public GameObject prefabsExplosion;

    void Start()
    {
        int rndNum = Random.Range(0, 10);
        if (rndNum % 3 == 0)
        {
            GameObject target = GameObject.Find("Character");
            direct = target.transform.position - transform.position;
            direct.Normalize();
        }
    }

    private void Update()
    {
        transform.position = transform.position + direct * spd * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.tag == "Bullet")
        {
            GameObject gameManager = GameObject.Find("GameManager");
            ScoreManager scoreManager = gameManager.GetComponent<ScoreManager>();
            MonsterDropper dropper = GetComponent<MonsterDropper>();

            scoreManager.nowScore++;
            scoreManager.nowScoreUI.text = "Now Score : " + scoreManager.nowScore;

            if (dropper != null)
            {
                dropper.Drop();
            }

            if (scoreManager.nowScore > scoreManager.bestScore)
            {
                scoreManager.bestScore = scoreManager.nowScore;
                scoreManager.bestScoreUI.text = "Best Score : " + scoreManager.bestScore;
                PlayerPrefs.SetInt("bestscore", scoreManager.bestScore);
            }

            GameObject explosionObj = Instantiate(prefabsExplosion);
            explosionObj.transform.position = transform.position;

            Destroy(collision.gameObject);
            Destroy(gameObject);

        }
    }
    
    private void OnTriggerEnter(Collider wall)
    {
        if (wall.CompareTag("Wall"))
        {
            GameObject gameManager = GameObject.Find("GameManager");
            ScoreManager scoreManager = gameManager.GetComponent<ScoreManager>();

            scoreManager.nowScore--;
            if (scoreManager.nowScore < 0)
            {
                scoreManager.nowScore = 0;
            }
            scoreManager.nowScoreUI.text = "Now Score : " + scoreManager.nowScore;

            if (scoreManager.nowScore > scoreManager.bestScore)
            {
                scoreManager.bestScore = scoreManager.nowScore;
                scoreManager.bestScoreUI.text = "Best Score : " + scoreManager.bestScore;
                PlayerPrefs.SetInt("bestscore", scoreManager.bestScore);
            }
        }
    }
}
