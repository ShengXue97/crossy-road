﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterMovement : MonoBehaviour
{
    public GameObject ScorePopup;
    public Animator anim;
    public Transform movePoint;
    public LayerMask whatStopsMovement;
    public LayerMask vehicleLayer;
    public LayerMask waterLayer;
    public LayerMask logLayer;
    public float moveSpeed = 20f;
    [SerializeField]
    public TextMeshProUGUI scoreText;
    [SerializeField]
    public TextMeshProUGUI maxScoreText;
    private MeshRenderer meshRenderer;

    private Hashtable foods;

    public int zPos = 0;
    private int zMax = 0;
    private int highscore = 0;
    private int score = 0;
    private bool canMoveHorizontal = true;
    private bool canMoveVertical = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
        movePoint.parent = null;
        if (PlayerPrefs.HasKey("highscore"))
        {
            highscore = PlayerPrefs.GetInt("highscore");
            maxScoreText.text = "Top: " + highscore;
        }

        foods = new Hashtable();

        foods.Add("apple(Clone)", 5);
        foods.Add("appleHalf(Clone)", 2);
        foods.Add("banana(Clone)", 5);
        foods.Add("beet(Clone)", 5);
        foods.Add("bread(Clone)", 5);
        foods.Add("broccoli(Clone)", 5);
        foods.Add("cabbage(Clone)", 20);
        foods.Add("carrot(Clone)", 5);
        foods.Add("cauliflower(Clone)", 20);
        foods.Add("cherries(Clone)", 5);
        foods.Add("coconutHalf(Clone)", 5);
        foods.Add("corn(Clone)", 5);
        foods.Add("egg(Clone)", 5);
        foods.Add("eggHalf(Clone)", 2);
        foods.Add("fish(Clone)", 10);
        foods.Add("grapes(Clone)", 5);
        foods.Add("oange(Clone)", 5);
        foods.Add("pepper(Clone)", 5);
        foods.Add("pineapple(Clone)", 20);
        foods.Add("pumpkin(Clone)", 30);
        foods.Add("strawberry(Clone)", 5);
        foods.Add("watermelon(Clone)", 30);


    }

    // Update is called once per frame
    void Update()
    {
        if (!meshRenderer.isVisible && Time.timeSinceLevelLoad > 0.1)
        {
            //Dead if player is not visible by any cameras
            // EditorUtility.DisplayDialog("Info", "You died", "Ok");
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 0f)
        {
            //Prevents holding down arrow keys to move, must release
            canMoveHorizontal = true;
        }

        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 0f)
        {
            //Prevents holding down arrow keys to move, must release
            canMoveVertical = true;
        }

        if (Physics.OverlapSphere(gameObject.transform.position, 0.2f, waterLayer).Length != 0)
        {
            if (Physics.OverlapSphere(gameObject.transform.position, 0.5f, logLayer).Length == 0)
            {
                //If on water but not on logs, die!
                //EditorUtility.DisplayDialog("Info", "You died", "Ok");
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }


        if (Physics.OverlapSphere(movePoint.position, 0.2f, vehicleLayer).Length != 0)
        {
            //EditorUtility.DisplayDialog("Info", "You died", "Ok");
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, 200f * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05F)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f && canMoveHorizontal)
            {
                canMoveHorizontal = false;
                if (Physics.OverlapSphere(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal") * 2, 0f, 0f), 0.2f, whatStopsMovement).Length == 0)
                {
                    if (Input.GetAxisRaw("Horizontal") == -1f)
                    {
                        gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
                    }
                    else
                    {
                        gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    anim.SetBool("Hop", true);
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal") * 2, 0f, 0f);
                }
                if (Physics.OverlapSphere(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal") * 2, 0f, 0f), 0.2f, vehicleLayer).Length != 0)
                {
                    //EditorUtility.DisplayDialog("Info", "You died", "Ok");
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f && canMoveVertical)
            {
                canMoveVertical = false;
                if (Physics.OverlapSphere(movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical") * 2), 0.2f, whatStopsMovement).Length == 0)
                {
                    if (Input.GetAxisRaw("Vertical") == -1f)
                    {
                        zPos -= 1;
                        gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else
                    {
                        zPos += 1;
                        if (zPos > zMax)
                        {
                            score += 1;
                            zMax = zPos;
                            scoreText.text = "Score: " + score;
                            if (score > highscore)
                            {
                                PlayerPrefs.SetInt("highscore", score);
                                highscore = score;
                                maxScoreText.text = "Top: " + score;
                            }
                        }
                        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    anim.SetBool("Hop", true);
                    movePoint.position += new Vector3(0f, 0f, Input.GetAxisRaw("Vertical") * 2);
                }
                if (Physics.OverlapSphere(movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical") * 2), 0.2f, vehicleLayer).Length != 0)
                {
                    //EditorUtility.DisplayDialog("Info", "You died", "Ok");
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                }
            }
            else
            {
                anim.SetBool("Hop", false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (foods.ContainsKey(other.name))
        {
            GameObject scorePopupObj = Instantiate(ScorePopup, new Vector3(transform.position.x + 37f, transform.position.y + 3f, transform.position.z), Quaternion.identity);
            scorePopupObj.GetComponent<TextMeshPro>().text = ((int)foods[other.name]).ToString();
            score += (int)foods[other.name];
            Destroy(other.gameObject);
        }

        else if (other.tag == "Snail")
        {
            GameObject scorePopupObj = Instantiate(ScorePopup, new Vector3(transform.position.x + 37f, transform.position.y + 3f, transform.position.z), Quaternion.identity);
            scorePopupObj.GetComponent<TextMeshPro>().text = "15";
            score += 15;
            Destroy(other.gameObject);
        }

        scoreText.text = "Score: " + score;
        if (score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            highscore = score;
            maxScoreText.text = "Top: " + score;
        }

    }

    public void changeScore(int scoreDiff)
    {
        GameObject scorePopupObj = Instantiate(ScorePopup, new Vector3(transform.position.x + 37f, transform.position.y + 3f, transform.position.z), Quaternion.identity);
        scorePopupObj.GetComponent<TextMeshPro>().text = scoreDiff.ToString();
        score += scoreDiff;
        scoreText.text = "Score: " + score;
        if (score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            highscore = score;
            maxScoreText.text = "Top: " + score;
        }
    }
}

