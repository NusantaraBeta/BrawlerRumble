using UnityEngine;

public class SS_MoveYSin : MonoBehaviour {
    public float Frequency = 1f;
    public float Magnitude = 10f;

    private float _startY;
    private float _time;   

	// Use this for initialization
	void Start () {
        _startY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        _time += Time.deltaTime * Frequency;
        Vector3 newPos = transform.position;

        newPos.y = _startY + Mathf.Sin(_time) * Magnitude;

        transform.position = newPos;
	}
}
