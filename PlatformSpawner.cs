using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//오브젝트를 계속 생성 파괴하는 것은 성능을 많이 요구하기 때문에, 오브젝트 풀링 기능을 사용해
//오브젝트들을 필요할때마다 생성하는 것이 아니라, 미리 생성해둔 오브젝트들을 계속 재사용
//발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab; //생성할 발판의 원본 프리팹
    public int count = 3; //생성할 발판 수

    public float timeBetSpawnMin = 1.25f; //다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 2.25f; //다음 배치까지의 시간 간격 최댓값
    private float timeBetSpawn; //다음 배치까지의 시간 간격

    public float yMin = -3.5f; //배치할 위치의 최소 y값
    public float yMax = 1.5f; //배치할 위치의 최대 y값
    private float xPos = 20f; //배치할 위치의 x값

    private GameObject[] platforms; //미리 생성한 발판들
    private int currentIndex = 0; //사용할 현재 순번의 발판

    //초반에 생성한 발판을 화면 밖에 숨겨둘 위치
    private Vector2 poolPosition = new Vector2(0, -25);
    private float lastSpawnTime; //마지막 배치 시점

    // Start is called before the first frame update
    void Start()
    {
        //count 만큼의 공간을 가지는 새로운 발판 배열 생성
        platforms = new GameObject[count];

        //count 만큼 루프하면서 발판 생성
        for(int i = 0; i < count; i++)
        {
            //platformPrefab을 원본으로 새 발판을 poolPosition 위치에 복제 생성
            //생성된 발판을 platform 배열에 할당
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
        }

        //마지막 배치 시점 초기화
        lastSpawnTime = 0f;
        //다음번 배치까지의 시간 간격을 0으로 초기화
        timeBetSpawn = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //게임오버 상태에서는 동작하지 않음
        if (GameManager.instance.isGameover)
        {
            return;
        }

        //마지막 배치 시점에서 timeBetSpawn 이상 시간이 흘렀다면
        if (Time.time >= lastSpawnTime + timeBetSpawn)
        {
            //기록된 마지막 배치 시점을 현재 시점으로 갱신
            lastSpawnTime = Time.time;

            //다음 배치까지의 시간 간격을 timeBetSpawnMin, timeBetSpawnMax 사이에서 랜덤 설정
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            //배치할 위치의 높이를 yMin과 yMax 사이에서 랜덤 설정
            float yPos = Random.Range(yMin, yMax);

            //사용할 현재 순번의 발판 게임 오브젝트를 비활성화하고 즉시 다시 활성화
            //이때 발판의 Platform 컴포넌트의 OnEnable 메서드가 실행됨 -> OnEnable 메서드를 실행함으로 함정의 재배치가 이루어짐
            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);

            //현재 순번의 발판을 화면 오른쪽에 재배치
            platforms[currentIndex].transform.position = new Vector2(xPos, yPos);
            //순번 넘기기
            currentIndex++;

            //마지막 순번에 도달했다면 순번을 리셋
            if (currentIndex >= count)
            {
                currentIndex = 0;
            }
        }
    }
}
