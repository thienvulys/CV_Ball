using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private TMP_Text points;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject redBall;
    [SerializeField] private GameObject greenBall;
    [SerializeField] private GameObject blueBall;

    private Color color;
    private List<Color> colorList;

    private int cnt;
    private float angle;
    float turnCalmVelocity;

    private void Start()
    {
        cnt = 0;
        angle = 0f;
        colorList = new List<Color>();
        colorList.Add(Color.red);
        colorList.Add(Color.green);
        colorList.Add(Color.blue);
        GetComponent<Renderer>().material.color = colorList[Random.Range(0, 3)];
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            angle -= 1 * Mathf.Deg2Rad;
        }
        if (Input.GetKey(KeyCode.C))
        {
            angle += 1 * Mathf.Deg2Rad;
        }
        cam.transform.localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 0f);
        cam.transform.position = new Vector3(2 * Mathf.Cos(-angle - 60 * Mathf.Deg2Rad) + transform.position.x, 1.5f, 2 * Mathf.Sin(-angle - 60 * Mathf.Deg2Rad) + transform.position.z);
        transform.position = new Vector3(transform.position.x + Mathf.Sin(angle) * Input.GetAxis("Vertical") *
            moveSpeed * Time.deltaTime + Mathf.Cos(angle) * Input.GetAxis("Horizontal") *
            moveSpeed * Time.deltaTime, 1, transform.position.z - Mathf.Sin(angle) * Input.GetAxis("Horizontal") *
            moveSpeed * Time.deltaTime + Mathf.Cos(angle) * Input.GetAxis("Vertical") *
            moveSpeed * Time.deltaTime);
        if (cnt == 10)
            Win();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(GetComponent<Renderer>().material.color + " " + other.gameObject.GetComponent<Renderer>().material.color);
        if(other.gameObject.tag == "ball")
        {
            if (GetComponent<Renderer>().material.color.Equals(other.gameObject.GetComponent<Renderer>().material.color))
            {
                //while(true)
                //{
                //    other.gameObject.transform.position = new Vector3(RandomDouble(), 0.875f, RandomDouble());
                //    if ((other.gameObject.transform.position.z - transform.position.z) * 
                //        (other.gameObject.transform.position.z - transform.position.z) + 
                //        (other.gameObject.transform.position.x - transform.position.x) * 
                //        (other.gameObject.transform.position.x - transform.position.x) > 25)
                //    break;
                //}    
                //other.gameObject.SetActive(false);
                Destroy(other.gameObject);
                color = GetComponent<Renderer>().material.color;
                StartCoroutine("Respawn", 1f);
                while (true)
                {
                    Color cur = GetComponent<Renderer>().material.color;
                    GetComponent<Renderer>().material.color = colorList[Random.Range(0, 3)];
                    if (!cur.Equals(GetComponent<Renderer>().material.color))
                        break;
                }
                points.text = "Point: " + (++cnt);
            }
            else
                Lose();
        }
        if(other.gameObject.tag == "wall")
        {
            Lose();
        }
    }
    IEnumerator Respawn(float spawnDelay)
    {
        yield return new WaitForSeconds(spawnDelay);
        if (color == Color.red)
            Instantiate(redBall);
        if (color == Color.green)
            Instantiate(greenBall);
        if (color == Color.blue)
            Instantiate(blueBall);
    }
    private float RandomDouble() => Random.Range(-7f, 7f);

    private void Lose()
    {
        SceneManager.LoadScene("Lose");
    }

    private void Win()
    {
        SceneManager.LoadScene("Win");
    }
}
