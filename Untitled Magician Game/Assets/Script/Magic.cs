using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Border"){ //벽에 부딛혔을 때 삭제
            Debug.Log("벽에 부딛힘 확인");
            this.gameObject.SetActive(false);
        }
    }
}
