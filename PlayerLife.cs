using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;
public class PlayerLife : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] TextMeshProUGUI text;
    public int playerHP = 100;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        text.text = $"HP: {playerHP}";
        if (playerHP <= 0)
        {
            Debug.Log("dead");
            Die();
        }
    }
    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        RestartLevel();
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
