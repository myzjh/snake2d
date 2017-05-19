using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Head : MonoBehaviour {
	public GameObject body;//
	public Transform food;
	public Transform panel;
	public Text title;
	public Text log;
	public Text highest;
	private int score = 0;
	private float time = 0;
	public float movet = 0.5f;
	public static float step = 0.3f;//步距
	private List<Transform> bodys = new List<Transform>();//蛇身
	public static float stepX = 0, stepY = 0;//方向状态控制
	private int maxFood = 10, foodCount = 0;//食物数量控制
	private float width = 5.73f,height = 3.16f;
	private Vector2 startPos, endPos;
	public static bool slideUp = false, slideDown = false, slideRight = false, slideLeft = false;
	void Awake(){
		panel = panel.GetComponent<Transform> ();
		panel.gameObject.SetActive (false);
	}
	// Use this for initialization
	void Start () {
		stepX = step;
		title = title.GetComponent<Text> ();
		log = log.GetComponent<Text> ();
		highest = highest.GetComponent<Text> ();
		if (PlayerPrefs.HasKey ("highest"))
			highest.text = "highest:"+PlayerPrefs.GetInt ("highest").ToString();
	}
	
	// Update is called once per frame
	void Update () {
		//手势识别，LeanTouch

		//设置方向，不能走反方向
		if (Input.GetKey (KeyCode.W) && stepY != -step) {
			stepX = 0;
			stepY = step;
		} else if (Input.GetKey (KeyCode.A) && stepX != step) {
			stepX = -step;
			stepY = 0;
		} else if (Input.GetKey (KeyCode.S) && stepY != step) {
			stepX = 0;
			stepY = -step;
		} else if (Input.GetKey (KeyCode.D) && stepX != -step) {
			stepX = step;
			stepY = 0;
		}
		if(time > movet){
			//防按键太快导致方向出错
			if (stepX != 0)
				stepY = 0;

			CreateFoodRandom ();
			iTween.MoveTo(gameObject, new Vector3(transform.position.x + stepX,transform.position.y + stepY,0),0);
			MoveBody ();
			time = 0;
		}else{
			time += Time.deltaTime;
		}
		//根据坐标位置，出界就从另一半出现
		if (transform.position.x > width)
			transform.position = new Vector3 (-width, transform.position.y, 0);
		if (transform.position.x < -width) 
			transform.position = new Vector3 (width, transform.position.y, 0);
		if (transform.position.y > height) 
			transform.position = new Vector3 (transform.position.x, -height, 0);
		if (transform.position.y < -height) 
			transform.position = new Vector3 (transform.position.x, height, 0);

	}

	void MoveBody(){
		//第一块body跟随头移动
		if(bodys.Count>0){
			iTween.MoveTo (bodys[0].gameObject,transform.position,0);
		}
		//其他body块跟随第一块移动
		for(int i=1;i<bodys.Count;i++){
			iTween.MoveTo (bodys[i].gameObject,bodys[i-1].position,0);
		}
	}

	/// <summary>
	/// Creates the food random.
	/// </summary>
	void CreateFoodRandom(){
		if (foodCount < maxFood) {
			Instantiate (food, new Vector3 (Random.Range(-width,width), Random.Range(-height,height), 0), Quaternion.identity);
			foodCount++;
		}
	}

	void OnTriggerEnter(Collider col){
		//吃到食物
		if (col.tag == "food") {
			Destroy (col.gameObject);
			foodCount--;
			score++;
			log.text = "score:"+score.ToString ();
			//新建一个body块
			GameObject obj = Instantiate (body, new Vector3 (20f, 20f, 20f), Quaternion.identity) as GameObject;
			bodys.Add (obj.transform);
		} else if(col.tag == "tail"){
			Time.timeScale = 0;
			Debug.Log ("You are dead..");
			title.text = "You are dead..";
			panel.gameObject.SetActive (true);
			//保存最高记录
			if (!PlayerPrefs.HasKey ("highest") || (PlayerPrefs.HasKey ("highest") && score > PlayerPrefs.GetInt ("highest"))) {
				PlayerPrefs.SetInt ("highest", score);
				title.text = "New record!";
			}
			
		}

	}
	/// <summary>
	/// Speeds up.
	/// </summary>
	public void SpeedUp(){
		movet /= 1.2f;
	}
	/// <summary>
	/// Speeds down.
	/// </summary>
	public void SpeedDown(){
		movet *= 1.2f;
	}
	/// <summary>
	/// Res the life.
	/// </summary>
	public void ReLife(){
		panel.gameObject.SetActive (false);
		string name = SceneManager.GetActiveScene ().name;
		SceneManager.LoadScene (name);
		Time.timeScale = 1;
	}
	/// <summary>
	/// Exit this instance.
	/// </summary>
	public void Exit(){
		Application.Quit ();
	}
}
