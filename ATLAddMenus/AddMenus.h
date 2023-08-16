// AddMenus.h : CAddMenus ������

#pragma once
#include "resource.h"       // ������



#include "ATLAddMenus_i.h"
#include "shlobj.h"
#include "comdef.h"


#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Windows CE ƽ̨(�粻�ṩ��ȫ DCOM ֧�ֵ� Windows Mobile ƽ̨)���޷���ȷ֧�ֵ��߳� COM ���󡣶��� _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA ��ǿ�� ATL ֧�ִ������߳� COM ����ʵ�ֲ�����ʹ���䵥�߳� COM ����ʵ�֡�rgs �ļ��е��߳�ģ���ѱ�����Ϊ��Free����ԭ���Ǹ�ģ���Ƿ� DCOM Windows CE ƽ̨֧�ֵ�Ψһ�߳�ģ�͡�"
#endif

using namespace ATL;


// CAddMenus

class ATL_NO_VTABLE CAddMenus :

	public IShellExtInit,
	public IContextMenu,
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CAddMenus, &CLSID_AddMenus>,
	public IDispatchImpl<IAddMenus, &IID_IAddMenus, &LIBID_ATLAddMenusLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
	CAddMenus()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_ADDMENUS)


BEGIN_COM_MAP(CAddMenus)
	COM_INTERFACE_ENTRY(IShellExtInit)
	COM_INTERFACE_ENTRY(IContextMenu)
	COM_INTERFACE_ENTRY(IAddMenus)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()



	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

//public:

	// IDesPdm
protected:
	TCHAR m_szFile[MAX_PATH];
public:
	// IShellExtInit
	STDMETHOD(Initialize)(LPCITEMIDLIST, LPDATAOBJECT, HKEY);
public:
	// IContextMenu
	STDMETHOD(GetCommandString)(UINT, UINT, UINT*, LPSTR, UINT);
	STDMETHOD(InvokeCommand)(LPCMINVOKECOMMANDINFO);
	STDMETHOD(QueryContextMenu)(HMENU, UINT, UINT, UINT, UINT);
	
};

OBJECT_ENTRY_AUTO(__uuidof(AddMenus), CAddMenus)
