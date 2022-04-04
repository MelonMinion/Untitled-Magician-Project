using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    public float speed;
    public float curDashDelay;
    public float maxDashDelay;
    public float nowDashTime;
    public float curFireDelay;
    public float maxFireDelay;
    public float fireSpeed;
    float h, v;
    bool isDash = false;
    public GameObject fireObject;
    Camera cam;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        cam = GetComponent<Camera>();
    }
    private void Update() {

        if(Input.GetKeyDown(KeyCode.Space)){ //대쉬 키 입력
            isDash = true;
            nowDashTime = curDashDelay;
        }

        Move();
        dashCount();
        Dash();
        fireCount();
        Fire();
    }

    void Move() { //캐릭터 움직임
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 rayLine = new Vector3(Input.GetAxisRaw("Horizontal") * 70, Input.GetAxisRaw("Vertical") * 100, 0);
        Debug.DrawRay(transform.position, rayLine, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, rayLine, 1, LayerMask.GetMask("Border"));

        if(rayHit.collider != null){
            Debug.Log(rayHit.collider.name);
        }

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;
    }

    void dashCount() { //캐릭터가 최근에 대쉬를 쓴 시간 계산
        curDashDelay += Time.deltaTime;
    }

    void Dash() {
        if(!isDash) //대쉬 중이 아니고
            return;

        if(curDashDelay < maxDashDelay) //대쉬 쿨타임이 넘었을 경우
            return;

        speed = 1500;
        if(curDashDelay - nowDashTime >= 0.1f){ //대쉬를 쓴 시점에서 부터 0.1초간 대쉬
            speed = 300;
            curDashDelay = 0;
            isDash = false;
        }
    }

    void fireCount() {
        curFireDelay += Time.deltaTime; //캐릭터가 최근에 공격을 한 시간 계산
    }
    
    void Fire() {
        if(!Input.GetKey(KeyCode.Mouse0)) //캐릭터가 왼클릭을 하면서
            return;

        if(isDash) //캐릭터가 대쉬 중이 아니고
            return;

        if(curFireDelay < maxFireDelay) //공격 가능 시간일 경우
            return;

        Vector3 mPosition = Input.mousePosition; //마우스 포지션 가져오기
        Vector3 oPosition = transform.position; //캐릭터 포지션 가져오기
        mPosition.z = oPosition.z - Camera.main.transform.position.z;
        Vector3 target = Camera.main.ScreenToWorldPoint(mPosition); //투사체가 향할 목표 계산
        float dy = target.y - oPosition.y;
        float dx = target.x - oPosition.x;
        float rotateDegree =  Mathf.Atan2(dy, dx) * Mathf.Rad2Deg; //투사체의 각도 계산

        GameObject fire = Instantiate(fireObject, transform.position, Quaternion.Euler(0f, 0f, rotateDegree)); //투사체를 캐릭터 위치에 계산한 각도로 복사
        Rigidbody2D fireRigid = fire.GetComponent<Rigidbody2D>();
        fireRigid.AddForce(new Vector3(dx, dy, 0).normalized * fireSpeed, ForceMode2D.Impulse); //투사체에 fireSpeed의 속도로 힘 주기

        curFireDelay = 0;
    }
}
