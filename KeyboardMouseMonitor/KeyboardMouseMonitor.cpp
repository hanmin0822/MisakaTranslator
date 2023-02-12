/*************************************************
*   由于.NET下使用全局键盘鼠标钩子总是会出现回调事件
*   没有反应的情况，故使用C++单写一个进程，C#中调用
*   这个EXE并进行通信，获取键鼠钩子事件。
*   启动传参为： 钩子类型（1=鼠标 2=键盘） 指定键值（见代码）
*   当指定键动作时输出一行返回值
*   使用时将这个项目单独编译，生成的EXE文件放入lib中即可
*************************************************/

#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include <windows.h>
#include <process.h>

using namespace std;

int keyCode;

LRESULT CALLBACK mymouse(int nCode, WPARAM wParam, LPARAM lParam)
{
	MOUSEHOOKSTRUCT *mhookstruct = (MOUSEHOOKSTRUCT*)lParam;
	POINT pt = mhookstruct->pt;

	if (keyCode == 1) {
		if (wParam == WM_LBUTTONUP) {
			cout << "MouseAction " << pt.x << " " << pt.y << endl;
		}
	}
	else if (keyCode == 2) {
		if (wParam == WM_RBUTTONUP) {
			cout << "MouseAction " << pt.x << " " << pt.y << endl;
		}
	}

	return CallNextHookEx(NULL, nCode, wParam, lParam);
}

LRESULT CALLBACK mykeyboard(int nCode, WPARAM wParam, LPARAM lParam)
{
	PKBDLLHOOKSTRUCT pKeyboardHookStruct = (PKBDLLHOOKSTRUCT)lParam;

	if (wParam == WM_KEYUP) {
		if (pKeyboardHookStruct->vkCode == keyCode) {
			cout << "KeyboardAction" << endl;
		}
	}

	return CallNextHookEx(NULL, nCode, wParam, lParam);
}


int main(int argc, char* argv[])
{

	if (argc != 3) {
		return -1;
	}

	HHOOK hook = 0;
	int actID = atoi(argv[1]);
	keyCode = atoi(argv[2]);
	if (actID == 1) {
		//鼠标 keyCode=1代表左键 keyCode=2代表右键
		hook = SetWindowsHookEx(WH_MOUSE_LL, mymouse, 0, 0);
	}
	else if (actID == 2) {
		//键盘 keyCode代表对应键的ASCII码
		hook = SetWindowsHookEx(WH_KEYBOARD_LL, mykeyboard, 0, 0);
	}

	if (hook == NULL) {
		cout << "hookFailed" << endl;
	}

	MSG msg;
	while (GetMessage(&msg, NULL, NULL, NULL))
	{
	}
	UnhookWindowsHookEx(hook);
	return 0;
}

