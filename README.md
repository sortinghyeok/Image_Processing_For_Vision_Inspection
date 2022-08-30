# 픽셀 직접 접근 비전 알고리즘 적용 프로그램 개발

## 대용량 bmp 파일에 대한 로딩 로직 구현, 전체 이미지 세이브 로직 구현 완료 / 2022.08.23

완료
- 파일을 바이트 Stream으로 변경하고, 해당 Stream을 메모리 직접 접근을 통한 로우 레벨 수준의 분할 Reading하여 메모리가 처리 가능한 수준으로 바꾸어 로딩 성공

![image](https://user-images.githubusercontent.com/80696846/187153489-ae6e2f66-4648-4a9d-8f6d-87f70829dfa1.png)

## Morphology Dilation 연산 적용 완료, 확대, 축소 기능 적용 개발 완료 / 2022.08.24

완료
- 픽셀 패딩 밀리는 현상 수정 완료,
- 모폴로지 연산 중 팽창 구현 완료

![image](https://user-images.githubusercontent.com/80696846/187153700-5586d86a-fe5f-44d1-81f6-582ad16a2bd0.png)

![image](https://user-images.githubusercontent.com/80696846/187153444-cf27ff3c-eab4-47a1-89ab-2015d861d676.png)

이슈
- 확대, 축소 시 저장 기능에 문제 있음
- 모폴로지 침식 연산 구현 요망

## Morphology Erosion 연산 적용 완료 / 2022.08.25

완료
- 모폴로지 연산 중 침식(축소) 구현 완료

![image](https://user-images.githubusercontent.com/80696846/187153586-d223fe68-4c57-43ee-be9d-59d80d054f98.png)

이슈
- 오리지널 비트맵의 저장시 저장 안되는 이슈 발견

## Histogram Equalization, Otsu Binarization 연산 적용 완료 / 2022.08.26

완료
- 히스토그램 평활화와 오츠 이진화 기능 구현 완료

![image](https://user-images.githubusercontent.com/80696846/187153766-56467f2b-f607-463c-9601-e3b246d188ba.png)

![image](https://user-images.githubusercontent.com/80696846/187153824-01ccb0c5-a318-4989-92c9-ccfbfdd10c05.png)

이슈
- Gaussian Filter 기능 로직 완료했으나 Blur의 느낌이 아님. 색이 단순히 어두워지는 느낌. 추후 로직을 살펴봐야할 필요 있음
- 연산 미처리한 원데이터 저장 불가 이슈 잔존, 연산 추가 개발 후 추후 수정하도록 함

## Morphology Erosion, Parallel Processing 중 이슈 발견 / 2022.08.29

이슈
- 모폴로지 침식 중 픽셀의 우측만이 수그러드는 현상 발견, 코드 구현 중 커널의 중심이 정가운데가 아닌 좌측에 있는 것을 확인하고 이에 대한 수정 요망
- 병렬 처리 시도 시 CPU 점유율이 98퍼센트까지 올라가고 프로그램은 응답이 없는 현상이 계속 

![image](https://user-images.githubusercontent.com/80696846/187153961-877e655e-1105-4fe1-a2e8-5706f2bf75ea.png)

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

![image](https://user-images.githubusercontent.com/80696846/187154278-64f072c3-8961-4e4c-8122-bed3f8454f3d.png)

이슈
- 히스토그램 평활화에 병렬화 로직 추가 필요
- 라플라스 필터 추가 구현 예정

## Laplace Filter 기능(Edge Detection) 구현 완료 / 2022.08.30

완료
- 라플라스 필터에 대한 구현 완료

![image](https://user-images.githubusercontent.com/80696846/187328167-427e0127-af39-4f5f-8edc-80fd275d1f3c.png)

이슈
- 라플라스 필터에 병렬화 적용 예정
- 해당 라플라스 필터 로직은 이미지의 경계 검출은 가능하나, bitmap 화소에 255 혹은 0의 값을 대입하므로 픽셀 간 상대적 차이를 반영하지 못함

## 모폴로지 침식 중 픽셀의 우측만이 수그러드는 현상 수정 완료 / 2022.08.30

완료
- Morphology Pointer를 자동으로 1씩 늘어나 픽셀 별로 모두 적용되는 것을 유도했으나, padding 1~3bytes에 대하여도 이것이 적용되어 우측 여유 공간의 수축이 일어났던 것으로 확인
- Pixel에 대한 접근시 해당 Row의 Stride를 갱신하는 것으로 정확한 line의 매핑이 가능해져, 해당 이슈에 대한 수정이 완료됨

![image](https://user-images.githubusercontent.com/80696846/187332712-6694b5f2-d5e9-4030-8b36-3f6db17f759d.png)

![image](https://user-images.githubusercontent.com/80696846/187332644-4c78387e-9b0e-4496-85d8-88d8cc88cef5.png)

## Laplace Filter 병렬화 구현 완료 / 2022.08.30

완료
- 라플라스 필터에 대한 병렬화 적용 완료

이슈
- 병렬화 적용으로 성능이 개선되었으나, 여전히 약 30초 정도의 시간 소요 중임. 프로세스와 비례하여 속도가 빨라지지 않는 원인 파악 필요
-> 백그라운드에 실행되고 있는 타 프로그램 때문인 것으로 확인. 약 10~12배 성능 개선되었으므로 CPU 갯수와 비례함
