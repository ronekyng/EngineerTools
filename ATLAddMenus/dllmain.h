// dllmain.h : 模块类的声明。

class CATLAddMenusModule : public ATL::CAtlDllModuleT< CATLAddMenusModule >
{
public :
	DECLARE_LIBID(LIBID_ATLAddMenusLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_ATLADDMENUS, "{8DC8E456-9BF4-4778-9855-D23DB9EBEE8F}")
};

extern class CATLAddMenusModule _AtlModule;
