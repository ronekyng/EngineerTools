// AddMenus.h : CAddMenus 的声明

#pragma once
#include "resource.h"       // 主符号



#include "ATLAddMenus_i.h"
#include "shlobj.h"
#include "comdef.h"


#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Windows CE 平台(如不提供完全 DCOM 支持的 Windows Mobile 平台)上无法正确支持单线程 COM 对象。定义 _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA 可强制 ATL 支持创建单线程 COM 对象实现并允许使用其单线程 COM 对象实现。rgs 文件中的线程模型已被设置为“Free”，原因是该模型是非 DCOM Windows CE 平台支持的唯一线程模型。"
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
