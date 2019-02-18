using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Rocket_Script : MonoBehaviour
{
    public Camera cam;
    public Text dist_UI;
    public Text vel_UI;
    public Text count_UI;


    private float thr_en;
    private float GRAV;
    private float vel_x;
    private float vel_y;
    private float angle;
    private float THRUST;
    private float angle_thrust;
    private float x_pos;
    private float y_pos;
    private Vector3 rocket_pos;
    private Transform left_thrust;
    private Transform main_thrust;
    private Transform right_thrust;
    private Transform heat_shield;
    private Transform storm;
    private Transform fade;
    private Transform dim;
    private BoxCollider2D ground_col;
    private BoxCollider2D storm_col;
    private int storm_count;

    private float heat_a;
    private int storm_dir;
    private float fade_a;

    private float altitude;
    private float vel_dis;
    private float land_angle;
    private int start;

    private int storm_active;

    private int end;

    private string next_scene;

    // Start is called before the first frame update
    void Start()
    {
        GRAV = -1/480f;
        vel_x = 0f;
        vel_y = 0f;
        angle = 0f;
        THRUST = 2/480f;
        thr_en = 0f;
        angle_thrust = 1f;
        x_pos = 0f;
        y_pos = 1540f/8f;
        rocket_pos.x = x_pos;
        rocket_pos.y = y_pos;
        rocket_pos.z = 0;

        left_thrust = this.gameObject.transform.GetChild(0);
        main_thrust = this.gameObject.transform.GetChild(1);
        right_thrust = this.gameObject.transform.GetChild(2);
        heat_shield = this.gameObject.transform.GetChild(3);
        storm = this.gameObject.transform.GetChild(4);
        fade = cam.transform.GetChild(1);
        storm_count = 0;

        storm_dir = Random.Range(-180, 0);
        storm.rotation = new Quaternion(0, 0, 1, (storm_dir-7)*Mathf.PI/180);

        heat_a = 0f;
        cam.transform.position = new Vector3 (0f, y_pos, -10f);

        altitude = Mathf.RoundToInt(rocket_pos.y + 3.5f);
        vel_dis = Mathf.RoundToInt(vel_y);
        dist_UI.text = "Altitude: " + altitude.ToString();
        vel_UI.text = "Velocity: " + vel_dis.ToString();


        right_thrust.transform.gameObject.SetActive(false);
        main_thrust.transform.gameObject.SetActive(false);
        left_thrust.transform.gameObject.SetActive(false);
        storm.transform.gameObject.SetActive(false);

        //storm_active = Mathf.RoundToInt(Random.Range(0f,1f));
        storm_active = PlayerPrefs.GetInt("rocket_level");
        start = 0;

        GameObject.Find("Rocket_Storm_Back").gameObject.SetActive(storm_active == 1);
        storm_col = GameObject.Find("Storm_Ground").GetComponent<BoxCollider2D>();
        storm_col.enabled = (storm_active == 1);
        GameObject.Find("Storm_Ground").gameObject.SetActive(storm_active == 1);
        ground_col = GameObject.Find("Ground").GetComponent<BoxCollider2D>();
        ground_col.enabled = (storm_active == 0);
        dim = cam.transform.GetChild(0);
        dim.gameObject.SetActive(storm_active == 1);

        fade_a = 0f;
        fade.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, fade_a);

        end = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        storm.transform.Translate(Vector3.right*1/2);
        if ((-16 > storm.position.x) || (storm.position.x > 16) || (-16 > storm.localPosition.y) || (storm.localPosition.y > 16))
        {
            storm_count += 1;
            storm.transform.Translate(Vector3.right * -30);
            if (storm_count == 6)
            {
                storm.position = new Vector3(0, rocket_pos.y, 0);
                storm_dir = Random.Range(-180, 0);
                storm.rotation = new Quaternion(0, 0, 1, (storm_dir - 7) * Mathf.PI / 180);
                storm.transform.Translate(Vector3.right * -15);
                storm_count = 0;
            }
        }

        if (rocket_pos.y >= 0 && rocket_pos.y <= 1540f / 8f) { cam.transform.position = new Vector3(0f, rocket_pos.y, -10f); }

        if (rocket_pos.y < 125)
        {
            start = 1;

            vel_x -= Mathf.Cos((storm_dir - 7) * Mathf.PI / 180) / 2500f * storm_active * (1-end);
            vel_y -= Mathf.Abs(Mathf.Sin((storm_dir - 7) * Mathf.PI / 180)/4000f) * storm_active*(1-end);

            if (Input.GetKey(KeyCode.UpArrow)) { thr_en = 1; }
            else { thr_en = 0; }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                angle -= angle_thrust;
                this.gameObject.transform.Rotate(Vector3.forward, 1f - end);
                storm.transform.Rotate(Vector3.forward, -1f + end);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                angle += angle_thrust;
                this.gameObject.transform.Rotate(Vector3.forward, -1f + end);
                storm.transform.Rotate(Vector3.forward, 1f - end);
            }
            heat_a -= 0.3f/60f;
            if (heat_a <= 0f) { heat_a = 0f; }

            right_thrust.transform.gameObject.SetActive(Input.GetKey(KeyCode.LeftArrow));
            main_thrust.transform.gameObject.SetActive(thr_en == 1);
            left_thrust.transform.gameObject.SetActive(Input.GetKey(KeyCode.RightArrow));
        }
        else if (start == 0){ if (rocket_pos.y <= 1050/8) { count_UI.text = ""; }
            else if (rocket_pos.y <= 1300/8) { count_UI.text = "Thrusters Enabled In\n1"; heat_a += 0.1f/60f; }
            else if (rocket_pos.y <= 1480/8)
            {
                count_UI.text = "Thrusters Enabled In\n2";
                heat_a += 0.1f/60f;
                storm.transform.gameObject.SetActive(true&&(storm_active == 1));
            }
         }
        
        heat_shield.GetComponent<SpriteRenderer>().color = new Color(1,1,1, heat_a);

        altitude = Mathf.RoundToInt(8f * (rocket_pos.y + 3.5f));
        vel_dis = Mathf.RoundToInt(200f*vel_y);
        if (end == 0)
        {
            dist_UI.text = "Altitude: " + altitude.ToString();
            vel_UI.text = "Velocity: " + vel_dis.ToString();
        }

        vel_x += Mathf.Sin(angle * Mathf.PI / 180) * THRUST * thr_en/5;
        vel_y += Mathf.Cos(angle * Mathf.PI / 180) * THRUST * thr_en + GRAV;

        x_pos += vel_x;
        y_pos += vel_y;

        rocket_pos.x = x_pos;
        rocket_pos.y = y_pos;
        this.gameObject.transform.position = rocket_pos;

        if (end == 1)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            fade_a += 1/200f;
            if (fade_a > 1f) { fade_a = 1f; SceneManager.LoadScene(next_scene);
                //SceneManager.LoadScene("game");
            }
            fade.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, fade_a);

        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if ((col.gameObject.name == "Ground") || (col.gameObject.name == "Storm_Ground"))
        {
            vel_UI.text = "Velocity: 0";
            dist_UI.text = "Altitude: 0";

            land_angle = angle % 45;
            vel_dis = Mathf.RoundToInt(8*vel_y);
            rocket_pos.y = -3.5f + vel_dis * 1 / 3 - Mathf.Abs(Mathf.Sin(angle * Mathf.PI / 180)) * 0.3f;

            if (rocket_pos.y >= -3.6f) { count_UI.text = "Perfect Landing!"; PlayerPrefs.SetInt("landing", 3); next_scene = "game"; }
            else if (rocket_pos.y >= -4.0f) { count_UI.text = "Successful Landing."; PlayerPrefs.SetInt("landing", 2); next_scene = "game"; }
            else if (rocket_pos.y >= -4.3f) { count_UI.text = "Poor Landing."; PlayerPrefs.SetInt("landing", 1); next_scene = "game"; }
            else { count_UI.text = "Critical Failure";  next_scene = "Rocket_mini";
            }

            Destroy(vel_UI);
            Destroy(dist_UI);

            GRAV = 0f;
            vel_x = 0f;
            vel_y = 0f;
            THRUST = 0f;
            x_pos = rocket_pos.x; y_pos = rocket_pos.y;
            
            this.gameObject.transform.position = rocket_pos;
            //vel_dis = 2;

            end = 1;

            right_thrust.transform.gameObject.SetActive(false);
            main_thrust.transform.gameObject.SetActive(false);
            left_thrust.transform.gameObject.SetActive(false);

            Destroy(col.collider);
        }

        if (col.gameObject.name == "BoundaryL" || col.gameObject.name == "BoundaryR")
        {
            count_UI.text = "Mission Failed:\nOff Course";
            Destroy(vel_UI);
            Destroy(dist_UI);

            GRAV = 0f;
            vel_x = 0f;
            vel_y = 0f;
            THRUST = 0f;

            end = 1;

            next_scene = "Rocket_mini";
        }
        if (col.gameObject.name == "BoundaryT")
        {
            count_UI.text = "Mission Failed:\nLeaving Orbit";
            Destroy(vel_UI);
            Destroy(dist_UI);

            GRAV = 0f;
            vel_x = 0f;
            vel_y = 0f;
            THRUST = 0f;

            end = 1;

            next_scene = "Rocket_mini";
        }
    }
}
