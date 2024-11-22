using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TilemapParallax : Obj
{
    private float startPositionX, startPositionY;
    private float cameraStartPosX, cameraStartPosY;
    private Transform cameraTransform;
    public TilemapRenderer MapTile;
    public float parallaxEffectX; // X축 패럴랙스 강도
    public float parallaxEffectY; // Y축 패럴랙스 강도
    public float smoothSpeed = 0.1f;
    public override void OnCreate()
    {
        base.OnCreate();

        // 타일맵의 폭과 높이 계산
        var bounds = GetComponent<TilemapRenderer>().bounds;
        // 메인 카메라 가져오기
        cameraTransform = Camera.main?.transform;
        if (cameraTransform == null)
        {
            Debug.LogError("Main Camera not found!");
        }

        // 시작 위치 저장
        startPositionX = 0f;
        startPositionY = 0f;

        cameraStartPosX = 3.04f;
        cameraStartPosY = 1.7f;
    }
    public override void AfterStepAlways()
    {
        base.AfterStepAlways();

        // 타일맵 크기 갱신
        var bounds = GetComponent<TilemapRenderer>().bounds;

        // 카메라가 없으면 다시 시도
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
            return;
        }
        else
        {
            float targetPositionX = startPositionX + (cameraTransform.position.x - cameraStartPosX) * parallaxEffectX;
            float targetPositionY = startPositionY + (cameraTransform.position.y - cameraStartPosY) * parallaxEffectY;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, new Vector3(targetPositionX, targetPositionY, transform.position.z), 0.6f);
            transform.position = smoothPosition;
        }


        
        
    }

}
