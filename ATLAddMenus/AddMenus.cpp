// AddMenus.cpp : CAddMenus ��ʵ��

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

		// �����ݶ����ڲ��� CF_HDROP ������.
		if (FAILED(pDataObj->GetData(&fmt, &stg)))
		{
			// Nope! Return an "invalid argument" error back to Explorer.
			return E_INVALIDARG;
		}

		// ���ָ��ʵ�����ݵ�ָ��
		hDrop = (HDROP)GlobalLock(stg.hGlobal);

		// ����NULL.
		if (NULL == hDrop)
		{
			return E_INVALIDARG;
		}

		// ����ڸò������м����ļ���ѡ��.
		nNumFiles = DragQueryFile(hDrop, 0xFFFFFFFF, NULL, 0);

		if (0 == nNumFiles)
		{
			GlobalUnlock(stg.hGlobal);
			ReleaseStgMedium(&stg);
			return E_INVALIDARG;
		}

		/*// ��Ч�Լ�� �C ��֤������һ���ļ���.
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
			//ȡ����һ���ļ���.
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

//�����������ץȡ����ѡ���ļ��ľ���·�������浽m_mapInt2StrFiles���MAP��(MAP�÷��Ͳ�ϸ˵�ˣ�����STL�������)

HRESULT CAddMenus::QueryContextMenu(HMENU hmenu, UINT uMenuIndex, UINT uidFirstCmd, UINT uidLastCmd, UINT uFlags)
{
	UINT uCmdID = uidFirstCmd;

	char *szMenuText_Popup = "�Զ���˵�";
	char *szMenuText_1 = "�Զ���˵�1...";
	char *szMenuText_2 = "�Զ���˵�2...";
	char *szMenuText_3 = "�Զ���˵�3...";
	char *szMenuText_4 = "�Զ���˵�4...";

	// �����־���� CMF_DEFAULTONLY ���ǲ����κ�����.
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

	//�������������������˼����˵���
	return MAKE_HRESULT(SEVERITY_SUCCESS, FACILITY_NULL, uCmdID);
}
//�������������Ӳ˵���ĺ�����
//InsertMenu(hmenu, uMenuIndex, MF_SEPARATOR | MF_BYPOSITION, 0, NULL); /*��һ��ӵ���һ���ղ˵�����ʾʱ����һ���˵���ķָ�����*/
//SetMenuItemBitmaps(hSubMenu, 2, MF_BYPOSITION, m_hRegBmp, m_hRegBmp); /*��һ�����趨�˵�����Ӧ��ͼ�꣬������Դ�����һ��BMPͼ��ID��ΪID_BMP1��Ȼ���ڹ��캯��������´��룺*/
//m_hRegBmp = LoadBitmap(_Module.GetModuleInstance(),
//MAKEINTRESOURCE(IDB_GREATSKYBMP));

//��ͷ�ļ���ӣ�HBITMAP m_hRegBmp;

//HRESULT CDesPdm::GetCommandString(UINT idCmd, UINT uFlags, UINT* pwReserved, LPSTR pszName, UINT cchMax)
HRESULT CAddMenus::GetCommandString(UINT idCmd, UINT uFlags, UINT* pwReserved, LPSTR pszName, UINT cchMax)

{
	USES_CONVERSION;
	LPCTSTR szPrompt;

	// ��� Explorer Ҫ������ַ������ͽ����������ṩ�Ļ�������.
	if (uFlags & GCS_HELPTEXT)
	{
		switch (idCmd)
		{
		case 0:
			szPrompt = _T("�Զ���˵�1");
			break;

		case 1:
			szPrompt = _T("�Զ���˵�2");
			break;

		case 2:
			szPrompt = _T("�Զ���˵�3");
			break;

		case 3:
			szPrompt = _T("�Զ���˵�4");
			break;

		default:
			ATLASSERT(0);           // should never get here
			return E_INVALIDARG;
			break;
		}
		if (uFlags & GCS_UNICODE)
		{
			// ������Ҫ�� pszName ת��Ϊһ�� Unicode �ַ���, ����ʹ��Unicode�ַ������� API.
			lstrcpynW((LPWSTR)pszName, T2CW(szPrompt), cchMax);
		}
		else
		{
			// ʹ�� ANSI �ַ�������API �����ذ����ַ���.
			lstrcpynA(pszName, T2CA(szPrompt), cchMax);
		}

		return S_OK;
	}

	return E_INVALIDARG;
}
//�����������Ӧ��Դ���������½ǵİ�����Ϣ

//HRESULT CDesPdm::InvokeCommand(LPCMINVOKECOMMANDINFO pCmdInfo)
HRESULT CAddMenus::InvokeCommand(LPCMINVOKECOMMANDINFO pCmdInfo)
{
	//�����������ȷ���л�MFCģ��״̬
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	CString strCmd = "";

	// ���lpVerb ʵ��ָ��һ���ַ���, ���Դ˴ε��ò��˳�.
	if (0 != HIWORD(pCmdInfo->lpVerb))
		return E_INVALIDARG;
	// ������������� �C �����Ψһ�Ϸ�������Ϊ0.
	switch (LOWORD(pCmdInfo->lpVerb))
	{
	case 0:
	{
			  //ִ���Զ���˵�1�Ĳ���
			  break;
	}
	case 1:
	{
			  //ִ���Զ���˵�2�Ĳ���
			  break;
	}
	case 2:
	{
			  //ִ���Զ���˵�3�Ĳ���
			  break;
	}
	case 3:
	{
			  //ִ���Զ���˵�4�Ĳ���
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
