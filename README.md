# AppleGame
 
그곳에서 유행했던 사과게임을 만들어 봄  
BGM 도 넣어서 진짜 비슷한 느낌이 듬  
배경도 추가하면 좋을 것 같음  

가능한 경우를 체크하고  
가능한 경우가 없으면 자동으로 불판을 가는 기능 추가 완료 함  

그리고 치트 모드도 만들었는데  
이건 그냥 가능한 경우를 시각적으로 보여주는 기능임  
F1을 누르면 치트모드가 활성/비활성화 됨  

가능한 경우 체크 로직  

1. 2차원 배열에 대해 부분합을 구함  
   <B>부분합 공식</B>  
   PreSum[&nbsp; i&nbsp; ][&nbsp; j&nbsp; ] = Original[&nbsp; i&nbsp; ][&nbsp; j&nbsp; ] + PreSum[&nbsp; i - 1&nbsp; , &nbsp; j&nbsp; ] + PreSum[&nbsp; i&nbsp; , &nbsp; j - 1&nbsp; ] - PreSum[&nbsp; i - 1&nbsp; , &nbsp; j - 1&nbsp; ]

2. 시작 인덱스와 끝 인덱스로 가상의 사각형을 만듬  

3. 해당 인덱스를 바탕으로 사각형의 합을 구함  
   <B>사각형의 합 공식</B>  
   Sum = PreSum[endPos.x, endPos.y] - PreSum[EndPos.x, StartPos.y - 1] - PreSum[StartPos.x - 1, EndPos.y] + PreSum[StartPos.x - 1, StartPos.y - 1]

* 참고 사항
  index에서 -1을 하면 IndexOutofRange 예외가 발생해서  
  PreSum 배열과 Original 배열의 행과 열에 +1 씩 해줌  
  그리고나서 0,0 좌표의 값을 1,1에 저장해주는 식으로 코드를 작성함

  이후에 인덱스로 Apple을 컨트롤 하는데  
  이때는 인덱스에서 -1을 해줘야 함



그냥 간단할 줄 알았는데  
갑자기 부분합을 쓸 줄은 몰랐음  
