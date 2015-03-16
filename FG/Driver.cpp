#include <windows.h>
#include <stdio.h>
#include "conio.h"

int CALLBACK WinMain(
	HINSTANCE HInstance,
 	HINSTANCE hPrevInstance,
	LPSTR lpCmdLine,
	int nCmdShadow)
{
	MessageBox(0,"This is my first window","hi",MB_OK | MB_ICONINFORMATION);

	getch();

	HDC DeviceContext = BeginPaint(hWnd,&Paint);
	
	PatBlt(DeviceContext,X,Y,Width,Height,WHITENESS);

	return 0;
}