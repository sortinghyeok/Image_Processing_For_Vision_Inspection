#  영상처리 Algorithm 직접 구현을 이용한 비전 및 장비 시퀀스용 알고리즘 개발

- 본 Readme에 사용된 이미지는 pixabay 등에서 구한 free image입니다.
- 본 프로젝트 수행 중 실제로 사용한 이미지는 반도체 Circuit 및 Particle 등의 bmp 파일이었음을 밝힙니다.
- 모든 영상처리 솔루션은 영상처리 및 컴퓨터비전 라이브러리가 일체 사용되지 않았으며, 픽셀에 대해 메모리에 직접 접근하여 알고리즘을 구현하였습니다.
- 픽셀에 대한 메모리 접근은 unsafe 구문 내의 포인터를 이용하였으며, C++ 기반의 영상처리 서적을 참고했습니다.

## 대용량 bmp 파일에 대한 로딩 로직 구현, 전체 이미지 세이브 로직 구현 완료 / 2022.08.23

완료
- 파일을 바이트 Stream으로 변경하고, 해당 Stream을 메모리 직접 접근을 통한 로우 레벨 수준의 분할 Reading하여 메모리가 처리 가능한 수준으로 바꾸어 로딩 성공

![image](https://user-images.githubusercontent.com/80696846/187596495-f558de18-e87f-45ba-8e19-6c44dcb2d101.png)

## Morphology Dilation 연산 적용 완료, 확대, 축소 기능 적용 개발 완료 / 2022.08.24

완료
- 픽셀 패딩 밀리는 현상 수정 완료
- 모폴로지 연산 중 팽창 구현 완료

![image](https://user-images.githubusercontent.com/80696846/187596453-4baf8951-2213-4cdd-a5e1-18c743de045d.png)


이슈
- 확대, 축소 시 저장 기능에 문제 있음
- 모폴로지 침식 연산 구현 요망

## Morphology Erosion 연산 적용 완료 / 2022.08.25

완료
- 모폴로지 연산 중 침식(축소) 구현 완료

![image](https://user-images.githubusercontent.com/80696846/187596615-c8a6bed0-4e2b-47bf-bdab-747fcc0d0fbe.png)

이슈
- 오리지널 비트맵의 저장시 저장 안되는 이슈 발견

## Histogram Equalization, Otsu Binarization 연산 적용 완료 / 2022.08.26

완료
- 히스토그램 평활화와 오츠 이진화 기능 구현 완료

![image](https://user-images.githubusercontent.com/80696846/187596663-1b80bfef-0612-41b5-b171-014460e6f92c.png)

![image](https://user-images.githubusercontent.com/80696846/187596681-8644c518-03d8-4a6b-96e7-c0ceaeb6e0ae.png)

이슈
- Gaussian Filter 기능 로직 완료했으나 Blur가 적용되지 않고 단순히 색채가 어두워지기만 하여 로직의 문제가 있음
- 연산 미처리한 원데이터 저장 불가 이슈 잔존, 연산 추가 개발 후 추후 수정하도록 함

## Morphology Erosion, Parallel Processing 중 이슈 발견 / 2022.08.29

이슈
- 모폴로지 침식 중 픽셀의 우측만이 수그러드는 현상 발견, 코드 구현 중 커널의 중심이 정가운데가 아닌 좌측에 있는 것을 확인하고 이에 대한 수정 요망
- 병렬 처리 시도 시 CPU 점유율이 98퍼센트까지 올라가고 프로그램은 응답이 없는 현상이 계속 

## Morphology Erosion, Dilation, Gaussian Filter 로직 병렬화 적용 성공 / 2022.08.29

완료
- 모폴로지 침식, 팽창, 가우시안 필터 연산에 병렬화를 적용하여 7~10배 정도의 연산 속도 향상 확인

병렬 프로세싱 전

![image](https://user-images.githubusercontent.com/80696846/187317595-3c7ba302-a153-4bbb-a2a2-222e45e64c89.png)

![image](https://user-images.githubusercontent.com/80696846/187317424-5f65a369-d3f1-4473-bdce-fae15a46d060.png)

병렬 프로세싱 후

![image](https://user-images.githubusercontent.com/80696846/187317689-20a17805-64b4-4a50-a6b2-513e4f91fa68.png)

![image](https://user-images.githubusercontent.com/80696846/187317004-027a250a-6cae-454e-8f0d-d341cd6925e6.png)

![image](https://user-images.githubusercontent.com/80696846/187154171-737cd69d-5e86-4fc3-aeed-2b0b67a8e42e.png)

이슈
- 픽셀의 우측이 수그러드는 현상에 대하여 커널 로직 상 문제 없음 확인. 이미지 패딩에 의한 것일 확률이 있어 이에 대한 이슈 발생

## Gaussian Filter 기능(Blur) 수정 및 구현 완료 / 2022.08.29

완료
- 가우시안 필터에 대한 로직 수정 완료

![image](https://user-images.githubusercontent.com/80696846/187596873-3155ea99-92e5-48c0-bfa6-ca6586811147.png)

이슈
- 히스토그램 평활화에 병렬화 로직 추가 필요
- 라플라스 필터 추가 구현 예정

## Laplace Filter 기능(Edge Detection) 구현 완료 / 2022.08.30

완료
- 라플라스 필터에 대한 구현 완료

![image](https://user-images.githubusercontent.com/80696846/187596930-9455e8ca-c215-472a-b2ee-1d3a9023dd7a.png)

이슈
- 라플라스 필터에 병렬화 적용 예정
- 해당 라플라스 필터 로직은 이미지의 경계 검출은 가능하나, bitmap 화소에 255 혹은 0의 값을 대입하므로 픽셀 간 상대적 차이를 반영하지 못함

## 모폴로지 침식 중 픽셀의 우측만이 수그러드는 현상 수정 완료 / 2022.08.30

완료
- Morphology Pointer를 자동으로 1씩 늘어나 픽셀 별로 모두 적용되는 것을 유도했으나, padding 1~3bytes에 대하여도 이것이 적용되어 우측 여유 공간의 수축이 일어났던 것으로 확인
- Pixel에 대한 접근시 해당 Row의 Stride를 갱신하는 것으로 정확한 line의 매핑이 가능해져, 해당 이슈에 대한 수정이 완료됨

![image](https://user-images.githubusercontent.com/80696846/187596958-eded2a80-4b77-4f5a-bdf1-d5e7b9c16949.png)

![image](https://user-images.githubusercontent.com/80696846/187596977-3de92bde-3037-4fb1-b810-b0782c471a29.png)

## Laplace Filter 병렬화 구현 완료 / 2022.08.30

완료
- 라플라스 필터에 대한 병렬화 적용 완료

이슈
- 병렬화 적용으로 성능이 개선되었으나, 여전히 약 30초 정도의 시간 소요 중임. 프로세스와 비례하여 속도가 빨라지지 않는 원인 파악 필요
 --> 픽셀에 대한 여러가지 연산이 중첩된 후에 적용시에는 느려지는 것이 맞음. 오리지널 데이터에 적용시 약 10~12배 성능 개선되었으므로 CPU 갯수와 비례하여 개선 완료

## Histogram Equalization 일부 코드에 대한 병렬화 적용, 코드 가시성 개선 및 Refactoring / 2022.09.01

완료
- 히스토그램 누계 버퍼에 대한 병렬화 구현
- Vision Algorithm 하이어라키 개선
- Dummy Code 제거 등

이슈
- 히스토그램 병렬화 메인 코드에 대한 병렬화 적용이 되지 않아 버퍼에 대한 병렬화를 진행했음에도 타 알고리즘의 개선된 정도와는 현저하게 낮은 정도로 개선됨
