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
    public float parallaxEffectX; // X�� �з����� ����
    public float parallaxEffectY; // Y�� �з����� ����
    public float smoothSpeed = 0.1f;
    public override void OnCreate()
    {
        base.OnCreate();

        // Ÿ�ϸ��� ���� ���� ���
        var bounds = GetComponent<TilemapRenderer>().bounds;
        // ���� ī�޶� ��������
        cameraTransform = Camera.main?.transform;
        if (cameraTransform == null)
        {
            Debug.LogError("Main Camera not found!");
        }

        // ���� ��ġ ����
        startPositionX = 0f;
        startPositionY = 0f;

        cameraStartPosX = 3.04f;
        cameraStartPosY = 1.7f;
    }
    public override void AfterStepAlways()
    {
        base.AfterStepAlways();

        // Ÿ�ϸ� ũ�� ����
        var bounds = GetComponent<TilemapRenderer>().bounds;

        // ī�޶� ������ �ٽ� �õ�
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
