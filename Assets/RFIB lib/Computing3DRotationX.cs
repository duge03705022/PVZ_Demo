using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Computing3DRotationX {

	private string getPositionStr(int contacedSurfacePosition) {
		if(contacedSurfacePosition==1) return "DOWN";
		if(contacedSurfacePosition==2) return "FRONT";
		if(contacedSurfacePosition==3) return "UP";
		if(contacedSurfacePosition==4) return "BACK";
		if(contacedSurfacePosition==5) return "RIGHT";
		if(contacedSurfacePosition==6) return "LEFT";
		return null;
	}

	public int[] Computing3DRotation(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int[] tmpIs = {-1,-1,-1,-1};

		//print(StackWay1+","+BlockSurfaceID2+","+contacedSurfacePosition+","+X1+","+Y1+","+Z1);

		if(StackWay1==123456) tmpIs = Computing3DRotation_123456(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==153642) tmpIs = Computing3DRotation_153642(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==143265) tmpIs = Computing3DRotation_143265(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==163524) tmpIs = Computing3DRotation_163524(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		//[1]2[3]456 => 2645 => 111144  
		//[1]5[3]642 => 5264 => 442244
		//[1]4[3]265 => 4526 => 
		//[1]6[3]524 => 6452

		//1UP 2RIGHT 3DOWN 4LEFT
		//123456 111144
		if(StackWay1==234156) tmpIs = Computing3DRotation_234156(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==254613) tmpIs = Computing3DRotation_254613(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==214365) tmpIs = Computing3DRotation_214365(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==264531) tmpIs = Computing3DRotation_264531(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		//[2]3[4]156 => 3516
		//[2]5[4]613 => 5163
		//[2]1[4]365 => 1635
		//[2]6[4]531 => 6351

		if(StackWay1==341256) tmpIs = Computing3DRotation_341256(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==351624) tmpIs = Computing3DRotation_351624(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==321465) tmpIs = Computing3DRotation_321465(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==361542) tmpIs = Computing3DRotation_361542(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		//[3]4[1]256 => 4526
		//[3]5[1]624 => 5264
		//[3]2[1]465 => 2645
		//[3]6[1]542 => 6452

		if(StackWay1==526431) tmpIs = Computing3DRotation_526431(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==536142) tmpIs = Computing3DRotation_536142(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==546213) tmpIs = Computing3DRotation_546213(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==516324) tmpIs = Computing3DRotation_516324(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		//[5]2[6]431 => 2341
		//[5]3[6]142 => 3412
		//[5]4[6]213 => 4123
		//[5]1[6]324 => 1234

		if(StackWay1==645231) tmpIs = Computing3DRotation_645231(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==635124) tmpIs = Computing3DRotation_635124(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==625413) tmpIs = Computing3DRotation_625413(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==615342) tmpIs = Computing3DRotation_615342(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		//[6]4[5]231 => 4321
		//[6]3[5]124 => 3214
		//[6]2[5]413 => 2143
		//[6]1[5]342 => 1432

		if(StackWay1==412356) tmpIs = Computing3DRotation_412356(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==452631) tmpIs = Computing3DRotation_452631(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==432165) tmpIs = Computing3DRotation_432165(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		if(StackWay1==462513) tmpIs = Computing3DRotation_462513(StackWay1,BlockSurfaceID2,contacedSurfacePosition,X1,Y1,Z1);
		//[4]1[2]356 => 1536
		//[4]5[2]631 => 5361
		//[4]3[2]165 => 3615
		//[4]6[2]513 => 6153


		if(tmpIs[3]==-1) {	
			Debug.Log("[Sys Mesg] No 3D Rotation Information. ");
			Debug.Log("StackWay1:"+StackWay1);
			Debug.Log("BlockSurfaceID2:"+BlockSurfaceID2);
			Debug.Log("contacedSurfacePosition:"+contacedSurfacePosition);
			Debug.Log("X1:"+X1);
			Debug.Log("Y1:"+Y1);
			Debug.Log("Z1:"+Z1);
			Application.Quit();
		}			
		return tmpIs;
	}
	private int[] Computing3DRotation_123456(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;

		if(StackWay1==123456) { //SURFACE1-FACING:DOWN
			if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 				
				if(BlockSurfaceID2==1) StackWay2=234156; 
				if(BlockSurfaceID2==2) StackWay2=341256; 
				if(BlockSurfaceID2==3) StackWay2=412356; 
				if(BlockSurfaceID2==4) StackWay2=123456; 
				if(BlockSurfaceID2==5) StackWay2=264531; 
				if(BlockSurfaceID2==6) StackWay2=452631;  
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 				
				if(BlockSurfaceID2==1) StackWay2=412356; 
				if(BlockSurfaceID2==2) StackWay2=123456; 
				if(BlockSurfaceID2==3) StackWay2=234156; 
				if(BlockSurfaceID2==4) StackWay2=341256; 
				if(BlockSurfaceID2==5) StackWay2=452631; 
				if(BlockSurfaceID2==6) StackWay2=264531;  
			} else if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1;

				if(BlockSurfaceID2==1) StackWay2=123456; 
				if(BlockSurfaceID2==2) StackWay2=234156; 
				if(BlockSurfaceID2==3) StackWay2=341256; 
				if(BlockSurfaceID2==4) StackWay2=412356; 
				if(BlockSurfaceID2==5) StackWay2=526431; 
				if(BlockSurfaceID2==6) StackWay2=645231; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=341256; 
				if(BlockSurfaceID2==2) StackWay2=412356; 
				if(BlockSurfaceID2==3) StackWay2=123456; 
				if(BlockSurfaceID2==4) StackWay2=234156; 
				if(BlockSurfaceID2==5) StackWay2=645231; 
				if(BlockSurfaceID2==6) StackWay2=526431; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=625413; 
				if(BlockSurfaceID2==2) StackWay2=635124; 
				if(BlockSurfaceID2==3) StackWay2=645231; 
				if(BlockSurfaceID2==4) StackWay2=615342; 
				if(BlockSurfaceID2==5) StackWay2=123456; 
				if(BlockSurfaceID2==6) StackWay2=143265; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=645231; 
				if(BlockSurfaceID2==2) StackWay2=615342; 
				if(BlockSurfaceID2==3) StackWay2=625413; 
				if(BlockSurfaceID2==4) StackWay2=635124; 
				if(BlockSurfaceID2==5) StackWay2=143265; 
				if(BlockSurfaceID2==6) StackWay2=123456; 
			}
		}		
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_153642(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==153642) { 
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=163524; 
				if(BlockSurfaceID2==2) StackWay2=264531; 
				if(BlockSurfaceID2==3) StackWay2=361542; 
				if(BlockSurfaceID2==4) StackWay2=462513; 
				if(BlockSurfaceID2==5) StackWay2=516324; 
				if(BlockSurfaceID2==6) StackWay2=615342; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=351624; 
				if(BlockSurfaceID2==2) StackWay2=452631; 
				if(BlockSurfaceID2==3) StackWay2=153642; 
				if(BlockSurfaceID2==4) StackWay2=254613; 
				if(BlockSurfaceID2==5) StackWay2=635124; 
				if(BlockSurfaceID2==6) StackWay2=536142; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=452631; 
				if(BlockSurfaceID2==2) StackWay2=153642; 
				if(BlockSurfaceID2==3) StackWay2=254613; 
				if(BlockSurfaceID2==4) StackWay2=351624; 
				if(BlockSurfaceID2==5) StackWay2=432165; 
				if(BlockSurfaceID2==6) StackWay2=234156; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=254613; 
				if(BlockSurfaceID2==2) StackWay2=351624; 
				if(BlockSurfaceID2==3) StackWay2=452631; 
				if(BlockSurfaceID2==4) StackWay2=153642; 
				if(BlockSurfaceID2==5) StackWay2=234156; 
				if(BlockSurfaceID2==6) StackWay2=432165; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=635124; 
				if(BlockSurfaceID2==2) StackWay2=645231; 
				if(BlockSurfaceID2==3) StackWay2=615342; 
				if(BlockSurfaceID2==4) StackWay2=625413; 
				if(BlockSurfaceID2==5) StackWay2=163524; 
				if(BlockSurfaceID2==6) StackWay2=153642; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=615342; 
				if(BlockSurfaceID2==2) StackWay2=625413; 
				if(BlockSurfaceID2==3) StackWay2=635124; 
				if(BlockSurfaceID2==4) StackWay2=645231; 
				if(BlockSurfaceID2==5) StackWay2=153642; 
				if(BlockSurfaceID2==6) StackWay2=163524; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_143265(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==143265) { 
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=143265; 
				if(BlockSurfaceID2==2) StackWay2=214365; 
				if(BlockSurfaceID2==3) StackWay2=321465; 
				if(BlockSurfaceID2==4) StackWay2=432165; 
				if(BlockSurfaceID2==5) StackWay2=546213; 
				if(BlockSurfaceID2==6) StackWay2=625431; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=321465; 
				if(BlockSurfaceID2==2) StackWay2=432165; 
				if(BlockSurfaceID2==3) StackWay2=143265; 
				if(BlockSurfaceID2==4) StackWay2=214365; 
				if(BlockSurfaceID2==5) StackWay2=625413; 
				if(BlockSurfaceID2==6) StackWay2=546213; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=645231; 
				if(BlockSurfaceID2==2) StackWay2=615342; 
				if(BlockSurfaceID2==3) StackWay2=625413; 
				if(BlockSurfaceID2==4) StackWay2=635124; 
				if(BlockSurfaceID2==5) StackWay2=143265; 
				if(BlockSurfaceID2==6) StackWay2=123456; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=625413; 
				if(BlockSurfaceID2==2) StackWay2=635124; 
				if(BlockSurfaceID2==3) StackWay2=645231; 
				if(BlockSurfaceID2==4) StackWay2=615342; 
				if(BlockSurfaceID2==5) StackWay2=123456; 
				if(BlockSurfaceID2==6) StackWay2=143265; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=432165; 
				if(BlockSurfaceID2==2) StackWay2=143265; 
				if(BlockSurfaceID2==3) StackWay2=214365; 
				if(BlockSurfaceID2==4) StackWay2=321465; 
				if(BlockSurfaceID2==5) StackWay2=462513; 
				if(BlockSurfaceID2==6) StackWay2=254613; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=214365; 
				if(BlockSurfaceID2==2) StackWay2=321465; 
				if(BlockSurfaceID2==3) StackWay2=432165; 
				if(BlockSurfaceID2==4) StackWay2=143265; 
				if(BlockSurfaceID2==5) StackWay2=254613; 
				if(BlockSurfaceID2==6) StackWay2=462513; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_163524(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==163524) {
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=163524; 
				if(BlockSurfaceID2==2) StackWay2=264531; 
				if(BlockSurfaceID2==3) StackWay2=361542; 
				if(BlockSurfaceID2==4) StackWay2=462513; 
				if(BlockSurfaceID2==5) StackWay2=516324; 
				if(BlockSurfaceID2==6) StackWay2=615342; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=361542; 
				if(BlockSurfaceID2==2) StackWay2=462513; 
				if(BlockSurfaceID2==3) StackWay2=163524; 
				if(BlockSurfaceID2==4) StackWay2=264531; 
				if(BlockSurfaceID2==5) StackWay2=615342; 
				if(BlockSurfaceID2==6) StackWay2=516324; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=264531; 
				if(BlockSurfaceID2==2) StackWay2=361542; 
				if(BlockSurfaceID2==3) StackWay2=462513; 
				if(BlockSurfaceID2==4) StackWay2=163524; 
				if(BlockSurfaceID2==5) StackWay2=214365; 
				if(BlockSurfaceID2==6) StackWay2=412356; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=462513; 
				if(BlockSurfaceID2==2) StackWay2=163524; 
				if(BlockSurfaceID2==3) StackWay2=264531; 
				if(BlockSurfaceID2==4) StackWay2=361542; 
				if(BlockSurfaceID2==5) StackWay2=412356; 
				if(BlockSurfaceID2==6) StackWay2=214365; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=635124; 
				if(BlockSurfaceID2==2) StackWay2=645231; 
				if(BlockSurfaceID2==3) StackWay2=615341; 
				if(BlockSurfaceID2==4) StackWay2=625413; 
				if(BlockSurfaceID2==5) StackWay2=163524; 
				if(BlockSurfaceID2==6) StackWay2=153642; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=615342; 
				if(BlockSurfaceID2==2) StackWay2=625413; 
				if(BlockSurfaceID2==3) StackWay2=635124; 
				if(BlockSurfaceID2==4) StackWay2=645231; 
				if(BlockSurfaceID2==5) StackWay2=153642; 
				if(BlockSurfaceID2==6) StackWay2=163524; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}

	private int[] Computing3DRotation_254613(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==254613) { //SURFACE1-FACING:LEFT111
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=153642; 
				if(BlockSurfaceID2==2) StackWay2=254613; 
				if(BlockSurfaceID2==3) StackWay2=351624; 
				if(BlockSurfaceID2==4) StackWay2=452631; 
				if(BlockSurfaceID2==5) StackWay2=536142; 
				if(BlockSurfaceID2==6) StackWay2=635124; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=351624; 
				if(BlockSurfaceID2==2) StackWay2=452631; 
				if(BlockSurfaceID2==3) StackWay2=153642; 
				if(BlockSurfaceID2==4) StackWay2=254613; 
				if(BlockSurfaceID2==5) StackWay2=635124; 
				if(BlockSurfaceID2==6) StackWay2=536142; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=452631; 
				if(BlockSurfaceID2==2) StackWay2=153642; 
				if(BlockSurfaceID2==3) StackWay2=254613; 
				if(BlockSurfaceID2==4) StackWay2=351624; 
				if(BlockSurfaceID2==5) StackWay2=432165; 
				if(BlockSurfaceID2==6) StackWay2=234156; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=254613; 
				if(BlockSurfaceID2==2) StackWay2=351624; 
				if(BlockSurfaceID2==3) StackWay2=452631; 
				if(BlockSurfaceID2==4) StackWay2=153642; 
				if(BlockSurfaceID2==5) StackWay2=234156; 
				if(BlockSurfaceID2==6) StackWay2=432165; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=432165; 
				if(BlockSurfaceID2==2) StackWay2=143265; 
				if(BlockSurfaceID2==3) StackWay2=214365; 
				if(BlockSurfaceID2==4) StackWay2=321465; 
				if(BlockSurfaceID2==5) StackWay2=462513; 
				if(BlockSurfaceID2==6) StackWay2=254613; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=214365; 
				if(BlockSurfaceID2==2) StackWay2=321465; 
				if(BlockSurfaceID2==3) StackWay2=432165; 
				if(BlockSurfaceID2==4) StackWay2=143265; 
				if(BlockSurfaceID2==5) StackWay2=254613; 
				if(BlockSurfaceID2==6) StackWay2=432513; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_234156(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==234156) { //SURFACE1-FACING:FRONT
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=123456; 
				if(BlockSurfaceID2==2) StackWay2=234156; 
				if(BlockSurfaceID2==3) StackWay2=341256; 
				if(BlockSurfaceID2==4) StackWay2=412356; 
				if(BlockSurfaceID2==5) StackWay2=526431; 
				if(BlockSurfaceID2==6) StackWay2=645231; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=341256; 
				if(BlockSurfaceID2==2) StackWay2=412356; 
				if(BlockSurfaceID2==3) StackWay2=123456; 
				if(BlockSurfaceID2==4) StackWay2=234156; 
				if(BlockSurfaceID2==5) StackWay2=645231; 
				if(BlockSurfaceID2==6) StackWay2=526431; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=264531; 
				if(BlockSurfaceID2==2) StackWay2=361542; 
				if(BlockSurfaceID2==3) StackWay2=462531; 
				if(BlockSurfaceID2==4) StackWay2=163524; 
				if(BlockSurfaceID2==5) StackWay2=214365; 
				if(BlockSurfaceID2==6) StackWay2=412356; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=462513; 
				if(BlockSurfaceID2==2) StackWay2=163524; 
				if(BlockSurfaceID2==3) StackWay2=264531; 
				if(BlockSurfaceID2==4) StackWay2=361542; 
				if(BlockSurfaceID2==5) StackWay2=412356; 
				if(BlockSurfaceID2==6) StackWay2=214365; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=234156; 
				if(BlockSurfaceID2==2) StackWay2=341256; 
				if(BlockSurfaceID2==3) StackWay2=412356; 
				if(BlockSurfaceID2==4) StackWay2=123456; 
				if(BlockSurfaceID2==5) StackWay2=264531; 
				if(BlockSurfaceID2==6) StackWay2=452631; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=412356; 
				if(BlockSurfaceID2==2) StackWay2=123456; 
				if(BlockSurfaceID2==3) StackWay2=234156; 
				if(BlockSurfaceID2==4) StackWay2=341256; 
				if(BlockSurfaceID2==5) StackWay2=452631; 
				if(BlockSurfaceID2==6) StackWay2=264531; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}	
	private int[] Computing3DRotation_214365(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==214365) {
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=143265; 
				if(BlockSurfaceID2==2) StackWay2=214365; 
				if(BlockSurfaceID2==3) StackWay2=321465; 
				if(BlockSurfaceID2==4) StackWay2=432165; 
				if(BlockSurfaceID2==5) StackWay2=546213; 
				if(BlockSurfaceID2==6) StackWay2=625413; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=321465; 
				if(BlockSurfaceID2==2) StackWay2=432165; 
				if(BlockSurfaceID2==3) StackWay2=143265; 
				if(BlockSurfaceID2==4) StackWay2=214365; 
				if(BlockSurfaceID2==5) StackWay2=625413; 
				if(BlockSurfaceID2==6) StackWay2=546213; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=264531; 
				if(BlockSurfaceID2==2) StackWay2=361542; 
				if(BlockSurfaceID2==3) StackWay2=462513; 
				if(BlockSurfaceID2==4) StackWay2=163524; 
				if(BlockSurfaceID2==5) StackWay2=214365; 
				if(BlockSurfaceID2==6) StackWay2=412356; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=462513; 
				if(BlockSurfaceID2==2) StackWay2=163524; 
				if(BlockSurfaceID2==3) StackWay2=264531; 
				if(BlockSurfaceID2==4) StackWay2=361542; 
				if(BlockSurfaceID2==5) StackWay2=412356; 
				if(BlockSurfaceID2==6) StackWay2=214365; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=432165; 
				if(BlockSurfaceID2==2) StackWay2=143265; 
				if(BlockSurfaceID2==3) StackWay2=214365; 
				if(BlockSurfaceID2==4) StackWay2=321465; 
				if(BlockSurfaceID2==5) StackWay2=462513; 
				if(BlockSurfaceID2==6) StackWay2=254613; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=214365; 
				if(BlockSurfaceID2==2) StackWay2=321465; 
				if(BlockSurfaceID2==3) StackWay2=432165; 
				if(BlockSurfaceID2==4) StackWay2=143265; 
				if(BlockSurfaceID2==5) StackWay2=254613; 
				if(BlockSurfaceID2==6) StackWay2=462513; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_264531(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==264531) {
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=163524; 
				if(BlockSurfaceID2==2) StackWay2=264531; 
				if(BlockSurfaceID2==3) StackWay2=361542; 
				if(BlockSurfaceID2==4) StackWay2=462513; 
				if(BlockSurfaceID2==5) StackWay2=516324; 
				if(BlockSurfaceID2==6) StackWay2=615342; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=361542; 
				if(BlockSurfaceID2==2) StackWay2=462513; 
				if(BlockSurfaceID2==3) StackWay2=163524; 
				if(BlockSurfaceID2==4) StackWay2=264531; 
				if(BlockSurfaceID2==5) StackWay2=615342; 
				if(BlockSurfaceID2==6) StackWay2=516324; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=264531; 
				if(BlockSurfaceID2==2) StackWay2=361542; 
				if(BlockSurfaceID2==3) StackWay2=462513; 
				if(BlockSurfaceID2==4) StackWay2=163524; 
				if(BlockSurfaceID2==5) StackWay2=214365; 
				if(BlockSurfaceID2==6) StackWay2=412356; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=462513; 
				if(BlockSurfaceID2==2) StackWay2=163524; 
				if(BlockSurfaceID2==3) StackWay2=264531; 
				if(BlockSurfaceID2==4) StackWay2=361542; 
				if(BlockSurfaceID2==5) StackWay2=412356; 
				if(BlockSurfaceID2==6) StackWay2=214365; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=234156; 
				if(BlockSurfaceID2==2) StackWay2=341256; 
				if(BlockSurfaceID2==3) StackWay2=412356; 
				if(BlockSurfaceID2==4) StackWay2=123456; 
				if(BlockSurfaceID2==5) StackWay2=264531; 
				if(BlockSurfaceID2==6) StackWay2=452631; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=412356; 
				if(BlockSurfaceID2==2) StackWay2=123456; 
				if(BlockSurfaceID2==3) StackWay2=234156; 
				if(BlockSurfaceID2==4) StackWay2=341256; 
				if(BlockSurfaceID2==5) StackWay2=452631; 
				if(BlockSurfaceID2==6) StackWay2=264531; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}

	private int[] Computing3DRotation_341256(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==341256) { //SURFACE1-FACING:UP
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=123456; 
				if(BlockSurfaceID2==2) StackWay2=234156; 
				if(BlockSurfaceID2==3) StackWay2=341256; 
				if(BlockSurfaceID2==4) StackWay2=412356; 
				if(BlockSurfaceID2==5) StackWay2=526431; 
				if(BlockSurfaceID2==6) StackWay2=645231; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=341256; 
				if(BlockSurfaceID2==2) StackWay2=412356; 
				if(BlockSurfaceID2==3) StackWay2=123456; 
				if(BlockSurfaceID2==4) StackWay2=234156; 
				if(BlockSurfaceID2==5) StackWay2=645231; 
				if(BlockSurfaceID2==6) StackWay2=526431; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=526431; 
				if(BlockSurfaceID2==2) StackWay2=536142; 
				if(BlockSurfaceID2==3) StackWay2=546213; 
				if(BlockSurfaceID2==4) StackWay2=516324; 
				if(BlockSurfaceID2==5) StackWay2=321465; 
				if(BlockSurfaceID2==6) StackWay2=341256; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=546231; 
				if(BlockSurfaceID2==2) StackWay2=516324; 
				if(BlockSurfaceID2==3) StackWay2=526431; 
				if(BlockSurfaceID2==4) StackWay2=536142; 
				if(BlockSurfaceID2==5) StackWay2=341256; 
				if(BlockSurfaceID2==6) StackWay2=321465; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=234156; 
				if(BlockSurfaceID2==2) StackWay2=341256; 
				if(BlockSurfaceID2==3) StackWay2=412356; 
				if(BlockSurfaceID2==4) StackWay2=123456; 
				if(BlockSurfaceID2==5) StackWay2=264531; 
				if(BlockSurfaceID2==6) StackWay2=452631; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=412356; 
				if(BlockSurfaceID2==2) StackWay2=123456; 
				if(BlockSurfaceID2==3) StackWay2=234156; 
				if(BlockSurfaceID2==4) StackWay2=341256; 
				if(BlockSurfaceID2==5) StackWay2=452631; 
				if(BlockSurfaceID2==6) StackWay2=264531; 
			}				
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_351624(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==351624) { 
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=153642; 
				if(BlockSurfaceID2==2) StackWay2=254613; 
				if(BlockSurfaceID2==3) StackWay2=351624; 
				if(BlockSurfaceID2==4) StackWay2=452631; 
				if(BlockSurfaceID2==5) StackWay2=236142; 
				if(BlockSurfaceID2==6) StackWay2=635124; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=351624; 
				if(BlockSurfaceID2==2) StackWay2=452631; 
				if(BlockSurfaceID2==3) StackWay2=153642; 
				if(BlockSurfaceID2==4) StackWay2=254613; 
				if(BlockSurfaceID2==5) StackWay2=635124; 
				if(BlockSurfaceID2==6) StackWay2=536142; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=452631; 
				if(BlockSurfaceID2==2) StackWay2=153642; 
				if(BlockSurfaceID2==3) StackWay2=254613; 
				if(BlockSurfaceID2==4) StackWay2=351624; 
				if(BlockSurfaceID2==5) StackWay2=432165; 
				if(BlockSurfaceID2==6) StackWay2=234156; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=254613; 
				if(BlockSurfaceID2==2) StackWay2=351624; 
				if(BlockSurfaceID2==3) StackWay2=452631; 
				if(BlockSurfaceID2==4) StackWay2=153642; 
				if(BlockSurfaceID2==5) StackWay2=234156; 
				if(BlockSurfaceID2==6) StackWay2=432165; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=536142; 
				if(BlockSurfaceID2==2) StackWay2=546213; 
				if(BlockSurfaceID2==3) StackWay2=516324; 
				if(BlockSurfaceID2==4) StackWay2=526431; 
				if(BlockSurfaceID2==5) StackWay2=361542; 
				if(BlockSurfaceID2==6) StackWay2=341624; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=516324; 
				if(BlockSurfaceID2==2) StackWay2=526431; 
				if(BlockSurfaceID2==3) StackWay2=536142; 
				if(BlockSurfaceID2==4) StackWay2=546213; 
				if(BlockSurfaceID2==5) StackWay2=351624; 
				if(BlockSurfaceID2==6) StackWay2=361542; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_321465(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==321465) { 
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=143265; 
				if(BlockSurfaceID2==2) StackWay2=214365; 
				if(BlockSurfaceID2==3) StackWay2=321465; 
				if(BlockSurfaceID2==4) StackWay2=432165; 
				if(BlockSurfaceID2==5) StackWay2=546213; 
				if(BlockSurfaceID2==6) StackWay2=625413; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=321465; 
				if(BlockSurfaceID2==2) StackWay2=432165; 
				if(BlockSurfaceID2==3) StackWay2=143265; 
				if(BlockSurfaceID2==4) StackWay2=214365; 
				if(BlockSurfaceID2==5) StackWay2=625413; 
				if(BlockSurfaceID2==6) StackWay2=546213; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=526431; 
				if(BlockSurfaceID2==2) StackWay2=536142; 
				if(BlockSurfaceID2==3) StackWay2=546213; 
				if(BlockSurfaceID2==4) StackWay2=516324; 
				if(BlockSurfaceID2==5) StackWay2=321465; 
				if(BlockSurfaceID2==6) StackWay2=341256; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=546213; 
				if(BlockSurfaceID2==2) StackWay2=516324; 
				if(BlockSurfaceID2==3) StackWay2=526431; 
				if(BlockSurfaceID2==4) StackWay2=536142; 
				if(BlockSurfaceID2==5) StackWay2=341256; 
				if(BlockSurfaceID2==6) StackWay2=321465; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=432165; 
				if(BlockSurfaceID2==2) StackWay2=143265; 
				if(BlockSurfaceID2==3) StackWay2=214365; 
				if(BlockSurfaceID2==4) StackWay2=321465; 
				if(BlockSurfaceID2==5) StackWay2=462513; 
				if(BlockSurfaceID2==6) StackWay2=254613; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=214365; 
				if(BlockSurfaceID2==2) StackWay2=321465; 
				if(BlockSurfaceID2==3) StackWay2=432165; 
				if(BlockSurfaceID2==4) StackWay2=143265; 
				if(BlockSurfaceID2==5) StackWay2=254613; 
				if(BlockSurfaceID2==6) StackWay2=462513; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_361542(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==361542) { 
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=163524; 
				if(BlockSurfaceID2==2) StackWay2=264531; 
				if(BlockSurfaceID2==3) StackWay2=361542; 
				if(BlockSurfaceID2==4) StackWay2=462513; 
				if(BlockSurfaceID2==5) StackWay2=516324; 
				if(BlockSurfaceID2==6) StackWay2=615342; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=361542; 
				if(BlockSurfaceID2==2) StackWay2=462513; 
				if(BlockSurfaceID2==3) StackWay2=163524; 
				if(BlockSurfaceID2==4) StackWay2=264531; 
				if(BlockSurfaceID2==5) StackWay2=615342; 
				if(BlockSurfaceID2==6) StackWay2=516324; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=264531; 
				if(BlockSurfaceID2==2) StackWay2=361542; 
				if(BlockSurfaceID2==3) StackWay2=462513; 
				if(BlockSurfaceID2==4) StackWay2=163524; 
				if(BlockSurfaceID2==5) StackWay2=214365; 
				if(BlockSurfaceID2==6) StackWay2=412356; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=264531; 
				if(BlockSurfaceID2==2) StackWay2=361542; 
				if(BlockSurfaceID2==3) StackWay2=462513; 
				if(BlockSurfaceID2==4) StackWay2=163524; 
				if(BlockSurfaceID2==5) StackWay2=214365; 
				if(BlockSurfaceID2==6) StackWay2=412356; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=536142; 
				if(BlockSurfaceID2==2) StackWay2=546231; 
				if(BlockSurfaceID2==3) StackWay2=516324; 
				if(BlockSurfaceID2==4) StackWay2=526431; 
				if(BlockSurfaceID2==5) StackWay2=361542; 
				if(BlockSurfaceID2==6) StackWay2=351624; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=516324; 
				if(BlockSurfaceID2==2) StackWay2=526431; 
				if(BlockSurfaceID2==3) StackWay2=536142; 
				if(BlockSurfaceID2==4) StackWay2=546213; 
				if(BlockSurfaceID2==5) StackWay2=351624; 
				if(BlockSurfaceID2==6) StackWay2=361542; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}

	private int[] Computing3DRotation_526431(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==526431) { //SURFACE1-FACING:RIGHT
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=123456; 
				if(BlockSurfaceID2==2) StackWay2=234156; 
				if(BlockSurfaceID2==3) StackWay2=341256; 
				if(BlockSurfaceID2==4) StackWay2=412356; 
				if(BlockSurfaceID2==5) StackWay2=526431; 
				if(BlockSurfaceID2==6) StackWay2=645231; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=341256; 
				if(BlockSurfaceID2==2) StackWay2=412356; 
				if(BlockSurfaceID2==3) StackWay2=123456; 
				if(BlockSurfaceID2==4) StackWay2=234156; 
				if(BlockSurfaceID2==5) StackWay2=645231; 
				if(BlockSurfaceID2==6) StackWay2=526431; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=526431; 
				if(BlockSurfaceID2==2) StackWay2=236142; 
				if(BlockSurfaceID2==3) StackWay2=546213; 
				if(BlockSurfaceID2==4) StackWay2=516324; 
				if(BlockSurfaceID2==5) StackWay2=321465; 
				if(BlockSurfaceID2==6) StackWay2=341256; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=546231; 
				if(BlockSurfaceID2==2) StackWay2=516324; 
				if(BlockSurfaceID2==3) StackWay2=526431; 
				if(BlockSurfaceID2==4) StackWay2=536142; 
				if(BlockSurfaceID2==5) StackWay2=341256; 
				if(BlockSurfaceID2==6) StackWay2=321465; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=536142; 
				if(BlockSurfaceID2==2) StackWay2=546213; 
				if(BlockSurfaceID2==3) StackWay2=516324; 
				if(BlockSurfaceID2==4) StackWay2=526431; 
				if(BlockSurfaceID2==5) StackWay2=361542; 
				if(BlockSurfaceID2==6) StackWay2=351624; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=516324; 
				if(BlockSurfaceID2==2) StackWay2=526431; 
				if(BlockSurfaceID2==3) StackWay2=536142; 
				if(BlockSurfaceID2==4) StackWay2=546231; 
				if(BlockSurfaceID2==5) StackWay2=351624; 
				if(BlockSurfaceID2==6) StackWay2=361542; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_536142(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==536142) { 
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=153642; 
				if(BlockSurfaceID2==2) StackWay2=254613; 
				if(BlockSurfaceID2==3) StackWay2=351624; 
				if(BlockSurfaceID2==4) StackWay2=452631; 
				if(BlockSurfaceID2==5) StackWay2=536142; 
				if(BlockSurfaceID2==6) StackWay2=635124; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=351624; 
				if(BlockSurfaceID2==2) StackWay2=452631; 
				if(BlockSurfaceID2==3) StackWay2=153642; 
				if(BlockSurfaceID2==4) StackWay2=254613; 
				if(BlockSurfaceID2==5) StackWay2=635124; 
				if(BlockSurfaceID2==6) StackWay2=536142; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=526431; 
				if(BlockSurfaceID2==2) StackWay2=536142; 
				if(BlockSurfaceID2==3) StackWay2=546213; 
				if(BlockSurfaceID2==4) StackWay2=516324; 
				if(BlockSurfaceID2==5) StackWay2=321465; 
				if(BlockSurfaceID2==6) StackWay2=341256; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=546213; 
				if(BlockSurfaceID2==2) StackWay2=516324; 
				if(BlockSurfaceID2==3) StackWay2=526431; 
				if(BlockSurfaceID2==4) StackWay2=536142; 
				if(BlockSurfaceID2==5) StackWay2=341256; 
				if(BlockSurfaceID2==6) StackWay2=321465; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=536142; 
				if(BlockSurfaceID2==2) StackWay2=546213; 
				if(BlockSurfaceID2==3) StackWay2=516324; 
				if(BlockSurfaceID2==4) StackWay2=526431; 
				if(BlockSurfaceID2==5) StackWay2=361542; 
				if(BlockSurfaceID2==6) StackWay2=351624; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=516324; 
				if(BlockSurfaceID2==2) StackWay2=526431; 
				if(BlockSurfaceID2==3) StackWay2=536142; 
				if(BlockSurfaceID2==4) StackWay2=546213; 
				if(BlockSurfaceID2==5) StackWay2=351624; 
				if(BlockSurfaceID2==6) StackWay2=361542; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_546213(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==546213) { 
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=143265; 
				if(BlockSurfaceID2==2) StackWay2=214365; 
				if(BlockSurfaceID2==3) StackWay2=321465; 
				if(BlockSurfaceID2==4) StackWay2=432165; 
				if(BlockSurfaceID2==5) StackWay2=546213; 
				if(BlockSurfaceID2==6) StackWay2=625413; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=321465; 
				if(BlockSurfaceID2==2) StackWay2=432165; 
				if(BlockSurfaceID2==3) StackWay2=143265; 
				if(BlockSurfaceID2==4) StackWay2=214365; 
				if(BlockSurfaceID2==5) StackWay2=625413; 
				if(BlockSurfaceID2==6) StackWay2=546213; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=526431; 
				if(BlockSurfaceID2==2) StackWay2=536142; 
				if(BlockSurfaceID2==3) StackWay2=546213; 
				if(BlockSurfaceID2==4) StackWay2=516324; 
				if(BlockSurfaceID2==5) StackWay2=321465; 
				if(BlockSurfaceID2==6) StackWay2=341256; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=546213; 
				if(BlockSurfaceID2==2) StackWay2=516324; 
				if(BlockSurfaceID2==3) StackWay2=526431; 
				if(BlockSurfaceID2==4) StackWay2=536142; 
				if(BlockSurfaceID2==5) StackWay2=341256; 
				if(BlockSurfaceID2==6) StackWay2=321465; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=536142; 
				if(BlockSurfaceID2==2) StackWay2=546213; 
				if(BlockSurfaceID2==3) StackWay2=516324; 
				if(BlockSurfaceID2==4) StackWay2=526431; 
				if(BlockSurfaceID2==5) StackWay2=361542; 
				if(BlockSurfaceID2==6) StackWay2=351624; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=516324; 
				if(BlockSurfaceID2==2) StackWay2=526431; 
				if(BlockSurfaceID2==3) StackWay2=536142; 
				if(BlockSurfaceID2==4) StackWay2=546213; 
				if(BlockSurfaceID2==5) StackWay2=351624; 
				if(BlockSurfaceID2==6) StackWay2=361542; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_516324(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==516324) { 
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=163524; 
				if(BlockSurfaceID2==2) StackWay2=264531; 
				if(BlockSurfaceID2==3) StackWay2=361542; 
				if(BlockSurfaceID2==4) StackWay2=462513; 
				if(BlockSurfaceID2==5) StackWay2=516324; 
				if(BlockSurfaceID2==6) StackWay2=615342; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=361542; 
				if(BlockSurfaceID2==2) StackWay2=462513; 
				if(BlockSurfaceID2==3) StackWay2=163524; 
				if(BlockSurfaceID2==4) StackWay2=264531; 
				if(BlockSurfaceID2==5) StackWay2=615342; 
				if(BlockSurfaceID2==6) StackWay2=516324; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=526431; 
				if(BlockSurfaceID2==2) StackWay2=536142; 
				if(BlockSurfaceID2==3) StackWay2=546213; 
				if(BlockSurfaceID2==4) StackWay2=516324; 
				if(BlockSurfaceID2==5) StackWay2=321465; 
				if(BlockSurfaceID2==6) StackWay2=341256; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=516213; 
				if(BlockSurfaceID2==2) StackWay2=516324; 
				if(BlockSurfaceID2==3) StackWay2=526431; 
				if(BlockSurfaceID2==4) StackWay2=536142; 
				if(BlockSurfaceID2==5) StackWay2=341256; 
				if(BlockSurfaceID2==6) StackWay2=321465; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=536142; 
				if(BlockSurfaceID2==2) StackWay2=546213; 
				if(BlockSurfaceID2==3) StackWay2=516324; 
				if(BlockSurfaceID2==4) StackWay2=526431; 
				if(BlockSurfaceID2==5) StackWay2=361542; 
				if(BlockSurfaceID2==6) StackWay2=351624; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=516324; 
				if(BlockSurfaceID2==2) StackWay2=526431; 
				if(BlockSurfaceID2==3) StackWay2=536142; 
				if(BlockSurfaceID2==4) StackWay2=546213; 
				if(BlockSurfaceID2==5) StackWay2=351624; 
				if(BlockSurfaceID2==6) StackWay2=361542; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}

	private int[] Computing3DRotation_625413(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==625413) { //SURFACE1-FACING:LEFT111
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=143265; 
				if(BlockSurfaceID2==2) StackWay2=214365; 
				if(BlockSurfaceID2==3) StackWay2=321465; 
				if(BlockSurfaceID2==4) StackWay2=432165; 
				if(BlockSurfaceID2==5) StackWay2=546231; 
				if(BlockSurfaceID2==6) StackWay2=625413; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=321465; 
				if(BlockSurfaceID2==2) StackWay2=432165; 
				if(BlockSurfaceID2==3) StackWay2=143265; 
				if(BlockSurfaceID2==4) StackWay2=214365; 
				if(BlockSurfaceID2==5) StackWay2=625413; 
				if(BlockSurfaceID2==6) StackWay2=546213; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=645231; 
				if(BlockSurfaceID2==2) StackWay2=615342; 
				if(BlockSurfaceID2==3) StackWay2=625413; 
				if(BlockSurfaceID2==4) StackWay2=635124; 
				if(BlockSurfaceID2==5) StackWay2=143265; 
				if(BlockSurfaceID2==6) StackWay2=123456; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=625413; 
				if(BlockSurfaceID2==2) StackWay2=635124; 
				if(BlockSurfaceID2==3) StackWay2=655231; 
				if(BlockSurfaceID2==4) StackWay2=615342; 
				if(BlockSurfaceID2==5) StackWay2=123456; 
				if(BlockSurfaceID2==6) StackWay2=143265; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=635124; 
				if(BlockSurfaceID2==2) StackWay2=645231; 
				if(BlockSurfaceID2==3) StackWay2=615342; 
				if(BlockSurfaceID2==4) StackWay2=625413; 
				if(BlockSurfaceID2==5) StackWay2=163524; 
				if(BlockSurfaceID2==6) StackWay2=153642; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=615342; 
				if(BlockSurfaceID2==2) StackWay2=625413; 
				if(BlockSurfaceID2==3) StackWay2=365124; 
				if(BlockSurfaceID2==4) StackWay2=645231; 
				if(BlockSurfaceID2==5) StackWay2=153642; 
				if(BlockSurfaceID2==6) StackWay2=163524; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_645231(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==645231) { //SURFACE1-FACING:LEFT111
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=123456; 
				if(BlockSurfaceID2==2) StackWay2=234156; 
				if(BlockSurfaceID2==3) StackWay2=341256; 
				if(BlockSurfaceID2==4) StackWay2=412356; 
				if(BlockSurfaceID2==5) StackWay2=526431; 
				if(BlockSurfaceID2==6) StackWay2=645231; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=341256; 
				if(BlockSurfaceID2==2) StackWay2=412356; 
				if(BlockSurfaceID2==3) StackWay2=123456; 
				if(BlockSurfaceID2==4) StackWay2=234156; 
				if(BlockSurfaceID2==5) StackWay2=645231; 
				if(BlockSurfaceID2==6) StackWay2=526431; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=645231; 
				if(BlockSurfaceID2==2) StackWay2=615342; 
				if(BlockSurfaceID2==3) StackWay2=625413; 
				if(BlockSurfaceID2==4) StackWay2=635124; 
				if(BlockSurfaceID2==5) StackWay2=143265; 
				if(BlockSurfaceID2==6) StackWay2=123456; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=625413; 
				if(BlockSurfaceID2==2) StackWay2=635124; 
				if(BlockSurfaceID2==3) StackWay2=645231; 
				if(BlockSurfaceID2==4) StackWay2=615342; 
				if(BlockSurfaceID2==5) StackWay2=123456; 
				if(BlockSurfaceID2==6) StackWay2=143265; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=635124; 
				if(BlockSurfaceID2==2) StackWay2=645231; 
				if(BlockSurfaceID2==3) StackWay2=315342; 
				if(BlockSurfaceID2==4) StackWay2=625413; 
				if(BlockSurfaceID2==5) StackWay2=163524; 
				if(BlockSurfaceID2==6) StackWay2=153642; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=615342; 
				if(BlockSurfaceID2==2) StackWay2=625413; 
				if(BlockSurfaceID2==3) StackWay2=635124; 
				if(BlockSurfaceID2==4) StackWay2=645231; 
				if(BlockSurfaceID2==5) StackWay2=153642; 
				if(BlockSurfaceID2==6) StackWay2=163524; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}	
	private int[] Computing3DRotation_635124(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==635124) { 
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=153642; 
				if(BlockSurfaceID2==2) StackWay2=254613; 
				if(BlockSurfaceID2==3) StackWay2=351624; 
				if(BlockSurfaceID2==4) StackWay2=452631; 
				if(BlockSurfaceID2==5) StackWay2=536142; 
				if(BlockSurfaceID2==6) StackWay2=635124; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=351624; 
				if(BlockSurfaceID2==2) StackWay2=452631; 
				if(BlockSurfaceID2==3) StackWay2=153642; 
				if(BlockSurfaceID2==4) StackWay2=254613; 
				if(BlockSurfaceID2==5) StackWay2=635124; 
				if(BlockSurfaceID2==6) StackWay2=536142; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=645231; 
				if(BlockSurfaceID2==2) StackWay2=615342; 
				if(BlockSurfaceID2==3) StackWay2=625413; 
				if(BlockSurfaceID2==4) StackWay2=635124; 
				if(BlockSurfaceID2==5) StackWay2=143265; 
				if(BlockSurfaceID2==6) StackWay2=123456; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=625413; 
				if(BlockSurfaceID2==2) StackWay2=635124; 
				if(BlockSurfaceID2==3) StackWay2=645231; 
				if(BlockSurfaceID2==4) StackWay2=615342; 
				if(BlockSurfaceID2==5) StackWay2=123456; 
				if(BlockSurfaceID2==6) StackWay2=143265; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=635124; 
				if(BlockSurfaceID2==2) StackWay2=645231; 
				if(BlockSurfaceID2==3) StackWay2=615342; 
				if(BlockSurfaceID2==4) StackWay2=625413; 
				if(BlockSurfaceID2==5) StackWay2=163524; 
				if(BlockSurfaceID2==6) StackWay2=153642; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=615342; 
				if(BlockSurfaceID2==2) StackWay2=625413; 
				if(BlockSurfaceID2==3) StackWay2=635124; 
				if(BlockSurfaceID2==4) StackWay2=645231; 
				if(BlockSurfaceID2==5) StackWay2=153642; 
				if(BlockSurfaceID2==6) StackWay2=163524; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_615342(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==615342) { 
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=163524; 
				if(BlockSurfaceID2==2) StackWay2=264531; 
				if(BlockSurfaceID2==3) StackWay2=361542; 
				if(BlockSurfaceID2==4) StackWay2=462513; 
				if(BlockSurfaceID2==5) StackWay2=516324; 
				if(BlockSurfaceID2==6) StackWay2=615342; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=361542; 
				if(BlockSurfaceID2==2) StackWay2=462513; 
				if(BlockSurfaceID2==3) StackWay2=163524; 
				if(BlockSurfaceID2==4) StackWay2=264531; 
				if(BlockSurfaceID2==5) StackWay2=615342; 
				if(BlockSurfaceID2==6) StackWay2=516324; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=645231; 
				if(BlockSurfaceID2==2) StackWay2=615342; 
				if(BlockSurfaceID2==3) StackWay2=625413; 
				if(BlockSurfaceID2==4) StackWay2=635124; 
				if(BlockSurfaceID2==5) StackWay2=143265; 
				if(BlockSurfaceID2==6) StackWay2=123456; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=625413; 
				if(BlockSurfaceID2==2) StackWay2=635124; 
				if(BlockSurfaceID2==3) StackWay2=645231; 
				if(BlockSurfaceID2==4) StackWay2=615342; 
				if(BlockSurfaceID2==5) StackWay2=123456; 
				if(BlockSurfaceID2==6) StackWay2=143265; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=635124; 
				if(BlockSurfaceID2==2) StackWay2=645231; 
				if(BlockSurfaceID2==3) StackWay2=615342; 
				if(BlockSurfaceID2==4) StackWay2=625413; 
				if(BlockSurfaceID2==5) StackWay2=163524; 
				if(BlockSurfaceID2==6) StackWay2=153642; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=615342; 
				if(BlockSurfaceID2==2) StackWay2=625413; 
				if(BlockSurfaceID2==3) StackWay2=635124; 
				if(BlockSurfaceID2==4) StackWay2=645231; 
				if(BlockSurfaceID2==5) StackWay2=153642; 
				if(BlockSurfaceID2==6) StackWay2=163524; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}

	private int[] Computing3DRotation_412356(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==412356) { 
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=123456; 
				if(BlockSurfaceID2==2) StackWay2=234156; 
				if(BlockSurfaceID2==3) StackWay2=341256; 
				if(BlockSurfaceID2==4) StackWay2=412356; 
				if(BlockSurfaceID2==5) StackWay2=526431; 
				if(BlockSurfaceID2==6) StackWay2=645231; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=341256; 
				if(BlockSurfaceID2==2) StackWay2=412356; 
				if(BlockSurfaceID2==3) StackWay2=123456; 
				if(BlockSurfaceID2==4) StackWay2=234156; 
				if(BlockSurfaceID2==5) StackWay2=645231; 
				if(BlockSurfaceID2==6) StackWay2=526431; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=264531; 
				if(BlockSurfaceID2==2) StackWay2=361542; 
				if(BlockSurfaceID2==3) StackWay2=462513; 
				if(BlockSurfaceID2==4) StackWay2=163524; 
				if(BlockSurfaceID2==5) StackWay2=214365; 
				if(BlockSurfaceID2==6) StackWay2=412356; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=462513; 
				if(BlockSurfaceID2==2) StackWay2=163524; 
				if(BlockSurfaceID2==3) StackWay2=264531; 
				if(BlockSurfaceID2==4) StackWay2=361542; 
				if(BlockSurfaceID2==5) StackWay2=412356; 
				if(BlockSurfaceID2==6) StackWay2=214365; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=234156; 
				if(BlockSurfaceID2==2) StackWay2=341256; 
				if(BlockSurfaceID2==3) StackWay2=412356; 
				if(BlockSurfaceID2==4) StackWay2=123456; 
				if(BlockSurfaceID2==5) StackWay2=264531; 
				if(BlockSurfaceID2==6) StackWay2=452631; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=412356; 
				if(BlockSurfaceID2==2) StackWay2=123456; 
				if(BlockSurfaceID2==3) StackWay2=234156; 
				if(BlockSurfaceID2==4) StackWay2=341256; 
				if(BlockSurfaceID2==5) StackWay2=452631; 
				if(BlockSurfaceID2==6) StackWay2=264531; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_452631(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==452631) { 
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=153642; 
				if(BlockSurfaceID2==2) StackWay2=254613; 
				if(BlockSurfaceID2==3) StackWay2=351624; 
				if(BlockSurfaceID2==4) StackWay2=452631; 
				if(BlockSurfaceID2==5) StackWay2=536142; 
				if(BlockSurfaceID2==6) StackWay2=635124; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=351624; 
				if(BlockSurfaceID2==2) StackWay2=452631; 
				if(BlockSurfaceID2==3) StackWay2=153642; 
				if(BlockSurfaceID2==4) StackWay2=254613; 
				if(BlockSurfaceID2==5) StackWay2=635124; 
				if(BlockSurfaceID2==6) StackWay2=536142; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=452631; 
				if(BlockSurfaceID2==2) StackWay2=153642; 
				if(BlockSurfaceID2==3) StackWay2=254613; 
				if(BlockSurfaceID2==4) StackWay2=351624; 
				if(BlockSurfaceID2==5) StackWay2=432165; 
				if(BlockSurfaceID2==6) StackWay2=234156; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=254613; 
				if(BlockSurfaceID2==2) StackWay2=351624; 
				if(BlockSurfaceID2==3) StackWay2=452631; 
				if(BlockSurfaceID2==4) StackWay2=153642; 
				if(BlockSurfaceID2==5) StackWay2=234156; 
				if(BlockSurfaceID2==6) StackWay2=432165; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=234156; 
				if(BlockSurfaceID2==2) StackWay2=341256; 
				if(BlockSurfaceID2==3) StackWay2=412356; 
				if(BlockSurfaceID2==4) StackWay2=123456; 
				if(BlockSurfaceID2==5) StackWay2=264531; 
				if(BlockSurfaceID2==6) StackWay2=452631; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=412356; 
				if(BlockSurfaceID2==2) StackWay2=123456; 
				if(BlockSurfaceID2==3) StackWay2=234156; 
				if(BlockSurfaceID2==4) StackWay2=321256; 
				if(BlockSurfaceID2==5) StackWay2=452631; 
				if(BlockSurfaceID2==6) StackWay2=264531; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_432165(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==432165) { 
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=143265; 
				if(BlockSurfaceID2==2) StackWay2=214365; 
				if(BlockSurfaceID2==3) StackWay2=321465; 
				if(BlockSurfaceID2==4) StackWay2=432165; 
				if(BlockSurfaceID2==5) StackWay2=546213; 
				if(BlockSurfaceID2==6) StackWay2=625413; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=321465; 
				if(BlockSurfaceID2==2) StackWay2=432165; 
				if(BlockSurfaceID2==3) StackWay2=143265; 
				if(BlockSurfaceID2==4) StackWay2=214365; 
				if(BlockSurfaceID2==5) StackWay2=625413; 
				if(BlockSurfaceID2==6) StackWay2=546213; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=452631; 
				if(BlockSurfaceID2==2) StackWay2=153642; 
				if(BlockSurfaceID2==3) StackWay2=254613; 
				if(BlockSurfaceID2==4) StackWay2=351624; 
				if(BlockSurfaceID2==5) StackWay2=432165; 
				if(BlockSurfaceID2==6) StackWay2=234156; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=254613; 
				if(BlockSurfaceID2==2) StackWay2=351624; 
				if(BlockSurfaceID2==3) StackWay2=452631; 
				if(BlockSurfaceID2==4) StackWay2=153642; 
				if(BlockSurfaceID2==5) StackWay2=234156; 
				if(BlockSurfaceID2==6) StackWay2=432165; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=432165; 
				if(BlockSurfaceID2==2) StackWay2=143265; 
				if(BlockSurfaceID2==3) StackWay2=214365; 
				if(BlockSurfaceID2==4) StackWay2=321465; 
				if(BlockSurfaceID2==5) StackWay2=462513; 
				if(BlockSurfaceID2==6) StackWay2=254613; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=214365; 
				if(BlockSurfaceID2==2) StackWay2=321465; 
				if(BlockSurfaceID2==3) StackWay2=432165; 
				if(BlockSurfaceID2==4) StackWay2=143265; 
				if(BlockSurfaceID2==5) StackWay2=254613; 
				if(BlockSurfaceID2==6) StackWay2=462513; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
	private int[] Computing3DRotation_462513(int StackWay1, int BlockSurfaceID2, int contacedSurfacePosition, int X1, int Y1, int Z1) {
		int StackWay2=-1, X2=-1, Y2=-1, Z2=-1;
		if(StackWay1==462513) { 
			if(getPositionStr(contacedSurfacePosition)=="UP") { X2=X1; Y2=Y1; Z2=Z1+1; 
				if(BlockSurfaceID2==1) StackWay2=163524; 
				if(BlockSurfaceID2==2) StackWay2=264531; 
				if(BlockSurfaceID2==3) StackWay2=361542; 
				if(BlockSurfaceID2==4) StackWay2=462513; 
				if(BlockSurfaceID2==5) StackWay2=516324; 
				if(BlockSurfaceID2==6) StackWay2=615342; 
			} else if(getPositionStr(contacedSurfacePosition)=="DOWN") { X2=X1; Y2=Y1; Z2=Z1-1; 
				if(BlockSurfaceID2==1) StackWay2=361542; 
				if(BlockSurfaceID2==2) StackWay2=462513; 
				if(BlockSurfaceID2==3) StackWay2=163524; 
				if(BlockSurfaceID2==4) StackWay2=264531; 
				if(BlockSurfaceID2==5) StackWay2=615342; 
				if(BlockSurfaceID2==6) StackWay2=516324; 
			} else if(getPositionStr(contacedSurfacePosition)=="RIGHT") { X2=X1-1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=264531; 
				if(BlockSurfaceID2==2) StackWay2=361542; 
				if(BlockSurfaceID2==3) StackWay2=462513; 
				if(BlockSurfaceID2==4) StackWay2=163524; 
				if(BlockSurfaceID2==5) StackWay2=214365; 
				if(BlockSurfaceID2==6) StackWay2=412356; 
			} else if(getPositionStr(contacedSurfacePosition)=="LEFT") { X2=X1+1; Y2=Y1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=462513; 
				if(BlockSurfaceID2==2) StackWay2=163524; 
				if(BlockSurfaceID2==3) StackWay2=264531; 
				if(BlockSurfaceID2==4) StackWay2=361542; 
				if(BlockSurfaceID2==5) StackWay2=412356; 
				if(BlockSurfaceID2==6) StackWay2=214365; 
			} else if(getPositionStr(contacedSurfacePosition)=="FRONT") { X2=X1; Y2=Y1-1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=432165; 
				if(BlockSurfaceID2==2) StackWay2=143265; 
				if(BlockSurfaceID2==3) StackWay2=214365; 
				if(BlockSurfaceID2==4) StackWay2=321465; 
				if(BlockSurfaceID2==5) StackWay2=462513; 
				if(BlockSurfaceID2==6) StackWay2=254613; 
			} else if(getPositionStr(contacedSurfacePosition)=="BACK") { X2=X1; Y2=Y1+1; Z2=Z1; 
				if(BlockSurfaceID2==1) StackWay2=214365; 
				if(BlockSurfaceID2==2) StackWay2=321465; 
				if(BlockSurfaceID2==3) StackWay2=432165; 
				if(BlockSurfaceID2==4) StackWay2=143265; 
				if(BlockSurfaceID2==5) StackWay2=254613; 
				if(BlockSurfaceID2==6) StackWay2=462513; 
			}
		}
		int[] tmpIs = {X2,Y2,Z2,StackWay2};
		return tmpIs;
	}
}
 