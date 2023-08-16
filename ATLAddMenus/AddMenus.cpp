// AddMenus.cpp : CAddMenus 的实现

#include "stdafx.h"
#include "AddMenus.h"
#include "shlobj.h"
#include "comdef.h"

// CAddMenus

HRESULT CAddMenus::Initialize(LPCITEMIDLIST pidlFolder, LPDATAOBJECT pDataObj, HKEY hProgID)
{
	{
		FORMATETC fmt = { CF_HDROP, NULL, DVASPECT_CONTENT, -1, TYMED_HGLOBAL };
		STGMEDIUM stg = { TYMED_HGLOBAL };
		HDROP hDrop;
		TCHAR szFile[MAX_PATH];
		int nNumFiles;

		// 在数据对象内查找 CF_HDROP 型数据.
		if (FAILED(pDataObj->GetData(&fmt, &stg)))
		{
			// Nope! Return an "invalid argument" error back to Explorer.
			return E_INVALIDARG;
		}

		// 获得指向实际数据的指针
		hDrop = (HDROP)GlobalLock(stg.hGlobal);

		// 检查非NULL.
		if (NULL == hDrop)
		{
			return E_INVALIDARG;
		}

		// 检查在该操作中有几个文件被选择.
		nNumFiles = DragQueryFile(hDrop, 0xFFFFFFFF, NULL, 0);

		if (0 == nNumFiles)
		{
			GlobalUnlock(stg.hGlobal);
			ReleaseStgMedium(&stg);
			return E_INVALIDARG;
		}

		/*// 有效性检查 – 保证最少有一个文件名.
		UINT uNumFiles = DragQueryFile ( hDrop, 0xFFFFFFFF, NULL, 0 );
		if ( 0 == uNumFiles )
		{
		GlobalUnlock ( stg.hGlobal );
		ReleaseStgMedium ( &stg );
		return E_INVALIDARG;
		} */
		std::
		TCHAR  m_mapInt2StrFiles[200][260];
		for (int uFile = 0; uFile < nNumFiles; uFile++)
		{
			//取得下一个文件名.
			if (0 == DragQueryFile(hDrop,
				uFile, szFile, MAX_PATH))
				continue;

			//m_lsFiles.AddTail(szFile);
			
			int i = 0;
			for each (TCHAR var in szFile)
			{
				m_mapInt2StrFiles[uFile][i] = var;
				i++;
			}
			

		} // end for

		GlobalUnlock(stg.hGlobal);
		ReleaseStgMedium(&stg);
		return (sizeof( m_mapInt2StrFiles) > 0) ? S_OK : E_INVALIDARG;
	}
}

//这个函数用来抓取所有选中文件的绝对路径，保存到m_mapInt2StrFiles这个MAP里(MAP用法就不细说了，查阅STL相关内容)

HRESULT CAddMenus::QueryContextMenu(HMENU hmenu, UINT uMenuIndex, UINT uidFirstCmd, UINT uidLastCmd, UINT uFlags)
{
	UINT uCmdID = uidFirstCmd;

	char *szMenuText_Popup = "自定义菜单";
	char *szMenuText_1 = "自定义菜单1...";
	char *szMenuText_2 = "自定义菜单2...";
	char *szMenuText_3 = "自定义菜单3...";
	char *szMenuText_4 = "自定义菜单4...";

	// 如果标志包含 CMF_DEFAULTONLY 我们不作任何事情.
	if (uFlags & CMF_DEFAULTONLY)
	{
		return MAKE_HRESULT(SEVERITY_SUCCESS, FACILITY_NULL, 0);
	}

	InsertMenu(hmenu, uMenuIndex, MF_SEPARATOR | MF_BYPOSITION, 0, NULL);
	uMenuIndex++;

	HMENU hSubMenu = CreateMenu();

	if (hSubMenu)
	{
		InsertMenu(hSubMenu, 0, MF_STRING | MF_BYPOSITION, uCmdID++, szMenuText_1);
		SetMenuItemBitmaps(hSubMenu, 0, MF_BYPOSITION, m_hRegBmp, m_hRegBmp);

		InsertMenu(hSubMenu, 1, MF_STRING | MF_BYPOSITION, uCmdID++, szMenuText_2);
		SetMenuItemBitmaps(hSubMenu, 1, MF_BYPOSITION, m_hRegBmp, m_hRegBmp);

		//InsertMenu(hSubMenu, 2, MF_SEPARATOR | MF_BYPOSITION, 0, NULL);

		InsertMenu(hSubMenu, 2, MF_STRING | MF_BYPOSITION, uCmdID++, szMenuText_3);
		SetMenuItemBitmaps(hSubMenu, 2, MF_BYPOSITION, m_hRegBmp, m_hRegBmp);

		InsertMenu(hSubMenu, 3, MF_STRING | MF_BYPOSITION, uCmdID++, szMenuText_4);
		SetMenuItemBitmaps(hSubMenu, 3, MF_BYPOSITION, m_hRegBmp, m_hRegBmp);
	}

	InsertMenu(hmenu, uMenuIndex, MF_STRING | MF_POPUP | MF_BYPOSITION, (UINT_PTR)hSubMenu, szMenuText_Popup);
	uMenuIndex++;

	InsertMenu(hmenu, uMenuIndex, MF_SEPARATOR | MF_BYPOSITION, 0, NULL);
	uMenuIndex++;

	//最后告诉浏览器我们添加了几个菜单项
	return MAKE_HRESULT(SEVERITY_SUCCESS, FACILITY_NULL, uCmdID);
}
//这个函数就是添加菜单项的函数。
//InsertMenu(hmenu, uMenuIndex, MF_SEPARATOR | MF_BYPOSITION, 0, NULL); /*这一句加的是一个空菜单，显示时就是一个菜单里的分隔符。*/
//SetMenuItemBitmaps(hSubMenu, 2, MF_BYPOSITION, m_hRegBmp, m_hRegBmp); /*这一句是设定菜单所对应的图标，可在资源中添加一张BMP图，ID设为ID_BMP1，然后在构造函数里加如下代码：*/
//m_hRegBmp = LoadBitmap(_Module.GetModuleInstance(),
//MAKEINTRESOURCE(IDB_GREATSKYBMP));

//在头文件里加：HBITMAP m_hRegBmp;

//HRESULT CDesPdm::GetCommandString(UINT idCmd, UINT uFlags, UINT* pwReserved, LPSTR pszName, UINT cchMax)
HRESULT CAddMenus::GetCommandString(UINT idCmd, UINT uFlags, UINT* pwReserved, LPSTR pszName, UINT cchMax)

{
	USES_CONVERSION;
	LPCTSTR szPrompt;

	// 如果 Explorer 要求帮助字符串，就将它拷贝到提供的缓冲区中.
	if (uFlags & GCS_HELPTEXT)
	{
		switch (idCmd)
		{
		case 0:
			szPrompt = _T("自定义菜单1");
			break;

		case 1:
			szPrompt = _T("自定义菜单2");
			break;

		case 2:
			szPrompt = _T("自定义菜单3");
			break;

		case 3:
			szPrompt = _T("自定义菜单4");
			break;

		default:
			ATLASSERT(0);           // should never get here
			return E_INVALIDARG;
			break;
		}
		if (uFlags & GCS_UNICODE)
		{
			// 我们需要将 pszName 转化为一个 Unicode 字符串, 接着使用Unicode字符串拷贝 API.
			lstrcpynW((LPWSTR)pszName, T2CW(szPrompt), cchMax);
		}
		else
		{
			// 使用 ANSI 字符串拷贝API 来返回帮助字符串.
			lstrcpynA(pszName, T2CA(szPrompt), cchMax);
		}

		return S_OK;
	}

	return E_INVALIDARG;
}
//这个函数是响应资源管理器左下角的帮助信息

//HRESULT CDesPdm::InvokeCommand(LPCMINVOKECOMMANDINFO pCmdInfo)
HRESULT CAddMenus::InvokeCommand(LPCMINVOKECOMMANDINFO pCmdInfo)
{
	//此语句用来正确地切换MFC模块状态
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	CString strCmd = "";

	// 如果lpVerb 实际指向一个字符串, 忽略此次调用并退出.
	if (0 != HIWORD(pCmdInfo->lpVerb))
		return E_INVALIDARG;
	// 点击的命令索引 – 在这里，唯一合法的索引为0.
	switch (LOWORD(pCmdInfo->lpVerb))
	{
	case 0:
	{
			  //执行自定义菜单1的操作
			  break;
	}
	case 1:
	{
			  //执行自定义菜单2的操作
			  break;
	}
	case 2:
	{
			  //执行自定义菜单3的操作
			  break;
	}
	case 3:
	{
			  //执行自定义菜单4的操作
			  break;
	}
	default:
	{
			   return E_INVALIDARG;
			   break;
	}
	}
	return S_OK;
}
